using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.User.Named
{
    public class DuplicateNamed : LoginUser
    {
        public DuplicateNamed(string id, string password) : base(id, password)
        {
        }
    }
}
