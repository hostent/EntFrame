using Ent.Model.Entity.Context;
using Ent.Model.Untility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Service.User
{
    public interface ICurrentUser
    {
        UserContext Get(string userKey);

        void RemoveCache(string userKey);

        Result Login(string userName, string password, string userOldKey);
    }
}
