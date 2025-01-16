using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uninside.API.Post;
using uninside.Util;

namespace uninside.API.Comment
{
    public class Comment
    {
        public string GalleryId { get; private set; }
        public GalleryType GalleryType { get; private set; }

        public PostInfo CurrentPost { get; private set; }

        public string WriterName { get; private set; }
        public string WriterIp { get; private set; }
        public string WriterId { get; private set; }
        public string WriterIcon { get; private set; } // 1, 2, 3 같은 정수 형태
        public string GallerCon { get; private set; } // https://nstatic.dcinside.com/dc/m/img/bestcon_fix.png 같은 url 형태

        public string CommentContent { get; private set; }
        public string CommentNo { get; private set; }
        public string CommentDate { get; private set; }
        public string DcCon { get; private set; }
        public string DcConDetailIdx { get; private set; }
        public string DcConType { get; private set; }
        public string DcConMp4 { get; private set; }

        public bool isReply { get; private set; }
        public Comment(PostInfo currentPost, Dictionary<string, object> jsonObject)
        {
            GalleryId = currentPost.GalleryId;
            GalleryType = currentPost.GalleryType;

            CurrentPost = currentPost;

            WriterName = (string)jsonObject.GetValue("name");
            WriterIp = (string)jsonObject.GetValue("ipData");
            WriterId = (string)jsonObject.GetValue("user_id");
            WriterIcon = (string)jsonObject.GetValue("member_icon");
            GallerCon = (string)jsonObject.GetValue("gallercon");

            DcCon = (string)jsonObject.GetValue("dccon");
            DcConDetailIdx = (string)jsonObject.GetValue("dccon_detail_idx");
            DcConType = (string)jsonObject.GetValue("dccon_type");
            DcConMp4 = (string)jsonObject.GetValue("dccon_mp4");

            CommentContent = (string)jsonObject.GetValue("comment_memo");
            CommentNo = (string)jsonObject.GetValue("comment_no");
            CommentDate = (string)jsonObject.GetValue("date_time");

            isReply = (jsonObject.ContainsKey("under_step") && (bool)jsonObject["under_step"]);
        }
    }
}
