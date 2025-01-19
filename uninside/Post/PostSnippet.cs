using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uninside.Gallery;
using uninside.Util;

namespace uninside.Post
{
    /// <summary>
    /// GetPostList를 호출헀을 때 글 목록의 게시글에 대한 정보를 담는 모델
    /// </summary>
    public class PostSnippet
    {
        public string Title { get; private set; }
        public string Id { get; private set; } // postNo
        public string HeadText { get; private set; }
        public string Date { get; private set; }
        public int TotalViews { get; private set; }
        public string HeadNum { get; private set; }

        public int RecommendCount { get; private set; }

        public int TotalComments { get; private set; }
        public int TotalVoice { get; private set; }

        public bool IsImageIcon {  get; private set; }
        public bool IsMovieIcon { get; private set; }
        public bool IsRecommendIcon { get; private set; }
        public bool IsVoiceIcon {  get; private set; }
        public bool IsWinnertaIcon { get; private set; }

        public string Level { get; private set; }

        public int MemberIcon { get; private set; }
        public string WriterName { get; private set; }
        public string WriterIp { get; private set; }
        public string WriterId { get; private set; }

        public bool BestChk { get; private set; }
        public bool RealtimeChk { get; private set; }
        public bool RealtimeLChk { get; private set; }
        public bool NftChk { get; private set; }


        public PostSnippet(string no, string headNum, string hit, string recommend, string imgIcon, string movieIcon, string recommendIcon, string bestChk, string realtimeChk, string realtimeLChk, string nftChk, string level, string totalComment, string totalVoice, string userId, string voiceIcon, string winnertaIcon, string memberIcon, string ip, string subject, string name, string dateTime, string headtext)
        {
            Title = subject;
            Id = no;
            TotalViews = int.Parse(hit ?? "-1");
            Date = dateTime;
            HeadNum = headNum;
            HeadText = headtext;

            RecommendCount = int.Parse(recommend ?? "-1");

            Level = level;
            MemberIcon = int.Parse(memberIcon ?? "-1");
            WriterName = name;
            WriterIp = ip;
            WriterId = userId;

            TotalComments = int.Parse(totalComment ?? "-1");
            TotalVoice = int.Parse(totalVoice ?? "-1");

            IsImageIcon = Utils.ToBoolean(imgIcon);
            IsMovieIcon = Utils.ToBoolean(movieIcon);
            IsRecommendIcon = Utils.ToBoolean(recommendIcon);
            IsVoiceIcon = Utils.ToBoolean(voiceIcon);
            IsWinnertaIcon = Utils.ToBoolean(winnertaIcon);

            BestChk = Utils.ToBoolean(bestChk);
            RealtimeChk = Utils.ToBoolean(realtimeChk);
            RealtimeLChk = Utils.ToBoolean(realtimeLChk);
            NftChk = Utils.ToBoolean(nftChk);
        }
    }
}