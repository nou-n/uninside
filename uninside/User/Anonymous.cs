using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.User
{
    public class Anonymous: IUser
    {
        public string Id { get; private set; }
        public string Password { get; private set; }

        public Anonymous(string id, string password)
        {
            Id = id;
            Password = password;
        }
    }
}
