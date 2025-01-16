using System;
using System.Threading.Tasks;
using AuthService = uninside.Auth.Auth;
using uninside.Http;
using System.Collections.Generic;
using Tiny;
using uninside.Util;
using uninside.API.Comment;
using uninside.API.Post;

namespace uninside
{
    public class Uninside
    {
        private AuthService authService;
        private App app;

        public Uninside()
        {
            authService = new AuthService();
        }

        async public Task Initialize()
        {
            await GetAppId();
        }

        private async Task<string> GetAppId()
        {
            string hashedAppKey = await authService.GenerateHashedAppKey();

            if (app != null && hashedAppKey == app.Token)
            {
                return app.Id;
            }
            else
            {
                (string id, string fcm) = await authService.FetchAppId(hashedAppKey);
                app = new App(token: hashedAppKey, id: id, fcm: fcm);
                return app.Id;
            }
        }

        #region GetPost
        async public Task<Post> GetPost(string galleryId, GalleryType galleryType, string postNo)
        {
            HttpResponse response = await Utils.RedirectRequest(ApiUrls.Article.READ + "?" + HttpRequest.UrlEncode(new Dictionary<string, string>
                {
                    { "id", Utils.GetGalleryId(galleryId, galleryType) },
                    { "no", postNo },
                    { "app_id", await GetAppId() }
                }
            ));
            Dictionary<string, object> jsonObject = Json.Decode<List<Dictionary<string, object>>>(await response.Message.Content.ReadAsStringAsync())[0];
            return new Post(galleryId, galleryType, (Dictionary<string, object>)jsonObject["view_info"], (Dictionary<string, object>)jsonObject["view_main"]);
        }

        async public Task<Post> GetPost(PostInfo postInfo)
        {
            return await GetPost(postInfo.GalleryId, postInfo.GalleryType, postInfo.PostNo);
        }
        #endregion

        #region GetCommentList
        async public Task<CommentList> GetCommentList(string galleryId, GalleryType galleryType, string postNo, int listPage = 1)
        {
            HttpResponse response = await Utils.RedirectRequest(ApiUrls.Comment.READ + "?" + HttpRequest.UrlEncode(new Dictionary<string, string>
                {
                    { "id", Utils.GetGalleryId(galleryId, galleryType) },
                    { "no", postNo },
                    { "re_page", listPage.ToString() },
                    { "app_id", await GetAppId() }
                }
            ));
            Dictionary<string, object> jsonObject = Json.Decode<List<Dictionary<string, object>>>(await response.Message.Content.ReadAsStringAsync())[0];
            return new CommentList(new PostInfo(galleryId, galleryType, postNo), jsonObject);
        }

        async public Task<CommentList> GetCommentList(PostInfo postInfo, int listPage = 1)
        {
            HttpResponse response = await Utils.RedirectRequest(ApiUrls.Comment.READ + "?" + HttpRequest.UrlEncode(new Dictionary<string, string>
                {
                    { "id", Utils.GetGalleryId(postInfo.GalleryId, postInfo.GalleryType) },
                    { "no", postInfo.PostNo },
                    { "re_page", listPage.ToString() },
                    { "app_id", await GetAppId() }
                }
            ));
            Dictionary<string, object> jsonObject = Json.Decode<List<Dictionary<string, object>>>(await response.Message.Content.ReadAsStringAsync())[0];
            return new CommentList(postInfo, jsonObject);
        }
        #endregion
    }

    public enum GalleryType
    {
        Normal,
        Minor,
        Mini
    }
}
