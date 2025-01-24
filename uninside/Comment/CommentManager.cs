using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiny;
using uninside.Gallery;
using uninside.Http;
using uninside.Util;
using uninside.Post;

namespace uninside.Comment
{
    public class CommentManager
    {
        public Uninside Client { get; private set; }

        public CommentManager(Uninside uninside)
        {
            if (!uninside.IsInitialized) throw new Exception("Initialize 메서드를 먼저 호출해야 합니다.");
            this.Client = uninside;
        }

        async public Task<List<Comment>> GetCommentList(string galleryId, GalleryType galleryType, string postNo, int listPage = 1)
        {
            HttpResponse commentResponse = await Utils.RedirectRequest(ApiUrls.Comment.READ + "?" + HttpRequest.UrlEncode(new Dictionary<string, string>()
                {
                    { "id", Utils.GetGalleryId(galleryId, galleryType) },
                    { "no", postNo },
                    { "re_page", listPage.ToString() },
                    { "app_id", await Client.GetAppId() }
                }
            ));

            Dictionary<string, object> jsonData = Json.Decode<List<Dictionary<string, object>>>(await commentResponse.Message.Content.ReadAsStringAsync())[0];

            List<Comment> comments = new List<Comment>();
            if (jsonData.ContainsKey("comment_list"))
            {
                List<object> commentList = (List<object>)jsonData["comment_list"];
                foreach (Dictionary<string, object> comment in commentList)
                {
                    comments.Add(new Comment(comment));
                }
            }
            return comments;
        }
        async public Task<List<Comment>> GetCommentList(Gallery.Gallery gallery, string postNo, int listPage = 1) => await GetCommentList(gallery.Id, gallery.Type, postNo, listPage);
        async public Task<List<Comment>> GetCommentList(Post.Post post, int listPage = 1) => await GetCommentList(post.GalleryId, post.GalleryType, post.Id, listPage);
    }
}