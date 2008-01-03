using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dope.DDXX.Utility
{
    public class FileUtility
    {
        private static string[] paths = new string[] { "./" };
        private static string blockFile;

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
            FileUtility.blockFile = file;
        }

        public static string GetBlockFile()
        {
            return blockFile;
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

        public static FileStream OpenStream(string file)
        {
            foreach (string path in paths)
            {
                try { 
                    FileStream stream = new FileStream(path + file, FileMode.Open, FileAccess.Read);
                    return stream;
                }
                catch (FileNotFoundException) { }
            }
            throw new DDXXException("File not found: " + file);
        }

    }
}
