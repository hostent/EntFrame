using Ent.Model.Colection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Service.User
{
    public interface IUserInfo
    {
        U_User Get(string id);

        void Save(U_User user);
    }


}
