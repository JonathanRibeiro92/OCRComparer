using System;
using System.IO;
using System.Linq;
using System.Net.Http;
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
                form.Add(new StringContent(OCRSpaceAPIKey), "apikey");
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
                return strContent;

            }
            catch (Exception exception)
            {
                Console.WriteLine("\n" + exception.Message);
                return "error";
            }
        }
    }
}
