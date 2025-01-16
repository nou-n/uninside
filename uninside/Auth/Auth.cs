using Google.Protobuf;
using Google.Protobuf.checkin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiny;
using uninside.Http;
using uninside.Utils;
using static Google.Protobuf.checkin.CheckinRequest.Types.Checkin.Types;
using static Google.Protobuf.checkin.CheckinRequest.Types;
using static uninside.Http.Http;
using static uninside.Utils.Utils;
using System.Security.Cryptography;
using System.Net.Http;
using System.IO;

namespace uninside.Auth
{

    internal class Auth
    {
        private TimeZoneInfo seoulTimeZone;
        private DateTime? lastRefreshTime = null;
        private string time = null;

        private string fcmToken = "";
        private string fid = RandomFidGenerator.CreateRandomFid();
        private string refreshToken = "";

        internal Auth()
        {
            string timeZoneId = Environment.OSVersion.Platform == PlatformID.Win32NT ? "Korea Standard Time" : "Asia/Seoul";
            seoulTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        //

        public async Task<(string app_id, string fcmToken)> FetchAppId(string hashedAppKey)
        {
            fcmToken = await FetchFcmToken(fid, refreshToken);

            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(new StringContent(hashedAppKey), "value_token");
            formData.Add(new StringContent(Values.DC_APP_SIGNATURE), "signature");
            formData.Add(new StringContent(Values.DC_APP_PACKAGE), "pkg");
            formData.Add(new StringContent(Values.DC_APP_VERSION_CODE), "vCode");
            formData.Add(new StringContent(Values.DC_APP_VERSION_NAME), "vName");
            formData.Add(new StringContent(fcmToken), "client_token");

            HttpResponse appVerification = await PostAsync(ApiUrls.Auth.APP_ID, payload: formData, headers: GetHeaders());
            Dictionary<string, object> verficationResponse = Json.Decode<Dictionary<string, object>>(await appVerification.Message.Content.ReadAsStringAsync());

            if (!(bool) verficationResponse["result"]) throw new Exception("app_id를 가져올 수 없습니다.");

            return ((string) verficationResponse["app_id"], fcmToken);
        }

        public async Task<string> GenerateHashedAppKey()
        {
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, seoulTimeZone);

            if (time == null || lastRefreshTime == null || NeedsRefresh(lastRefreshTime, now))
            {
                try
                {
                    AppCheck appCheck = await GetAppCheck();
                    if (appCheck.Date != null)
                    {
                        lastRefreshTime = now;
                        time = appCheck.Date;
                        return GenerateHexString($"dcArdchk_{time}");
                    }
                }
                catch
                {

                }
            }
            else
            {
                return GenerateHexString($"dcArdchk_{time}");
            }

            lastRefreshTime = now;
            time = DateToString(now);

            return GenerateHexString($"dcArdchk_{time}");
        }

        //

        private async Task<CheckinResponse> FetchAndroidCheckin()
        {
            CheckinRequest checkinReq = new CheckinRequest
            {
                AndroidId = 0,
                Checkin = new Checkin
                {
                    Build = new Build
                    {
                        Fingerprint = "google/razor/flo:7.1.1/NMF26Q/1602158:user/release-keys",
                        Hardware = "flo",
                        Brand = "google",
                        Radio = "FLO-04.04",
                        ClientId = "android-google",
                        SdkVersion = int.Parse(Values.Firebase.OS_VERSION),
                        PackageVersionCode = int.Parse(Values.DC_APP_VERSION_CODE)
                    },
                    LastCheckinMs = 0,
                    Roaming = "WIFI::"
                },
                Locale = CultureInfo.CurrentCulture.Name,
                LoggingId = GenerateRandomLong(new Random(unchecked((int)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()))),
                MacAddress = { GenerateRandomString(12, "abcdef0123456789") },
                Meid = GenerateRandomString(14, "0123456789"),
                TimeZone = TimeZoneInfo.Local.Id,
                Version = 3,
                OtaCert = { "71Q6Rn2DDZl1zPDVaaeEHItd" },
                MacAddressType = { "wifi" },
                Fragment = 0,
                UserSerialNumber = 0
            };

            HttpResponse response = await PostAsync(ApiUrls.PlayService.CHECKIN, payload: checkinReq.ToByteArray(), headers: new Dictionary<string, string>
            {
                { "Content-Type", "application/x-protobuf" }
            });

            Stream responseStream = await response.Message.Content.ReadAsStreamAsync();
            return CheckinResponse.Parser.ParseFrom(responseStream);
        }

