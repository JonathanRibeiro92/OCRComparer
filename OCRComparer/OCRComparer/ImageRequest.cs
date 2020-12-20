using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OCRComparer
{
    public static class ImageRequest
    {
        public static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        public static MemoryStream GetImageAsMemoryStream(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                byte[] data = null;
                data = new byte[fileStream.Length];
                fileStream.Read(data, 0, (int)fileStream.Length);
                return new MemoryStream(data);
            }
        }

    }
}
