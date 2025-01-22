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
using uninside.User;

namespace uninside
{
    public class Uninside
    {
        private AuthManager AuthManagerInstance;
        private IUser CurrentUser;

        internal App AppInstance;
        internal Session CurrentSession;

        internal bool isInitialized = false;

        public Uninside(IUser user)
        {
            AuthManagerInstance = new AuthManager();
            CurrentUser = user;
        }

        async public Task Initialize()
        {
            await GetAppId();

            LoginManager loginManager = new LoginManager(this);
            CurrentSession = await loginManager.Login(CurrentUser);

            isInitialized = true;
        }

        internal async Task<string> GetAppId()
        {
            string hashedAppKey = await AuthManagerInstance.GenerateHashedAppKey();

            if (AppInstance != null && hashedAppKey == AppInstance.Token) return AppInstance.Id;

            (string id, string fcm) = await AuthManagerInstance.FetchAppId(hashedAppKey);
            AppInstance = new App(token: hashedAppKey, id: id, fcm: fcm);
            return AppInstance.Id;
        }

        public GalleryManager GetGalleryManager() => new GalleryManager(this);
        public PostManager GetPostManager() => new PostManager(this);
        public CommentManager GetCommentManager() => new CommentManager(this);
    }
}