        private async Task<string> FetchFcmToken(string argFid, string argRefreshToken)
        {
            Dictionary<string, string> installationsPayload = new Dictionary<string, string>();

            if (argFid != null) installationsPayload["fid"] = argFid;
            if (argRefreshToken != null) installationsPayload["refreshToken"] = argRefreshToken;

            installationsPayload["appId"] = Values.Firebase.APP_ID;
            installationsPayload["authVersion"] = Values.Firebase.AUTH_VERSION;
            installationsPayload["sdkVersion"] = Values.Firebase.SDK_VERSION;

            HttpResponse firebaseInstallations = await PostAsync(ApiUrls.Firebase.INSTALLATIONS, payload: Json.Encode(installationsPayload), headers: new Dictionary<string, string>
            {
                { "X-Android-Package", Values.Installations.X_ANDROID_PACKAGE },
                { "X-Android-Cert", Values.Installations.X_ANDROID_CERT },
                { "X-firebase-client", Values.Firebase.FIREBASE_CLIENT },
                { "x-goog-api-key", Values.Installations.X_GOOG_API_KEY },
                { "Content-Type", "application/json" }
            });

            Dictionary<string, object> installationsResponse = Json.Decode<Dictionary<string, object>>(await firebaseInstallations.Message.Content.ReadAsStringAsync());
            fid = (string)installationsResponse["fid"];
            refreshToken = (string)installationsResponse["refreshToken"];

            Dictionary<string, object> authToken = (Dictionary<string, object>)installationsResponse["authToken"];
            string token = (string)authToken["token"];

            CheckinResponse androidCheckin = await FetchAndroidCheckin();

            string register3Payload = UrlEncode(new Dictionary<string, string>
            {
                { "X-subtype", Values.Register3.SENDER },
                { "sender", Values.Register3.SENDER },
                { "X-app_ver", Values.DC_APP_VERSION_CODE },
                { "X-osv", Values.Firebase.OS_VERSION },
                { "X-cliv", Values.Firebase.CLIV },
                { "X-gmsv", Values.Firebase.GCM_VERSION },
                { "X-appid", fid },
                { "X-scope", Values.Register3.X_SCOPE_ALL },
                { "X-Goog-Firebase-Installations-Auth", token },
                { "X-gmp_app_id", Values.Firebase.APP_ID },
                { "X-firebase-app-name-hash", Values.Register3.X_FIREBASE_APP_NAME_HASH },
                { "X-app_ver_name", Values.DC_APP_VERSION_NAME },
                { "app", Values.Register3.APP },
                { "device", androidCheckin.AndroidId.ToString() },
                { "app_ver", Values.DC_APP_VERSION_CODE },
                { "info", Values.Firebase.INFO },
                { "plat", "0" },
                { "gcm_ver", Values.Register3.GCM_VERSION },
                { "cert", Values.Register3.CERT },
                { "target_ver", Values.Firebase.TARGET_VER }
            });
            HttpResponse register3Response = await PostAsync(ApiUrls.PlayService.REGISTER3, payload: register3Payload, headers: new Dictionary<string, string>
            {
                { "Authorization", $"AidLogin {androidCheckin.AndroidId}:{androidCheckin.SecurityToken}" },
                { "app", Values.Register3.APP },
                { "gcm_ver", Values.Register3.GCM_VERSION },
                { "app_ver", Values.DC_APP_VERSION_CODE },
                { "User-Agent", Values.Register3.USER_AGENT },
                { "Content-Type", "application/x-www-form-urlencoded" }
            });
            string clientToken = (await register3Response.Message.Content.ReadAsStringAsync()).Split('=')[1];

            await RequestToGcmWithScope(androidCheckin, clientToken, token, Values.Register3.X_SCOPE_REFRESH_REMOTE_CONFIG);
            await RequestToGcmWithScope(androidCheckin, clientToken, token, Values.Register3.X_SCOPE_SHOW_NOTICE_MESSAGE);

            Dictionary<string, object> remoteConfigPayload = new Dictionary<string, object>
            {
                { "platformVersion", Values.Firebase.OS_VERSION },
                { "appInstanceId", fid },
                { "packageName", Values.DC_APP_PACKAGE },
                { "appVersion", Values.DC_APP_VERSION_NAME },
                { "countryCode", CultureInfo.CurrentCulture.Name },
                { "sdkVersion", Values.Firebase.REMOTE_CONFIG_SDK_VERSION },
                { "appBuild", Values.DC_APP_VERSION_CODE },
                { "firstOpenTime", DateTime.Now.Date.AddHours(12).ToString("o") },
                { "analyticsUserProperties", new Dictionary<string, object>
                    {
                        { "store_name", "ONE" }
                    }
                },
                { "appId", Values.Firebase.APP_ID },
                { "languageCode", CultureInfo.CurrentCulture.IetfLanguageTag },
                { "appInstanceIdToken", token },
                { "timeZone", TimeZoneInfo.Local.Id }
            };
            await PostAsync(ApiUrls.Firebase.REMOTE_CONFIG, payload: Json.Encode(remoteConfigPayload), new Dictionary<string, string>
            {
                { "X-Goog-Api-Key", Values.Installations.X_GOOG_API_KEY },
                { "X-Android-Package", Values.Installations.X_ANDROID_PACKAGE },
                { "X-Android-Cert", Values.Installations.X_ANDROID_CERT },
                { "X-Google-GFE-Can-Retry", "yes" },
                { "X-Goog-Firebase-Installations-Auth", token },
                { "Content-Type", "application/json" }
            });
            return clientToken;
        }

