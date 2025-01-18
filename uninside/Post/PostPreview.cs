using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uninside.Gallery;

namespace uninside.Post
{
    /// <summary>
    /// ReadPost를 호출했을 때 이전 게시글과 다음 게시글에 대한 정보를 담는 모델
    /// </summary>
    public class PostPreview
    {
        public string GalleryId { get; private set; }
        public GalleryType GalleryType { get; private set; }

        public string Title { get; private set; }
        public string Id { get; private set; } // postNo

        public string WriterName { get; private set; }
        public string WriterIp { get; private set; }
        public string WriterId { get; private set; }

        public string Spoiler { get; private set; }

        public PostPreview(string galleryId, GalleryType galleryType, string postNo, string postTitle, string writerName, string writerIp, string writerId, string spoiler)
        {
            GalleryId = galleryId;
            GalleryType = galleryType;

            Title = postTitle;
            Id = postNo;

            WriterName = writerName;
            WriterIp = writerIp;
            WriterId = writerId;
            Spoiler = spoiler;
        }
    }
}
