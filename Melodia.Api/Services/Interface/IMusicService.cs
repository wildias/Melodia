namespace Melodia.Api.Services.Interface
{
    public interface IMusicService
    {
        Task<byte[]> GenerateMusicAsync(string prompt, int duration);
    }

}