        private async Task RequestToGcmWithScope(CheckinResponse androidCheckin, string clientToken, string installationToken, string scope)
        {
            await PostAsync(ApiUrls.PlayService.REGISTER3, headers: new Dictionary<string, string>
            {
                { "Authorization", $"AidLogin {androidCheckin.AndroidId}:{androidCheckin.SecurityToken}" },
                { "app", Values.Register3.APP },
                { "gcm_ver", Values.Register3.GCM_VERSION },
                { "app_ver", Values.DC_APP_VERSION_CODE },
                { "User-Agent", Values.Register3.USER_AGENT },
                { "Content-Type", "application/x-www-form-urlencoded" }
            }, payload: UrlEncode(new Dictionary<string, string>
            {
                { "X-subtype", clientToken },
                { "sender", clientToken },
                { "X-gcm.topic", scope },
                { "X-app_ver", Values.DC_APP_VERSION_CODE.ToString() },
                { "X-osv", Values.Firebase.OS_VERSION.ToString() },
                { "X-cliv", Values.Firebase.CLIV },
                { "X-gmsv", Values.Firebase.GCM_VERSION.ToString() },
                { "X-appid", fid },
                { "X-scope", scope },
                { "X-Goog-Firebase-Installations-Auth", installationToken },
                { "X-gmp_app_id", Values.Firebase.APP_ID },
                { "X-firebase-app-name-hash", Values.Register3.X_FIREBASE_APP_NAME_HASH },
                { "X-app_ver_name", Values.DC_APP_VERSION_NAME },
                { "app", Values.Register3.APP },
                { "device", androidCheckin.AndroidId.ToString() },
                { "app_ver", Values.DC_APP_VERSION_CODE.ToString() },
                { "info", Values.Firebase.INFO },
                { "plat", "0" },
                { "gcm_ver", Values.Register3.GCM_VERSION.ToString() },
                { "cert", Values.Register3.CERT },
                { "target_ver", Values.Firebase.TARGET_VER }
            }));
        }

        #region AppCheck

        public class AppCheck
        {
            public bool Result { get; set; }
            public string Version { get; set; }
            public bool Notice { get; set; }
            public bool NoticeUpdate { get; set; }
            public string Date { get; set; }
        }
        public async Task<AppCheck> GetAppCheck()
        {
            HttpResponse appCheck = await GetAsync(ApiUrls.Auth.APP_CHECK, GetHeaders());
            Dictionary<string, object> appCheckResponse = (Dictionary<string, object>) Json.Decode<List<object>>(await appCheck.Message.Content.ReadAsStringAsync())[0];

            return new AppCheck
            {
                Result = (bool)appCheckResponse["result"],
                Version = (string)appCheckResponse["ver"],
                Notice = (bool)appCheckResponse["notice"],
                NoticeUpdate = (bool)appCheckResponse["notice_update"],
                Date = (string)appCheckResponse["date"]
            };
        }
        #endregion

        #region
        private bool NeedsRefresh(DateTime? oldDate, DateTime newDate)
        {
            if (oldDate == null || !oldDate.HasValue) throw new ArgumentNullException(nameof(oldDate));

            return oldDate.Value.Year != newDate.Year ||
                   oldDate.Value.Month != newDate.Month ||
                   oldDate.Value.Day != newDate.Day ||
                   oldDate.Value.Hour != newDate.Hour;
        }

        private string DateToString(DateTime dateTime)
        {
            int dayOfYear = dateTime.DayOfYear;
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            int weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int dayOfWeekMonday = ((int)dayOfWeek + 6) % 7 + 1;
            string dayOfWeekAbbreviation = dayOfWeek.ToString().Substring(0, 3);
            return string.Format(
                "{0}{1}d{2}{3}{4:00}M{5:00}MM",
                dayOfWeekAbbreviation,
                dayOfYear - 1,
                dayOfWeekMonday,
                (int)dayOfWeek - 1,
                weekOfYear,
                dateTime.Day,
                dateTime.Month
            );
        }

        private string GenerateHexString(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder hex = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes)
                {
                    hex.AppendFormat("{0:x2}", b);
                }
                return hex.ToString();
            }
        }
        #endregion
    }
}
