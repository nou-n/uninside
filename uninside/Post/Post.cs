using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uninside.Gallery;
using static uninside.Util.Utils;

namespace uninside.Post
{
    public class Post
    {
        public string GalleryId { get; private set; }
        public GalleryType GalleryType { get; private set; }
        public string GalleryTitle { get; private set; }
        public string GalleryNickname { get; private set; }
        public string Category { get; private set; }

        #region 게시글
        public string Title { get; private set; }
        public string Id { get; private set; } // postNo
        public string HeadTitle { get; private set; }
        public string Date { get; private set; }
        public string ContentHTML { get; private set; }
        public int TotalViews { get; private set; }
        #endregion

        #region 댓글
        public int TotalComments { get; private set; }
        public bool CommentCaptchaEnabled { get; private set; }
        public int CommentCaptchaLength { get; private set; }
        #endregion

        public string Level { get; private set; }

        #region 글쓴이
        public string WriterName { get; private set; }
        public string WriterIp { get; private set; }
        public string WriterId { get; private set; }
        public string WriterIcon { get; private set; }
        public bool IsAnonymous { get; private set; }
        #endregion

        #region 추천 & 비추천

        public int RecommendCount { get; private set; }
        public int MemberRecommendCount { get; private set; }

        public int NonRecommendCount { get; private set; }
        public bool NonRecommendEnabled { get; private set; }
        #endregion

        #region 추가 정보
        public bool ImageChk { get; private set; }
        public bool RecommendChk { get; private set; }
        public bool WinnertaChk { get; private set; }
        public bool VoiceChk { get; private set; }
        public bool BestChk { get; private set; }
        public bool RealtimeLChk { get; private set; }
        public bool NftChk { get; private set; }
        public bool NftOnChainChk { get; private set; }

        public bool IsNotice { get; private set; }

        public PostPreview PreviousPost { get; private set; }

        public PostPreview NextPost { get; private set; }
        #endregion

        public Post(string galleryId, GalleryType galleryType, Dictionary<string, object> viewInfo, Dictionary<string, object> viewMain)
        {
            GalleryId = galleryId;
            this.GalleryType = galleryType;
            GalleryTitle = (string)viewInfo.GetValue("galltitle");
            GalleryNickname = (string)viewInfo.GetValue("gall_nickname");

            Category = (string)viewInfo.GetValue("category");

            Title = (string)viewInfo.GetValue("subject");
            Id = (string)viewInfo.GetValue("no");
            HeadTitle = (string)viewInfo.GetValue("headtitle");
            Date = (string)viewInfo.GetValue("date_time");
            ContentHTML = (string)viewMain.GetValue("memo");

            RecommendCount = int.Parse((string)viewMain.GetValue("recommend"));
            MemberRecommendCount = int.Parse((string)viewMain.GetValue("recommend_member"));

            NonRecommendCount = int.Parse((string)viewMain.GetValue("nonrecommend"));
            NonRecommendEnabled = !(bool)viewMain.GetValue("nonrecomm_use");

            TotalViews = int.Parse((string)viewInfo.GetValue("hit"));

            WriterName = (string)viewInfo.GetValue("name");
            Level = (string)viewInfo.GetValue("level");
            WriterIp = (string)viewInfo.GetValue("ip");
            WriterId = (string)viewInfo.GetValue("user_id");
            WriterIcon = (string)viewInfo.GetValue("member_icon");
            IsAnonymous = !string.IsNullOrEmpty((string)viewInfo.GetValue("anonymous"));

            TotalComments = int.Parse((string)viewInfo.GetValue("total_comment"));
            CommentCaptchaEnabled = (bool)(viewInfo.GetValue("comment_captcha") ?? false);
            CommentCaptchaLength = int.Parse((string)(viewInfo.GetValue("comment_code_count") ?? "0"));

            ImageChk = ToBoolean((string)viewInfo.GetValue("img_chk"));
            RecommendChk = ToBoolean((string)viewInfo.GetValue("recommend_chk"));
            WinnertaChk = ToBoolean((string)viewInfo.GetValue("winnerta_chk"));
            VoiceChk = ToBoolean((string)viewInfo.GetValue("voice_chk"));
            BestChk = ToBoolean((string)viewInfo.GetValue("best_chk"));
            RealtimeLChk = ToBoolean((string)viewInfo.GetValue("realtime_l_chk"));
            NftChk = ToBoolean((string)viewInfo.GetValue("nft_chk"));
            NftOnChainChk = ToBoolean((string)viewInfo.GetValue("nft_onchain_chk"));
            IsNotice = ToBoolean((string)viewInfo.GetValue("isNotice"));

            // 현재 게시글과 NextPost, PreviousPost의 갤러리가 다른 경우가 있는지 확인 필요

            if (viewInfo.ContainsKey("prev_post"))
            {
                Dictionary<string, object> previousPost = (Dictionary<string, object>)viewInfo["prev_post"];

                PreviousPost = new PostPreview(galleryId, galleryType,
                    postNo: previousPost.ContainsKey("no") ? ((Int64)previousPost["no"]).ToString() : null,
                    postTitle: previousPost.ContainsKey("subject") ? ((string)previousPost["subject"]) : null,
                    writerName: previousPost.ContainsKey("name") ? ((string)previousPost["name"]) : null,
                    writerIp: previousPost.ContainsKey("ip") ? ((string)previousPost["ip"]) : null,
                    writerId: previousPost.ContainsKey("user_id") ? ((string)previousPost["user_id"]) : null,
                    spoiler: previousPost.ContainsKey("spoiler") ? ((string)previousPost["spoiler"]) : null
                );
            }
            else PreviousPost = null;

            if(viewInfo.ContainsKey("next_post"))
            {
                Dictionary<string, object> nextPost = (Dictionary<string, object>)viewInfo["next_post"];

                NextPost = new PostPreview(galleryId, galleryType,
                    postNo: nextPost.ContainsKey("no") ? ((Int64)nextPost["no"]).ToString() : null,
                    postTitle: nextPost.ContainsKey("subject") ? ((string)nextPost["subject"]) : null,
                    writerName: nextPost.ContainsKey("name") ? ((string)nextPost["name"]) : null,
                    writerIp: nextPost.ContainsKey("ip") ? ((string)nextPost["ip"]) : null,
                    writerId: nextPost.ContainsKey("user_id") ? ((string)nextPost["user_id"]) : null,
                    spoiler: nextPost.ContainsKey("spoiler") ? ((string)nextPost["spoiler"]) : null
                );
            }
            else NextPost = null;
        }
    }
}