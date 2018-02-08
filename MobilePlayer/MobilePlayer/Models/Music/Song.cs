using Java.IO;

namespace MobilePlayer.Models.Music
{
    public class Song
    {                                                                                                                                         
        public string Title { get; }
        public string Album { get; }
        public string Artist { get; }
        public string AlbumArtist { get; }
        public File Path { get; }

        public Song(string title, string album, string artist, File path, string albumArtist = null)
        {
            Title = title;
            Album = album;
            Artist = artist;
            Path = path;
            
            AlbumArtist = albumArtist;
        }

        public override string ToString()
        {
            return $"{Title} - {Album} - {Artist}";
        }
    }
}