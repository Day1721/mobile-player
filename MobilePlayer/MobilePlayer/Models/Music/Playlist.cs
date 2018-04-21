using System.Collections.Generic;

namespace MobilePlayer.Models.Music
{
    public class Playlist
    {
        public int Posision { get; set; }
        public IList<string> Songs { get; set; }
    }
}