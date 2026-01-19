using Melodia.Api.Model.ViewModel;
using Melodia.Api.Services;
using Melodia.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Melodia.Api.Controllers
{
    [ApiController]
    [Route("api/music")]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _service;

        public MusicController(IMusicService service)
        {
            _service = service;
        }

        [HttpPost("gerar")]
        public async Task<IActionResult> Generate(MusicViewModel request)
        {
            var audioBytes = await _service.GenerateMusicAsync(
                request.Prompt,
                request.DurationSeconds
            );

            return File(audioBytes, "audio/wav", $"{request.Nome}.wav");
        }
    }
}
