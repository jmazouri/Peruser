using System;

namespace Peruser
{
    public class ImageData
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime LastModified { get; set; }

        public override string ToString()
        {
            return Path;
        }
    }
}
