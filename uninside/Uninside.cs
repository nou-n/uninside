using System;
using System.Threading.Tasks;
using uninside.Http;
using System.Collections.Generic;
using Tiny;
using uninside.Util;
using uninside.Auth;
using uninside.Post;
using uninside.Comment;
using uninside.Gallery;

namespace uninside
{
    public class Uninside
    {
        private AuthManager authManager;
        private App app;

        public Uninside()
        {
            authManager = new AuthManager();
        }

        async public Task Initialize()
        {
            await GetAppId();
        }

        internal async Task<string> GetAppId()
        {
            string hashedAppKey = await authManager.GenerateHashedAppKey();

            if (app != null && hashedAppKey == app.Token) return app.Id;

            (string id, string fcm) = await authManager.FetchAppId(hashedAppKey);
            app = new App(token: hashedAppKey, id: id, fcm: fcm);
            return app.Id;
        }

        public GalleryManager GetGalleryManager() => new GalleryManager(this);
        public PostManager GetPostManager() => new PostManager(this);
        public CommentManager GetCommentManager() => new CommentManager(this);
    }
}
