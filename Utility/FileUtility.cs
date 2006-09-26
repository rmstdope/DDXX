using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public class FileUtility
    {
        private static string[] paths;

        public static void Initialize(params string[] paths)
        {
            FileUtility.paths = paths;
        }

        public static string[] LoadPaths()
        {
            return paths;
        }

        public static string FilePath(string file)
        {
            foreach (string path in paths)
            {
                if (System.IO.File.Exists(path + file))
                    return path + file;
            }
            throw new DDXXException("Could not find file \"" + file + "\" in search paths.");
        }
    }
}
