using System;

namespace OCRComparer
{
    class Program
    {
        static async void Main(string[] args)
        {
            Console.WriteLine("\nExtracting text...\n");

            string imageFilePath = "E:\\TCC\\YUVA EB DATASET-20200928T152820Z-001\\YUVA EB DATASET\\RAW IMAGES\\1)Day Light\\32.JPG";

            string jsonAzure = await AzureOCR.MakeOCRRequest(imageFilePath);
            string jsonAmazon = await AmazonOCR.MakeOCRRequest(imageFilePath);
            string jsonGoogle =  GoogleOCR.DetectText(imageFilePath);



            //IBMWatsonOCR.DetectText(imageFilePath);

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
