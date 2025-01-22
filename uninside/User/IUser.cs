using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uninside.User
{
    public interface IUser
    {
        string Id { get; }
        string Password { get; }
    }
}
