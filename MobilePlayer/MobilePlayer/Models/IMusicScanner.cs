using System.Collections.Generic;

namespace MobilePlayer.Models
{
    public interface IMusicScanner
    {
        ICollection<string> Scan(string directory);
    }
}