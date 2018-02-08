using System.Collections.Generic;
using System.Threading.Tasks;
using Java.IO;

namespace MobilePlayer.Models.Music
{
    public interface IMusicScanner
    {
        ICollection<File> Scan(File directory);
    }

    public interface IAsyncMusicScanner
    {
        Task<ICollection<File>> ScanAsync(File directory);
    }
}