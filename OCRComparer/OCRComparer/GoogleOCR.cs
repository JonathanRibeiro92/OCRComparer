using System;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using Newtonsoft.Json.Linq;

namespace OCRComparer
{
    public static class GoogleOCR
    {
        public static async Task<string> MakeOCRRequest(string imageFilePath)
        {
            string credentialsPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            var image = Image.FromFile(imageFilePath);
            ImageAnnotatorClientBuilder builder = new ImageAnnotatorClientBuilder
            {
                CredentialsPath = credentialsPath
            };
            try
            {
                ImageAnnotatorClient client = builder.Build();
                var response = await client.DetectTextAsync(image);
                string contentString = response.ToString();

                return JToken.Parse(contentString).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                return "error";
            }
        }
    }
}
