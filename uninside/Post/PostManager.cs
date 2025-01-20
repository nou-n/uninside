using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiny;
using uninside.Gallery;
using uninside.Util;
using uninside.Http;

namespace uninside.Post
{
    public class PostManager
    {
        public Uninside Client { get; private set; }
        
        public PostManager(Uninside uninside)
        {
            this.Client = uninside;
        }

        async public Task<Post> ReadPost(string galleryId, GalleryType galleryType, string postNo)
        {
            HttpResponse articleResponse = await Utils.RedirectRequest(ApiUrls.Article.READ + "?" + HttpRequest.UrlEncode(new Dictionary<string, string>()
                {
                    { "id", Utils.GetGalleryId(galleryId, galleryType) },
                    { "no", postNo },
                    { "app_id", await Client.GetAppId() }
                }
            ));

            Dictionary<string, object> jsonData = Json.Decode<List<Dictionary<string, object>>>(await articleResponse.Message.Content.ReadAsStringAsync())[0];

            if(!(jsonData.ContainsKey("view_info") && jsonData.ContainsKey("view_main"))) throw new Exception(
                "view_info와 view_main 키를 찾을 수 없습니다. (Response: " + Json.Encode(jsonData) + ")"
            );

            return new Post(
                galleryId,
                galleryType,
                (Dictionary<string, object>)jsonData["view_info"],
                (Dictionary<string, object>)jsonData["view_main"]
            );
        }
        async public Task<Post> ReadPost(Gallery.Gallery gallery, string postNo) => await ReadPost(gallery.Id, gallery.Type, postNo);

        async public Task<(GalleryInfo galleryInfo, List<PostSnippet> postList)> GetPostList(string galleryId, GalleryType galleryType, string headId = "", int page = 1)
        {
            Dictionary<string, string> payload = new Dictionary<string, string>()
            {
                { "id", Utils.GetGalleryId(galleryId, galleryType) },
                { "page", page.ToString() },
                { "app_id", await Client.GetAppId() }
            };
            if (!string.IsNullOrEmpty(headId)) payload.Add("headid", headId);

            HttpResponse postListResponse = await Utils.RedirectRequest(ApiUrls.Article.LIST + "?" + HttpRequest.UrlEncode(payload));

            Dictionary<string, object> jsonData = Json.Decode<List<Dictionary<string, object>>>(await postListResponse.Message.Content.ReadAsStringAsync())[0];

            if (!(jsonData.ContainsKey("gall_info") && jsonData.ContainsKey("gall_list"))) throw new Exception(
                "gall_info와 gall_list 키를 찾을 수 없습니다. (Response: " + Json.Encode(jsonData) + ")"
            );

            Dictionary<string, object> gallInfo = (Dictionary<string, object>) ((List<object>)jsonData["gall_info"])[0];
            List<object> gallList = (List<object>)jsonData["gall_list"];

            List<PostSnippet> postList = new List<PostSnippet>();
            foreach(Dictionary<string, object> dict in gallList)
            {
                postList.Add(new PostSnippet(
                    (string)dict.GetValue("no"),
                    (string)dict.GetValue("headnum"),
                    (string)dict.GetValue("hit"),
                    (string)dict.GetValue("recommend"),
                    (string)dict.GetValue("img_icon"),
                    (string)dict.GetValue("movie_icon"),
                    (string)dict.GetValue("recommend_icon"),
                    (string)dict.GetValue("best_chk"),
                    (string)dict.GetValue("realtime_chk"),
                    (string)dict.GetValue("realtime_l_chk"),
                    (string)dict.GetValue("nft_chk"),
                    (string)dict.GetValue("level"),
                    (string)dict.GetValue("total_comment"),
                    (string)dict.GetValue("total_voice"),
                    (string)dict.GetValue("user_id"),
                    (string)dict.GetValue("voice_icon"),
                    (string)dict.GetValue("winnerta_icon"),
                    (string)dict.GetValue("member_icon"),
                    (string)dict.GetValue("ip"),
                    (string)dict.GetValue("subject"),
                    (string)dict.GetValue("name"),
                    (string)dict.GetValue("date_time"),
                    (string)dict.GetValue("headtext")
                ));
            }

            return (
                new GalleryInfo(
                    (string)gallInfo.GetValue("gall_title"),
                    (string)gallInfo.GetValue("category"),
                    (string)gallInfo.GetValue("file_cnt"),
                    (string)gallInfo.GetValue("file_size"),
                    (bool)(gallInfo.GetValue("is_minor") ?? false),
                    (bool)(gallInfo.GetValue("use_ai_write") ?? false),
                    (List<object>)gallInfo.GetValue("head_text"),
                    (List<object>)gallInfo.GetValue("placeholder"),
                    (string)gallInfo.GetValue("notify_recent"),
                    (Dictionary<string, object>)gallInfo.GetValue("must_read"),
                    (string)gallInfo.GetValue("gall_nickname")
                ),
                postList
            );
        }
        async public Task<(GalleryInfo galleryInfo, List<PostSnippet> postList)> GetPostList(Gallery.Gallery gallery, string headId = "", int page = 1) => await GetPostList(gallery.Id, gallery.Type, headId, page);
    }
}