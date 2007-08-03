using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dope.DDXX.Utility
{
    public class FileUtility
    {
        private class BlockFileInfo
        {
            public string Filename;
            public string Pathname;
            public long Size;
            public long Offset;
        }

        private static string[] paths = new string[] { };
        private static FileStream blockReadStream;
        private static BinaryReader blockReader;
        private static List<BlockFileInfo> blockFileLayout = new List<BlockFileInfo>();

        public static void SetLoadPaths(params string[] paths)
        {
            FileUtility.paths = paths;
        }

        public static string[] GetLoadPaths()
        {
            return paths;
        }

        public static void SetBlockFile(string file)
        {
            try
            {
                blockReadStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                blockReader = new BinaryReader(blockReadStream);
                LoadBlockFileLayout();
            }
            catch (FileNotFoundException exception)
            {
            }
        }

        public static string FilePath(string file)
        {
            foreach (string path in paths)
            {
                if (File.Exists(path + file))
                    return path + file;
            }
            throw new DDXXException("Could not find file \"" + file + "\" in search paths.");
        }

        public static Stream OpenStream(string file)
        {
            if (blockReader != null)
            {
                foreach (BlockFileInfo info in blockFileLayout)
                {
                    if (info.Filename == file)
                        return ReadFromBlockFile(info);
                }
            }
            foreach (string path in paths)
            {
                try { 
                    FileStream stream = new FileStream(path + file, FileMode.Open, FileAccess.Read);
                    SaveToBlockFile(file, path, stream);
                    return stream;
                }
                catch (FileNotFoundException) { }
            }
            throw new DDXXException("File not found: " + file);
        }

        private static Stream ReadFromBlockFile(BlockFileInfo info)
        {
            blockReadStream.Seek(info.Offset, SeekOrigin.Begin);
            byte[] data = blockReader.ReadBytes((int)info.Size);
            MemoryStream stream = new MemoryStream(data);
            return stream;
        }

        private static void SaveToBlockFile(string file, string path, FileStream stream)
        {
            BlockFileInfo item = new BlockFileInfo();
            item.Filename = file;
            item.Pathname = path;
            item.Size = stream.Length;
            blockFileLayout.Add(item);
        }

        public static void SaveBlockFile(string fileName)
        {
            FileStream writeStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(writeStream);
            writer.Write(blockFileLayout.Count);
            foreach (BlockFileInfo info in blockFileLayout)
            {
                writer.Write(info.Filename);
                writer.Write(info.Size);
            }
            foreach (BlockFileInfo info in blockFileLayout)
            {
                FileStream read = new FileStream(info.Pathname + info.Filename, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(read);
                byte[] data = reader.ReadBytes((int)info.Size);
                writer.Write(data);
            }
            writeStream.Close();
        }

        private static void LoadBlockFileLayout()
        {
            int numEntries;
            numEntries = blockReader.ReadInt32();
            for (int i = 0; i < numEntries; i++)
            {
                BlockFileInfo info = new BlockFileInfo();
                info.Filename = blockReader.ReadString();
                info.Size = blockReader.ReadInt64();
                blockFileLayout.Add(info);
            }
            long offset = blockReadStream.Position;
            for (int i = 0; i < numEntries; i++)
            {
                blockFileLayout[i].Offset = offset;
                offset += blockFileLayout[i].Size;
            }
        }

    }
}
