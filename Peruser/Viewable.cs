using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
namespace Peruser
{
    public class Viewable
    {
        public string Path { get; set; }

        public Viewable(string path)
        {
            Path = path;
        }
    }
}
