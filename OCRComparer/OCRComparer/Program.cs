using System;

namespace OCRComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nExtracting text...\n");

            string imageFilePath = "E:\\TCC\\YUVA EB DATASET-20200928T152820Z-001\\YUVA EB DATASET\\RAW IMAGES\\1)Day Light\\32.JPG";

            //AzureOCR.MakeOCRRequest(imageFilePath).Wait();
            //AmazonOCR.MakeOCRRequest(imageFilePath).Wait();
            GoogleOCR.DetectText(imageFilePath);
            
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
