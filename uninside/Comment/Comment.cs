using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uninside.Gallery;
using uninside.Post;
using uninside.Util;

namespace uninside.Comment
{
    public class Comment
    {
        #region 글쓴이
        public string WriterName { get; private set; }
        public string WriterIp { get; private set; }
        public string WriterId { get; private set; }
        public string WriterIcon { get; private set; } // 1, 2, 3 같은 정수 형태
        public string GallerCon { get; private set; } // https://nstatic.dcinside.com/dc/m/img/bestcon_fix.png 같은 url 형태
        #endregion

        #region 댓글
        public string Content { get; private set; }
        public string Id { get; private set; } // commentNo
        public string Date { get; private set; }
        #endregion

        #region dc콘
        public string DcCon { get; private set; }
        public string DcConDetailIdx { get; private set; }
        public string DcConType { get; private set; }
        public string DcConMp4 { get; private set; }
        #endregion

        public bool isReply { get; private set; }

        public Comment(Dictionary<string, object> jsonObject)
        {
            WriterName = (string)jsonObject.GetValue("name");
            WriterIp = (string)jsonObject.GetValue("ipData");
            WriterId = (string)jsonObject.GetValue("user_id");
            WriterIcon = (string)jsonObject.GetValue("member_icon");
            GallerCon = (string)jsonObject.GetValue("gallercon");

            DcCon = (string)jsonObject.GetValue("dccon");
            DcConDetailIdx = (string)jsonObject.GetValue("dccon_detail_idx");
            DcConType = (string)jsonObject.GetValue("dccon_type");
            DcConMp4 = (string)jsonObject.GetValue("dccon_mp4");

            Content = (string)jsonObject.GetValue("comment_memo");
            Id = (string)jsonObject.GetValue("comment_no");
            Date = (string)jsonObject.GetValue("date_time");

            isReply = jsonObject.ContainsKey("under_step") && (bool)jsonObject["under_step"];
        }
    }
}