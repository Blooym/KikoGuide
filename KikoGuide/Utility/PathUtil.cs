using System.IO;

namespace KikoGuide.Utility
{
    internal static class PathUtil
    {
        /// <summary>
        /// Creates a path if it doesn't exist.
        /// </summary>
        /// <param name="path">The path to create.</param>
        public static void CreatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}