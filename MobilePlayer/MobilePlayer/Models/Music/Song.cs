using System;
using Java.IO;

namespace MobilePlayer.Models.Music
{
    public class Song : ICloneable<Song>
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

        public Song Clone() => new Song(Title, Album, Artist, Path, AlbumArtist);
    }
}