using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.User
{
    public class SessionDetail
    {
        public string Id { get; private set; }
        public string No { get; private set; }
        public string Name { get; private set; }
        public Int64 IsAdult { get; private set; }
        public Int64 IsDormancy { get; private set; }
        public Int64 IsOTP { get; private set; }
        public Int64 IsFixedName { get; private set; } // isGonick
        public string SecurityCode { get; private set; }
        public Int64 AuthChange { get; private set; }
        public string SessionType { get; private set; }
        public Int64 PasswordCampaign {  get; private set; }

        public SessionDetail(string userId, string userNo, string name, string isAdult, Int64 isDormancy, Int64 isOTP, Int64 isGonick, string isSecurityCode, Int64 authChange, string sType, Int64 pwCampaign)
        {
            Id = userId;
            No = userNo;
            Name = name;
            IsAdult = int.Parse(isAdult ?? "-1");
            IsDormancy = isDormancy;
            IsOTP = isOTP;
            IsFixedName = isGonick;
            SecurityCode = isSecurityCode;
            AuthChange = authChange;
            SessionType = sType;
            PasswordCampaign = pwCampaign;
        }
    }
}
