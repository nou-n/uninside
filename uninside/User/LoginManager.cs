using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tiny;
using uninside.Http;
using uninside.User.Named;
using uninside.Util;

namespace uninside.User
{
    public class LoginManager
    {
        public Uninside Client { get; private set; }

        public LoginManager(Uninside uninside)
        {
            this.Client = uninside;
        }

        public async Task<Session> Login(IUser user)
        {
            if(!(user is Anonymous))
            {
                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent("login_quick"), "mode");
                formData.Add(new StringContent(user.Password), "user_pw");
                formData.Add(new StringContent(user.Id), "user_id");
                formData.Add(new StringContent(Client.AppInstance.Fcm), "client_token");

                HttpResponse response = await HttpRequest.PostAsync(ApiUrls.Auth.LOGIN, headers: Utils.defaultHeaders, payload: formData);

                string str = await response.Message.Content.ReadAsStringAsync();
                
                if(str.StartsWith("["))
                {
                    Dictionary<string, object> failedData = Json.Decode<List<Dictionary<string, object>>>(str)[0];

                    if (!failedData.ContainsKey("result")) throw new Exception("로그인 실패");

                    throw new Exception("로그인 실패 (Cause: " + (
                        (string)failedData.GetValue("cause") ?? "null"
                    ) + ")");
                }

                Dictionary<string, object> data = Json.Decode<Dictionary<string, object>>(str);

                if (!(bool)data["result"]) throw new Exception((string)data.GetValue("cause") ?? "로그인 실패");

                if(!data.ContainsKey("stype")) throw new Exception("stype 키를 찾을 수 없습니다. (Response: " + Json.Encode(data) + ")");

                SessionDetail detail = new SessionDetail(
                    (string)data.GetValue("user_id"),
                    (string)data.GetValue("user_no"),
                    (string)data.GetValue("name"),
                    (string)data.GetValue("is_adult"),
                    (Int64)(data.GetValue("is_dormancy") ?? -1),
                    (Int64)(data.GetValue("is_otp") ?? -1),
                    (Int64)(data.GetValue("is_gonick") ?? -1),
                    (string)data.GetValue("is_security_code"),
                    (Int64)(data.GetValue("auth_change") ?? -1),
                    (string)data["stype"],
                    (Int64)(data.GetValue("pw_campaign") ?? -1)
                );

                LoginUser loginUser;
                switch(Utils.GetUserType((string)data["stype"]))
                {
                    case UserType.NAMED:
                        loginUser = new Named.Named(user.Id, user.Password);
                        break;
                    case UserType.DUPLICATE_NAMED:
                        loginUser = new DuplicateNamed(user.Id, user.Password);
                        break;
                    default:
                        throw new Exception("계정의 타입을 알 수 없습니다.");
                }

                return new Session(loginUser, detail);
            }
            return new Session(user, null);
        }
    }

    public enum UserType
    {
        ANONYMOUS, // 익명
        NAMED, // 고정
        DUPLICATE_NAMED // 반고정
    }
}
