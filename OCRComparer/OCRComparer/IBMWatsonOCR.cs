using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Cloud.SDK.Core.Http;
using IBM.Watson.VisualRecognition;
using IBM.Watson.VisualRecognition.v3;
using IBM.Watson.VisualRecognition.v3.Model;

namespace OCRComparer
{
    public static class IBMWatsonOCR
    {


        public static object DetectText(string imageFilePath)
        {
            string visualRecognitionApiKey = Environment.GetEnvironmentVariable("VISUAL_RECOGNITION_APIKEY");
            string visualRecognitionIAMApiKey = Environment.GetEnvironmentVariable("VISUAL_RECOGNITION_IAM_APIKEY");
            string visualRecognitionURL = Environment.GetEnvironmentVariable("VISUAL_RECOGNITION_URL");


            IamAuthenticator authenticator = new IamAuthenticator(apikey: visualRecognitionApiKey);

            VisualRecognitionService visualRecognitionService = new VisualRecognitionService(DateTime.Now.ToString("yyyy-MM-dd"), authenticator);
            visualRecognitionService.SetServiceUrl(visualRecognitionURL);

            DetailedResponse<ClassifiedImages> result = null;
            using (FileStream image = File.OpenRead(imageFilePath))
            {
                using (MemoryStream imageMemoryStream = new MemoryStream())
                {
                    image.CopyTo(imageMemoryStream);
                    string imageFileName = imageFilePath.Split('\\')[imageFilePath.Split('\\').Length-1];
                    Dictionary<string, MemoryStream> positiveExamples = new Dictionary<string, MemoryStream>();
                    result = visualRecognitionService.Classify(imageMemoryStream, imageFileName, null, null, null, null, null, null);

                }
            }

            Console.WriteLine(result.Response);

            return 0;
        }







    }
}
