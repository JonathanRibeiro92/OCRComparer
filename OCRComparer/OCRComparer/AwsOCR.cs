using System;
using System.Threading.Tasks;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;

namespace OCRComparer
{
    public static class AwsOCR
    {
        static string stringAccessKeyId = Environment.GetEnvironmentVariable("STRING_ACCESS_KEY_ID");
        static string stringSecretAccessKey = Environment.GetEnvironmentVariable("STRING_SECRET_ACCESS_KEY");
        static AmazonRekognitionClient client = new AmazonRekognitionClient(stringAccessKeyId, stringSecretAccessKey);

        public static async Task<string> MakeOCRRequest(string imageFilePath)
        {
            try
            {
                Image image = new Image();
                image.Bytes = ImageRequest.GetImageAsMemoryStream(imageFilePath);

                DetectTextRequest detectTextRequest = new DetectTextRequest()
                {
                    Image = image
                    
                };

                DetectTextResponse detectTextResponse = await client.DetectTextAsync(detectTextRequest);
                string json = System.Text.Json.JsonSerializer.Serialize(detectTextResponse.TextDetections);
                return json;
            }
            catch (Exception)
            {
                return "error";
            }
        }
    }

}
