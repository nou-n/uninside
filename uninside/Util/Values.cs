using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.Util
{
    internal static class Values
    {
        public const string DC_APP_SIGNATURE = "5rJxRKJ2YLHgBgj6RdMZBl2X0KcftUuMoXVug0bsKd0=";
        public const string DC_APP_PACKAGE = "com.dcinside.app.android";
        public const string DC_APP_VERSION_CODE = "100100";
        public const string DC_APP_VERSION_NAME = "5.0.2";
        public const string DC_APP_TARGET_VERSION = "33";
        public const string USER_AGENT = "dcinside.app";

        public static class Firebase
        {
            public const string APP_ID = "1:477369754343:android:d2ffdd960120a207727842";
            public const string AUTH_VERSION = "FIS_v2";
            public const string FIREBASE_CLIENT = "H4sIAAAAAAAAAKtWykhNLCpJSk0sKVayio7VUSpLLSrOzM9TslIyUqoFAFyivEQfAAAA";
            public const string SDK_VERSION = "a:17.1.0";
            public const string REMOTE_CONFIG_SDK_VERSION = "21.2.1";
            public const string CERT = "43bd70dfc365ec1749f0424d28174da44ee7659d";
            public const string OS_VERSION = "25";
            public const string CLIV = "fcm-23.1.1";
            public const string INFO = "Q2U3ar09NyAToOhBO1boBVw1nzmBjxg";
            public const string TARGET_VER = DC_APP_TARGET_VERSION;
            public const string GCM_VERSION = "232512022";
        }

        public static class Installations
        {
            public const string X_ANDROID_PACKAGE = DC_APP_PACKAGE;
            public const string X_ANDROID_CERT = Firebase.CERT;
            public const string X_GOOG_API_KEY = "AIzaSyDcbVof_4Bi2GwJ1H8NjSwSTaMPPZeCE38";
        }

        public static class Register3
        {
            public const string SENDER = "477369754343";
            public const string X_SCOPE_ALL = "*";
            public const string X_SCOPE_REFRESH_REMOTE_CONFIG = "/topics/DcRefreshRemoteConfig";
            public const string X_SCOPE_SHOW_NOTICE_MESSAGE = "/topics/DcShowNoticeMessage";
            public const string X_FIREBASE_APP_NAME_HASH = "R1dAH9Ui7M-ynoznwBdw01tLxhI";
            public const string USER_AGENT = "Android-GCM/1.5 (generic_x86 KK)";
            public const string APP = DC_APP_PACKAGE;
            public const string GCM_VERSION = Firebase.GCM_VERSION;
            public const string CERT = Firebase.CERT;
        }
    }
}
