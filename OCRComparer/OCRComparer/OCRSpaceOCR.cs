using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OCRComparer
{
    public static class OCRSpaceOCR
    {

        static string OCRSpaceAPIKey = Environment.GetEnvironmentVariable("OCR_SPACE_API_KEY");

        public static async Task<string> MakeOCRRequest(string imageFilePath)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(OCRSpaceAPIKey), "apikey"); //Added api key in form data
                form.Add(new StringContent("eng"), "language");

                form.Add(new StringContent("2"), "ocrengine");
                form.Add(new StringContent("true"), "scale");
                form.Add(new StringContent("true"), "istable");

                var listFilePath = imageFilePath.Split("\\");
                string fileName = listFilePath.Last();

                if (string.IsNullOrEmpty(imageFilePath) == false)
                {
                    byte[] imageData = File.ReadAllBytes(imageFilePath);
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", fileName);
                }

                HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);

                string strContent = await response.Content.ReadAsStringAsync();

                //string strResult = string.Empty;

                //Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);


                //if (ocrResult.OCRExitCode == 1)
                //{
                //    for (int i = 0; i < ocrResult.ParsedResults.Length; i++)
                //    {
                //        strResult = strResult + ocrResult.ParsedResults[i].ParsedText;
                //    }
                //}
                //else
                //{
                //    Console.WriteLine("ERROR: " + strContent);
                //}

                return strContent;

            }
            catch (Exception exception)
            {
                Console.WriteLine("\n" + exception.Message);
                return "erro";
            }
        }
    }


    public class Rootobject
    {
        public Parsedresult[] ParsedResults { get; set; }
        public int OCRExitCode { get; set; }
        public bool IsErroredOnProcessing { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }

    public class Parsedresult
    {
        public object FileParseExitCode { get; set; }
        public string ParsedText { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }
}
