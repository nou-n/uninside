using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiny;
using uninside.API.Comment;
using uninside.Util;

namespace uninside.API.Post
{
    public class Post
    {
        public string GalleryId { get; private set; }
        public GalleryType GalleryType { get; private set; }
        public string GalleryTitle { get; private set; }
        public string GalleryNickname { get; private set; }
        public string Category { get; private set; }

        public string PostTitle { get; private set; }
        public string PostNo { get; private set; }
        public string PostHeadTitle { get; private set; }
        public string PostDate { get; private set; }
        public string PostContent { get; private set; }

        public int RecommendCount {  get; private set; }
        public int MemberRecommendCount { get; private set; }

        public int NonRecommendCount { get; private set; }
        public bool NonRecommendEnabled { get; private set; }

        public int TotalViews { get; private set; }

        public string WriterName { get; private set; }
        public string WriterLevel { get; private set; }
        public string WriterIp { get; private set; }
        public string WriterId { get; private set; }
        public string WriterIcon { get; private set; }
        public bool IsAnonymous { get; private set; }

        public int TotalComments { get; private set; }
        public bool CommentCaptchaEnabled { get; private set; }
        public int CommentCaptchaLength { get; private set; }

        public bool ImageChk { get; private set; }
        public bool RecommendChk { get; private set; }
        public bool WinnertaChk { get; private set; }
        public bool VoiceChk { get; private set; }
        public bool BestChk { get; private set; }
        public bool RealtimeLChk { get; private set; }
        public bool NftChk { get; private set; }
        public bool NftOnChainChk { get; private set; }
        public bool IsNotice { get; private set; }

        public PostInfo PrevPost { get; private set; }

        public PostInfo NextPost { get; private set; }

        public Post(string galleryid, GalleryType galleryType, Dictionary<string, object> viewInfo, Dictionary<string, object> viewMain)
        {
            //Console.WriteLine(Json.Encode((viewInfo)));
            //Console.WriteLine(Json.Encode(viewMain));

            GalleryId = galleryid;
            GalleryType = galleryType;
            GalleryTitle = (string)viewInfo.GetValue("galltitle");
            GalleryNickname = (string)viewInfo.GetValue("gall_nickname");
            Category = (string)viewInfo.GetValue("category");

            PostTitle = (string)viewInfo.GetValue("subject");
            PostNo = (string)viewInfo.GetValue("no");
            PostHeadTitle = (string)viewInfo.GetValue("headtitle");
            PostDate = (string)viewInfo.GetValue("date_time");
            PostContent = (string) viewMain.GetValue("memo");

            RecommendCount = int.Parse((string)viewMain.GetValue("recommend"));
            MemberRecommendCount = int.Parse((string)viewMain.GetValue("recommend_member"));

            NonRecommendCount = int.Parse((string)viewMain.GetValue("nonrecommend"));
            NonRecommendEnabled = !(bool)(viewMain.GetValue("nonrecomm_use"));

            TotalViews = int.Parse((string)viewInfo.GetValue("hit"));

            WriterName = (string)viewInfo.GetValue("name");
            WriterLevel = (string)viewInfo.GetValue("level");
            WriterIp = (string)viewInfo.GetValue("ip");
            WriterId = (string)viewInfo.GetValue("user_id");
            WriterIcon = (string)viewInfo.GetValue("member_icon");
            IsAnonymous = !string.IsNullOrEmpty((string)viewInfo.GetValue("anonymous"));

            TotalComments = int.Parse((string)viewInfo.GetValue("total_comment"));
            CommentCaptchaEnabled = (bool) (viewInfo.GetValue("comment_captcha") ?? false);
            CommentCaptchaLength = int.Parse((string) (viewInfo.GetValue("comment_code_count") ?? "0"));

            ImageChk = ToBoolean((string)viewInfo.GetValue("img_chk"));
            RecommendChk = ToBoolean((string)viewInfo.GetValue("recommend_chk"));
            WinnertaChk = ToBoolean((string)viewInfo.GetValue("winnerta_chk"));
            VoiceChk = ToBoolean((string)viewInfo.GetValue("voice_chk"));
            BestChk = ToBoolean((string)viewInfo.GetValue("best_chk"));
            RealtimeLChk = ToBoolean((string)viewInfo.GetValue("realtime_l_chk"));
            NftChk = ToBoolean((string)viewInfo.GetValue("nft_chk"));
            NftOnChainChk = ToBoolean((string)viewInfo.GetValue("nft_onchain_chk"));
            IsNotice = ToBoolean((string)viewInfo.GetValue("isNotice"));

            PrevPost = viewInfo.ContainsKey("prev_post") ? new PostInfo(galleryid, galleryType, (Dictionary<string, object>)viewInfo["prev_post"]) : null;
            NextPost = viewInfo.ContainsKey("next_post") ? new PostInfo(galleryid, galleryType, (Dictionary<string, object>)viewInfo["next_post"]) : null;
        }

        private bool ToBoolean(string yn)
        {
            if (string.IsNullOrEmpty(yn))
                return false;
            return yn.Trim().ToUpper() == "Y";
        }

        async public Task<CommentList> GetCommentList(Uninside client, int listPage = 1)
        {
            return await client.GetCommentList(new PostInfo(GalleryId, GalleryType, PostNo, PostTitle, WriterName, WriterIp, WriterId), listPage);
        }
    }
}
