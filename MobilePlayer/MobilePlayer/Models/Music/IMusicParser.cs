using System.Threading.Tasks;
using Java.IO;

namespace MobilePlayer.Models.Music
{
    public interface IMusicParser
    {
        Song Parse(File path);
    }

    public interface IAsyncMusicParser
    {
        Task<Song> ParseAsync(File path);
    }
}