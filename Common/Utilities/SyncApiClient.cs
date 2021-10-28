using System.Net.Http;
using System.Text;

namespace Common.Utilities
{
    public class SyncApiClient
    {
        private static readonly HttpClient _client = new HttpClient();

        public static HttpResponseMessage SendJson(string json, string url, string httpMethod)
        {
            var httpMethd = new HttpMethod(httpMethod.ToUpper());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(httpMethd, url)
            {
                Content = content
            };

            var task = _client.SendAsync(request);
            task.Wait();

            return task.Result;
        }
    }
}