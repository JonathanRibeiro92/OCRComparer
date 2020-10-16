using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Abbyy.CloudSdk.V2.Client;
using Abbyy.CloudSdk.V2.Client.Models;
using Abbyy.CloudSdk.V2.Client.Models.Enums;
using Abbyy.CloudSdk.V2.Client.Models.RequestParams;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace OCRComparer
{
    public static class AbbyyOCR
    {
        private const string ApplicationId = @"aplicationID";
        private const string Password = @"Password";

        private const string ServiceUrl = "https://cloud-westus.ocrsdk.com";

        private static int _retryCount = 3;
        private static int _delayBetweenRetriesInSeconds = 3;
        private static string _httpClientName = "OCR_HTTP_CLIENT";

        private static ServiceProvider _serviceProvider;
        private static HttpClient _httpClient;

        private static readonly AuthInfo AuthInfo = new AuthInfo
        {
            Host = ServiceUrl,
            ApplicationId = ApplicationId,
            Password = Password
        };

        public static async Task<string> MakeOCRRequest(string imageFilePath)
        {
			using (var ocrClient = GetOcrClientWithRetryPolicy())
			{
				// Process image
				// You could also call ProcessDocumentAsync or any other processing method declared below
				var json = await ProcessImageAsync(ocrClient, imageFilePath);

				return json ;
			}

		}



		private static IOcrClient GetOcrClient()
		{
			return new OcrClient(AuthInfo);
		}

		private static IOcrClient GetOcrClientWithRetryPolicy()
		{
			

			var httpClientHandler = new HttpClientHandler
			{
				PreAuthenticate = true,
				Credentials = new NetworkCredential(AuthInfo.ApplicationId, AuthInfo.Password)
			};
			

			var _httpClient = HttpClientFactory.Create(httpClientHandler);
			_httpClient.BaseAddress = new Uri(AuthInfo.Host);
			_httpClient.Timeout = _httpClient.Timeout + TimeSpan.FromSeconds(_retryCount * _delayBetweenRetriesInSeconds);

			return new OcrClient(_httpClient);
		}

		

		private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
		{
			return HttpPolicyExtensions.HandleTransientHttpError()
				//Condition - what kind of request errors should we repeat
				.OrResult(r => r.StatusCode == HttpStatusCode.GatewayTimeout)
				.WaitAndRetryAsync(
					_retryCount,
					sleepDurationProvider => TimeSpan.FromSeconds(_delayBetweenRetriesInSeconds),
					(exception, calculatedWaitDuration, retries, context) =>
					{
						Console.WriteLine($"Retry {retries} for policy with key {context.PolicyKey}");
					}
				)
				.WithPolicyKey("WaitAndRetryAsync_For_GatewayTimeout_504__StatusCode");
		}

		private static async Task<string> ProcessImageAsync(IOcrClient ocrClient, string filePath)
		{
			var imageParams = new ImageProcessingParams
			{
				ExportFormats = new[] { ExportFormat.Docx, ExportFormat.Txt, },
				Language = "English,French",
			};

			using (var fileStream = new FileStream(filePath, FileMode.Open))
			{
				var taskInfo = await ocrClient.ProcessImageAsync (
					null,
					fileStream,
					Path.GetFileName(filePath),
					waitTaskFinished: true);

				return JsonSerializer.Serialize(taskInfo.ResultUrls);
			}
		}

		private static async Task<List<string>> ProcessDocumentAsync(IOcrClient ocrClient)
		{
			var taskId = await UploadFilesAsync(ocrClient);

			var processingParams = new DocumentProcessingParams
			{
				ExportFormats = new[] { ExportFormat.Docx, ExportFormat.Txt, },
				Language = "English,French",
				TaskId = taskId,
			};

			var taskInfo = await ocrClient.ProcessDocumentAsync(
				processingParams,
				waitTaskFinished: true);

			return taskInfo.ResultUrls;
		}

		private static async Task<Guid> UploadFilesAsync(IOcrClient ocrClient)
		{
			ImageSubmittingParams submitParams;
			var firstFilePath = "processImage.jpg";
			var secondFilePath = "processDocument.jpg";

			// First file
			using (var fileStream = new FileStream(firstFilePath, FileMode.Open))
			{
				var submitImageResult = await ocrClient.SubmitImageAsync(
					null,
					fileStream,
					Path.GetFileName(firstFilePath));

				// Save TaskId for next files and ProcessDocument method
				submitParams = new ImageSubmittingParams { TaskId = submitImageResult.TaskId };
			}

			// Second file
			using (var fileStream = new FileStream(secondFilePath, FileMode.Open))
			{
				await ocrClient.SubmitImageAsync(
					submitParams,
					fileStream,
					Path.GetFileName(secondFilePath));
			}

			return submitParams.TaskId.Value;
		}

		private static async Task<TaskList> GetFinishedTasksAsync(IOcrClient ocrClient)
		{
			var finishedTasks = await ocrClient.ListFinishedTasksAsync();
			return finishedTasks;
		}

		private static void DisposeServices()
		{
			(_serviceProvider as IDisposable)?.Dispose();
		}
	}

	public class HttpClientRetryPolicyHandler : DelegatingHandler
	{
		private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

		/// <summary>Initializes a new instance of the <see cref="HttpClientRetryPolicyHandler"/> class.</summary>
		public HttpClientRetryPolicyHandler(IAsyncPolicy<HttpResponseMessage> retryPolicy)
		{
			_retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
		}

		/// <inheritdoc />
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellation) =>
			_retryPolicy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellation);
	}
}
