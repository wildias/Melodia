using Melodia.Api.Services.Interface;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace Melodia.Api.Services
{
    public class MusicService : IMusicService
    {
        private readonly HttpClient _http;


        public MusicService(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "SEU_TOKEN_AQUI");
        }

        public async Task<byte[]> GenerateMusicAsync(string prompt, int duration)
        {
            var payload = new
            {
                inputs = prompt,
                parameters = new
                {
                    max_new_tokens = duration * 1000
                }
            };

            var response = await _http.PostAsJsonAsync(
           "https://api-inference.huggingface.co/models/facebook/musicgen-small",
           payload
            );

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
