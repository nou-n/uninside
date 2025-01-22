using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.User
{
    public class LoginUser: IUser
    {
        public string Id { get; private set; }
        public string Password { get; private set; }

        public LoginUser(string id, string password)
        {
            Id = id;
            Password = password;
        }
    }
}
