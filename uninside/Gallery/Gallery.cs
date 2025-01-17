using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.Gallery
{
    public class Gallery
    {
        public string Id { get; private set; }
        public GalleryType Type { get; private set; }
        public string Title { get; private set; }
        public string Nickname { get; private set; }

        public Gallery(string galleryId, GalleryType galleryType, string galleryTitle, string galleryNickname)
        {
            Id = galleryId;
            Type = galleryType;
            Title = galleryTitle;
            Nickname = galleryNickname;
        }
    }

    public enum GalleryType
    {
        Normal,
        Minor,
        Mini
    }
}