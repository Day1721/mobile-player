using System.Threading.Tasks;
using Android.Media;
using Java.IO;

namespace MobilePlayer.Models.Music
{
    public class MusicParser : IMusicParser, IAsyncMusicParser
    {
        public Song Parse(File path)
        {
            var retriever = new MediaMetadataRetriever();
            retriever.SetDataSource(path.Path);

            var title = retriever.ExtractMetadata(MetadataKey.Title);
            var album = retriever.ExtractMetadata(MetadataKey.Album);
            var artist = retriever.ExtractMetadata(MetadataKey.Artist);
            var albumArtist = retriever.ExtractMetadata(MetadataKey.Albumartist);
            
            return new Song(title, album, artist, path, albumArtist);
        }

        public async Task<Song> ParseAsync(File path)
        {
            var retriever = new MediaMetadataRetriever();
            await retriever.SetDataSourceAsync(path.Path);
            
            var title = retriever.ExtractMetadata(MetadataKey.Title);
            var album = retriever.ExtractMetadata(MetadataKey.Album);
            var artist = retriever.ExtractMetadata(MetadataKey.Artist);
            var albumArtist = retriever.ExtractMetadata(MetadataKey.Albumartist);
            
            return new Song(title, album, artist, path, albumArtist);
        }
    }
}