namespace MobilePlayer.Models
{
    public interface IMusicParser
    {
        Song Parse(string path);
    }
}