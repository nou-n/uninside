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
            Console.WriteLine(galleryId, galleryType);
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
    }
}