using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.Gallery
{
    public class GalleryManager
    {
        public Uninside Client { get; private set; }

        public GalleryManager(Uninside uninside)
        {
            this.Client = uninside;
        }
    }
}