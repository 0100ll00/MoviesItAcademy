using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MoviesItAcademy.Worker
{
    public class ApiClient
    {
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(ILogger<ApiClient> logger)
        {
            _logger = logger;
        }

        public async Task MoveMoviesToArchive()
        {
            var url = "https://localhost:44328/worker/moviedelition";
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            using var client = new HttpClient(httpClientHandler);
            try
            {
                var response = await client.PostAsync(url, null);
                string result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        public async Task CancelExpiredBookings()
        {
            var url = "https://localhost:44328/worker/bookingdelition";
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            using var client = new HttpClient(httpClientHandler);
            try
            {
                var response = await client.PostAsync(url, null);
                string result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
