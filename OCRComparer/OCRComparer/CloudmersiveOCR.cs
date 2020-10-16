using System;
using Cloudmersive.APIClient.NETCore.OCR.Api;
using Cloudmersive.APIClient.NETCore.OCR.Model;
using Cloudmersive.APIClient.NETCore.OCR.Client;
using System.Threading.Tasks;
using System.IO;

namespace OCRComparer
{
	public static class CloudmersiveOCR
	{
		static string ApiKey = System.Environment.GetEnvironmentVariable("CLOUDMERSIVE_API_KEY");

		public static async Task<string> MakeOCRRequest(string imageFilePath)
		{
			Configuration.Default.AddApiKey("Apikey", ApiKey);
			var apiInstance = new ImageOcrApi();
			var imageFile = new FileStream(imageFilePath, FileMode.Open);


			try
            {
				var result = await apiInstance.ImageOcrPhotoWordsWithLocationAsync(imageFile);
				return result.ToJson();
			}
            catch (Exception exception)
            {
                Console.WriteLine("\n" + exception.Message);
				return "erro";
			}
		}
	}

}
