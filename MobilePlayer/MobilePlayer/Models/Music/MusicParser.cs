using System.Threading.Tasks;
using Android.Graphics;
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
            var coverBitmap = GetCoverFromRetriever(retriever);
            
            return new Song(title, album, artist, path, albumArtist, coverBitmap);
        }

        public async Task<Song> ParseAsync(File path)
        {
            var retriever = new MediaMetadataRetriever();
            await retriever.SetDataSourceAsync(path.Path);
            
            var title = retriever.ExtractMetadata(MetadataKey.Title);
            var album = retriever.ExtractMetadata(MetadataKey.Album);
            var artist = retriever.ExtractMetadata(MetadataKey.Artist);
            var albumArtist = retriever.ExtractMetadata(MetadataKey.Albumartist);
            var coverBitmap = GetCoverFromRetriever(retriever);
            
            return new Song(title, album, artist, path, albumArtist, coverBitmap);
        }

        private Bitmap GetCoverFromRetriever(MediaMetadataRetriever retriever)
        {
            var bytes = retriever.GetEmbeddedPicture();
            return bytes == null ? null : BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
        }
    }
}