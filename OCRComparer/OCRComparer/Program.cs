using System;
using System.IO;
using System.Linq;

namespace OCRComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nExtracting text...\n");

            string targetDirectory = "E:\\TCC\\YUVA EB DATASET-20200928T152820Z-001\\YUVA EB DATASET\\RAW IMAGES";

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                string[] files = Directory.GetFiles(subdirectory, "*.JPG");
                foreach (var file in files)
                {
                    string pathJsonResult = "E:\\TCC\\source\\OCRComparer\\results\\";
                    string fileName = file.Split('\\').Last();
                    string directoryName = subdirectory.Split('\\').Last();
                    string nomeArquivoAWSJson = pathJsonResult + "AWS\\" + directoryName + "\\" + fileName.Split('.')[0] + ".json";
                    string nomeArquivoGoogleJson = pathJsonResult + "Google\\" + directoryName + "\\" + fileName.Split('.')[0] + ".json";
                    string nomeArquivoAzureJson = pathJsonResult + "Azure\\" + directoryName + "\\" + fileName.Split('.')[0] + ".json";
                    string nomeArquivoOCRSpaceJson = pathJsonResult + "OCRSpace\\" + directoryName + "\\" + fileName.Split('.')[0] + ".json";
                    string jsonAmazon = AwsOCR.MakeOCRRequest(file).Result;
                    string jsonAzure = AzureOCR.MakeOCRRequest(file).Result;
                    string jsonGoogle = GoogleOCR.DetectText(file).Result;
                    string jsonOCRSpace = OCRSpaceOCR.MakeOCRRequest(file).Result;
                    File.WriteAllText(nomeArquivoAWSJson, jsonAmazon);
                    File.WriteAllText(nomeArquivoAzureJson, jsonAzure);
                    File.WriteAllText(nomeArquivoGoogleJson, jsonGoogle);
                    File.WriteAllText(nomeArquivoOCRSpaceJson, jsonOCRSpace);
                }

            }




            //IBMWatsonOCR.DetectText(imageFilePath);

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
