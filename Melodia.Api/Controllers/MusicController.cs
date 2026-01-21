using Melodia.Api.Model.ViewModel;
using Melodia.Api.Services;
using Melodia.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Melodia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var audio = await _service.GenerateMusicAsync(
                request.Prompt,
                request.Duracao
            );

            if (audio == null)
            {
                return BadRequest("Erro ao criar som");
            }

            var fileName = string.IsNullOrWhiteSpace(request.Nome) ? "music.wav" : $"{request.Nome}.wav";

            return File(audio, "audio/wav", fileName);
        }
    }
}
