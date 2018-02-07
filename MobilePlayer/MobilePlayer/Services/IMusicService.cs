namespace MobilePlayer.Services
{
    public interface IMusicService
    {
        void Play(string path);

        void Play();

        void Pause();
    }
}