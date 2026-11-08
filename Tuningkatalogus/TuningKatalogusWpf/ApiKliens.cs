using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace TuningKatalogusWpf
{
    // az összes API hívás ezen keresztül megy
    public static class ApiKliens
    {
        private static readonly HttpClient kliens = new HttpClient();
        private const string AlapUrl = "http://localhost:5080/api/";

        private static readonly JsonSerializerOptions jsonBeallitas = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<T?> Get<T>(string url)
        {
            var valasz = await kliens.GetAsync(AlapUrl + url);
            await HibaEllenorzes(valasz);
            string json = await valasz.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, jsonBeallitas);
        }

        public static async Task Post(string url, object adat)
        {
            var tartalom = new StringContent(JsonSerializer.Serialize(adat), Encoding.UTF8, "application/json");
            var valasz = await kliens.PostAsync(AlapUrl + url, tartalom);
            await HibaEllenorzes(valasz);
        }

        public static async Task Put(string url, object? adat)
        {
            var tartalom = new StringContent(JsonSerializer.Serialize(adat), Encoding.UTF8, "application/json");
            var valasz = await kliens.PutAsync(AlapUrl + url, tartalom);
            await HibaEllenorzes(valasz);
        }

        public static async Task Delete(string url)
        {
            var valasz = await kliens.DeleteAsync(AlapUrl + url);
            await HibaEllenorzes(valasz);
        }

        // hibás válasznál kivételt dobunk, benne a szerver üzenetével
        private static async Task HibaEllenorzes(HttpResponseMessage valasz)
        {
            if (!valasz.IsSuccessStatusCode)
            {
                string uzenet = await valasz.Content.ReadAsStringAsync();
                throw new Exception(uzenet);
            }
        }
    }
}
