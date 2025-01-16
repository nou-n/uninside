using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.Util
{
    internal static class ApiUrls
    {

        public const string PC_WEB = Protocol.HTTPS + "gall.dcinside.com/";
        public const string MOBILE_WEB = Protocol.HTTP + "m.dcinside.com/";
        public const string MOBILE_WEB_HTTPS = Protocol.HTTPS + "m.dcinside.com/";
        public const string MOBILE_APP = Protocol.HTTPS + "app.dcinside.com/";
        public const string APP_API = MOBILE_APP + "api/";
        public const string AUTH_API = Protocol.HTTPS + "msign.dcinside.com/";
        public const string MAIN_API = Protocol.HTTP + "json2.dcinside.com/";
        public const string UPLOAD = Protocol.HTTPS + "upload.dcinside.com/";
        public const string MOVIE_UPLOAD = Protocol.HTTPS + "m4up4.dcinside.com/";
        public const string REDIRECT = APP_API + "redirect.php";

        private static class Protocol
        {
            public const string HTTP = "http://";
            public const string HTTPS = "https://";
        }

        public static class Firebase
        {
            public const string INSTALLATIONS =
                Protocol.HTTPS + "firebaseinstallations.googleapis.com/v1/projects/dcinside-b3f40/installations";
            public const string REMOTE_CONFIG =
                Protocol.HTTPS + "firebaseremoteconfig.googleapis.com/v1/projects/" + Values.Register3.SENDER + "/namespaces/firebase:fetch";
        }

        public static class PlayService
        {
            public const string ANDROID_CLIENT = Protocol.HTTPS + "android.clients.google.com";
            public const string ANDROID_APIS = Protocol.HTTPS + "android.apis.google.com";
            public const string REGISTER3 = ANDROID_APIS + "/c2dm/register3";
            public const string CHECKIN = ANDROID_CLIENT + "/checkin";
        }

        public static class Upload
        {
            public const string CHECK_UPLOAD_RESTRICTION = APP_API + "chk_upload_restriction";
            public const string MOVIE = MOVIE_UPLOAD + "/movie_upload_v1.php";
        }

        public static class Article
        {
            public const string LIST = APP_API + "gall_list_new.php";
            public const string READ = APP_API + "gall_view_new.php";
            public const string WRITE = UPLOAD + "_app_write_api.php";
            public const string DELETE = APP_API + "gall_del.php";
            public const string MODIFY = APP_API + "gall_modify.php";
            public const string UPVOTE = APP_API + "_recommend_up.php";
            public const string DOWNVOTE = APP_API + "_recommend_down.php";
            public const string REPORT = MOBILE_WEB + "api/report.php";
            public const string HIT_UPVOTE = APP_API + "hit_recommend";
            public const string INSERT_MOVIE_INFO = MOBILE_APP + "movie/insert-mvinfo";
        }

        public static class Comment
        {
            public const string OK = APP_API + "comment_ok.php";
            public const string DELETE = APP_API + "comment_del.php";
            public const string READ = APP_API + "comment_new.php";
        }

        public static class DCCon
        {
            public const string DCCON = APP_API + "dccon.php";
        }

        public static class Gallery
        {
            public const string MINOR_INFO = APP_API + "minor_info";
            public const string MINOR_MANAGEMENT = PC_WEB + "mgallery/management/mobile";
            public const string MINOR_NOMEMBER = MOBILE_WEB_HTTPS + "management/minor/nomember";
            public const string MINOR_MANAGER_REQUEST = APP_API + "_manager_request.php";
            public const string MINOR_BLOCK_WEB = APP_API + "minor_avoid";
            public const string MINOR_BLOCK_ADD = APP_API + "minor_avoidadd";
        }

        public static class Search
        {
            public const string SEARCH = APP_API + "_total_search.php";
        }

        public static class Auth
        {
            public const string LOGIN = AUTH_API + "api/login";
            public const string APP_ID = AUTH_API + "auth/mobile_app_verification";
            public const string APP_CHECK = MAIN_API + "json0/app_check_A_rina_one_new.php";
        }

        public static class User
        {
            public const string MY_GALL = APP_API + "mygall.php";
            public const string MY_GALL_MODIFY = APP_API + "mygall_modify.php";
            public const string MY_MANAGE_GALL_CHECK = APP_API + "mymanageGallChk";
            public const string MY_MINI_JOIN_CHECK = APP_API + "myminijoinGallChk";
        }

        public static class MiniGallery
        {
            public const string JOIN = APP_API + "memberjoin";
            public const string JOIN_OK = APP_API + "memberjoin_ok";
            public const string QUIT = APP_API + "memberout_ok";
        }

        public static class MainInfo
        {
            public const string NOTICE = MAIN_API + "json3/app_dc_notice_one_new.php";
            public const string APP_MAIN = MAIN_API + "json3/main_content.php";
            public const string GALLERY_RANKING = MAIN_API + "json3/ranking_gallery.php";
            public const string MINOR_GALLERY_RANKING = MAIN_API + "json1/mgallmain/mgallery_ranking.php";
            public const string MINI_GALLERY_RANKING = MAIN_API + "json1/migallmain/migallery_ranking.php";
        }
    }
}
