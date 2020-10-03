using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.Vision.V1;
using Newtonsoft.Json.Linq;

namespace OCRComparer
{
    public static class GoogleOCR
    {
        public static object DetectText(string imageFilePath)
        {
            string credentialsPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            var image = Image.FromFile(imageFilePath);
            ImageAnnotatorClientBuilder builder = new ImageAnnotatorClientBuilder
            {
                CredentialsPath = credentialsPath
            };

            ImageAnnotatorClient client = builder.Build();
            var response = client.DetectText(image);

            string contentString = response.ToString();

            Console.WriteLine(JToken.Parse(contentString).ToString());
            

            //foreach (var annotation in response)
            //{
            //    if (annotation.Description != null)
            //        Console.WriteLine(annotation.Description);
            //}
            return 0;
        }
    }
}
