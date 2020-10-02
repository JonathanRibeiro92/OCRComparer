using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OCRComparer
{
    public static class Util
    {
        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        public static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }


        public static MemoryStream GetImageAsMemoryStream(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a Memory Stream.
                byte[] data = null;
                data = new byte[fileStream.Length];
                fileStream.Read(data, 0, (int)fileStream.Length);
                return new MemoryStream(data);
            }
        }
    }
}
