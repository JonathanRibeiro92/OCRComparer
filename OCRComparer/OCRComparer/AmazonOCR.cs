using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;


namespace OCRComparer
{
    public static class AmazonOCR
    {
        static string stringAccessKeyId = Environment.GetEnvironmentVariable("STRING_ACCESS_KEY_ID");
        static string stringSecretAccessKey = Environment.GetEnvironmentVariable("STRING_SECRET_ACCESS_KEY");


        static AmazonRekognitionClient client = new AmazonRekognitionClient(stringAccessKeyId, stringSecretAccessKey);



        public static async Task MakeOCRRequest(string imageFilePath)
        {
            try
            {
                Image image = new Image();
                image.Bytes = Util.GetImageAsMemoryStream(imageFilePath);

                DetectTextRequest detectTextRequest = new DetectTextRequest()
                {
                    Image = image
                    
                };

                DetectTextResponse detectTextResponse = await client.DetectTextAsync(detectTextRequest);
                Console.WriteLine("Detected lines and words for " + imageFilePath);
                foreach (TextDetection text in detectTextResponse.TextDetections)
                {
                    Console.WriteLine("Detected: " + text.DetectedText);
                    Console.WriteLine("Confidence: " + text.Confidence);
                    Console.WriteLine("Id : " + text.Id);
                    Console.WriteLine("Parent Id: " + text.ParentId);
                    Console.WriteLine("Type: " + text.Type);
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
