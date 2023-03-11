using System.IO;

namespace ExHentaiDownloader3.Helpers
{
    public static class FileNameHelper
    {
        public static string CleanFileName(string filename)
        {
            return CleanInvalidChars(Path.GetInvalidPathChars(), CleanInvalidChars(Path.GetInvalidFileNameChars(), filename));
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
