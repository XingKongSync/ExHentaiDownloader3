using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Helpers
{
    public static class FileNameHelper
    {
        public static string CleanFileName(string filename)
        {
            return CleanInvalidChars(Path.GetInvalidFileNameChars(), filename);
        }

        public static string CleanDirectoryName(string dir)
        {
            return CleanInvalidChars(Path.GetInvalidPathChars(), dir);
        }

        public static string CleanInvalidChars(char[] invalidChars, string path)
        {
            foreach (char c in invalidChars)
            {
                path = path.Replace(c, ' ');
            }
            return path;
        }
    }
}
