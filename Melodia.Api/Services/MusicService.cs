using Melodia.Api.Services.Interface;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Melodia.Api.Services
{
    public class MusicService : IMusicService
    {
        private readonly HttpClient _http;
        private readonly string _token;

        public MusicService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _token = config["Replicate:Token"]!;

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<byte[]> GenerateMusicAsync(string prompt, int duration)
        {
            try
            {
                var payload = new
                {
                    input = new
                    {
                        prompt = prompt
                    }
                };

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "v1/predictions"
                );

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                request.Content = JsonContent.Create(payload);

                var response = await _http.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var id = doc.RootElement.GetProperty("id").GetString();

                // Polling
                while (true)
                {
                    await Task.Delay(2000);

                    var statusResponse = await _http.GetAsync($"v1/predictions/{id}");
                    statusResponse.EnsureSuccessStatusCode();

                    var statusJson = await statusResponse.Content.ReadAsStringAsync();
                    using var statusDoc = JsonDocument.Parse(statusJson);

                    var status = statusDoc.RootElement.GetProperty("status").GetString();

                    if (status == "succeeded")
                    {
                        var output = statusDoc.RootElement.GetProperty("output");

                        var audioUrl =
                            output.ValueKind == JsonValueKind.Array
                                ? output[0].GetString()
                                : output.GetString();

                        return await _http.GetByteArrayAsync(audioUrl);
                    }

                    if (status == "failed")
                    {
                        throw new Exception("Falha ao gerar música no Replicate.");
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
