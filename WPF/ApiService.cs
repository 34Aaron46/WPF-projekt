using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BoostBayWpf
{
    public class ApiService
    {
        private static HttpClient client = new HttpClient();
        private static string baseUrl = "http://localhost:5080/api/";
        private static JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public static async Task<T?> Get<T>(string url)
        {
            var response = await client.GetAsync(baseUrl + url);
            await Ellenorzes(response);
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public static async Task Post(string url, object data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(baseUrl + url, content);
            await Ellenorzes(response);
        }

        public static async Task Put(string url, object? data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(baseUrl + url, content);
            await Ellenorzes(response);
        }

        public static async Task Delete(string url)
        {
            var response = await client.DeleteAsync(baseUrl + url);
            await Ellenorzes(response);
        }

        // ha hibás a válasz akkor kivételt dobunk a szerver üzenetével
        private static async Task Ellenorzes(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string uzenet = await response.Content.ReadAsStringAsync();
                throw new Exception(uzenet);
            }
        }
    }
}
