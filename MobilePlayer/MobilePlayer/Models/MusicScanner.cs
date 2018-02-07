using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MobilePlayer.Models
{
    public class MusicScanner : IMusicScanner
    {
        private readonly string[] _patterns = new[]
        {
            "*.mp3"
        };
        
        public ICollection<string> Scan(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new ArgumentException("directory param is not a directory", nameof(directory));
            }

            var filesEnum = _patterns.SelectMany(pattern => Directory.GetFiles(directory, pattern)).Distinct();
            var subResults = Directory.GetDirectories(directory).SelectMany(Scan);

            return filesEnum.Concat(subResults).ToList();
        }
    }
}