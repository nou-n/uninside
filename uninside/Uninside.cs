using System;
using System.Threading.Tasks;
using AuthService = uninside.Auth.Auth;

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
    }

    public enum GalleryType
    {
        Normal,
        Minor,
        Mini
    }
}
