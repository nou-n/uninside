using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uninside.API.Comment;

namespace uninside.API.Post
{
    public class PostInfo
    {
        public string GalleryId { get; private set; }
        public GalleryType GalleryType { get; private set; }

        public string PostTitle { get; private set; }
        public string PostNo { get; private set; }

        public string WriterName { get; private set; }
        public string WriterIp { get; private set; }
        public string WriterId { get; private set; }

        public string Spoiler { get; private set; }

        public PostInfo(string galleryId, GalleryType galleryType, string postNo, string postTitle = null, string writerName = null, string writerIp = null, string writerId = null, string spoiler = null)
        {
            GalleryId = galleryId;
            GalleryType = galleryType;
            PostTitle = postTitle;
            PostNo = postNo;
            WriterName = writerName;
            WriterIp = writerIp;
            WriterId = writerId;
            Spoiler = spoiler;
        }
        public PostInfo(string galleryId, GalleryType galleryType, Dictionary<string, object> post)
        {
            GalleryId = galleryId;
            GalleryType = galleryType;
            PostTitle = post.ContainsKey("subject") ? (string)post["subject"] : null;
            PostNo = post.ContainsKey("no") ? ((Int64)post["no"]).ToString() : null;
            WriterIp = post.ContainsKey("ip") ? (string)post["ip"] : null;
            WriterName = post.ContainsKey("name") ? (string)post["name"] : null;
            WriterId = post.ContainsKey("user_id") ? (string)post["user_id"] : null;
            Spoiler = post.ContainsKey("spoiler") ? (string)post["spoiler"] : null;
        }

        async public Task<Post> GetPost(Uninside client)
        {
            return await client.GetPost(this);
        }
        async public Task<CommentList> GetCommentList(Uninside client, int listPage = 1)
        {
            return await client.GetCommentList(this, listPage);
        }
    }
}
