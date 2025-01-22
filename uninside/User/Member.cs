using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.User
{
    public class Member
    {
        public string Id { get; private set; }
        public string Name { get; private set; }

        public Member(string userId, string userName)
        {
            Id = userId;
            Name = userName;
        }
    }
}
