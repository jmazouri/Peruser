using System;
using System.Collections.Generic;

namespace Peruser
{
    public class ImageData
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime LastModified { get; set; }
        public Dictionary<string, string> ExtraData = new Dictionary<string, string>();

        public bool IsLocalFile
        {
            get
            {
                return new Uri(Path).IsFile;
            }
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
