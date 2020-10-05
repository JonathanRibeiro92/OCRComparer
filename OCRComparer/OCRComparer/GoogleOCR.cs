using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using Newtonsoft.Json.Linq;

namespace OCRComparer
{
    public static class GoogleOCR
    {
        public static async Task<string> DetectText(string imageFilePath)
        {
            string credentialsPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            var image = Image.FromFile(imageFilePath);
            ImageAnnotatorClientBuilder builder = new ImageAnnotatorClientBuilder
            {
                CredentialsPath = credentialsPath
            };

            ImageAnnotatorClient client = builder.Build();
            var response = await client.DetectTextAsync(image);

            string contentString = response.ToString();

            return JToken.Parse(contentString).ToString();
            

            //foreach (var annotation in response)
            //{
            //    if (annotation.Description != null)
            //        Console.WriteLine(annotation.Description);
            //}
        }
    }
}
