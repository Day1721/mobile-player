using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MobilePlayer.Models.Extentions;
using File = Java.IO.File;

namespace MobilePlayer.Models.Music
{
    public class MusicScanner : IMusicScanner, IAsyncMusicScanner
    {
        private static readonly Regex[] Patterns = new[]
        {
            new Regex(@"\.mp3$") 
        };

        public async Task<ICollection<File>> ScanAsync(File directory)
        {
            if (!directory.IsDirectory)
            {
                throw new ArgumentException("directory param is not a directory", nameof(directory));
            }

            var fileClasses = await directory.ListFilesAsync();
            var (directories, files) = fileClasses.Split(file => file.IsDirectory);
            directories = directories.ToList();
            
            files = files.Where(file => Patterns.Any(pattern => pattern.IsMatch(file.Name)));
            var subResults = directories.SelectMany(Scan);

            return files.Concat(subResults).ToList();
        }
        
        public ICollection<File> Scan(File directory)
        {
            if (!directory.IsDirectory)
            {
                throw new ArgumentException("directory param is not a directory", nameof(directory));
            }

            var fileClasses = directory.ListFiles();
            var (directories, files) = fileClasses.Split(file => file.IsDirectory);
            directories = directories.ToList();
            
            files = files.Where(file => Patterns.Any(pattern => pattern.IsMatch(file.Name)));
            var subResults = directories.SelectMany(Scan);

            return files.Concat(subResults).ToList();
        }
    }
}