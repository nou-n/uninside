using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.User
{
    public class Session
    {
        IUser User;
        SessionDetail Detail;

        public Session(IUser user, SessionDetail detail)
        {
            User = user;
            Detail = detail;
        }
    }
}
