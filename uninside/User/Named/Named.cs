using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.User.Named
{
    public class Named : LoginUser
    {
        public Named(string id, string password) : base(id, password)
        {
        }
    }
}
