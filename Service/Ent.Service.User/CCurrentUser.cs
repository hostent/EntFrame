using Ent.Common.Cache;
using Ent.Common.Mvc;
using Ent.Common.Security;
using Ent.Model.Colection;
using Ent.Model.Entity.Context;
using Ent.Model.Service.User;
using Ent.Model.Untility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Service.User
{
    public class CCurrentUser : ICurrentUser
    {
        public UserContext Get(string userKey)
        {
            UserContext context = new UserContext();

            string userId = AESHelper.AESDecrypt(userKey);

            Guid tempUserID = Guid.Empty;

            if (Guid.TryParse(userId, out tempUserID))
            {
                return null;
            }

            if (CacheHelp.Default.Get(userKey) != null)
            {
                return (UserContext)CacheHelp.Default.Get(userKey);

            }

            context.User = _Db.SqlDb.Query<U_User>().Where(q => q.Id == userId).FirstOrDefault();

            if (context.User == null)
            {
                return null;
            }

            CacheHelp.Default.Add(userKey, context);


            return context;

        }

        public void RemoveCache(string userKey)
        {
            CacheHelp.Default.Remove(userKey);

        }

        public Result Login(string userName, string password, string userOldKey)
        {

            var user = _Db.SqlDb.Query<U_User>().Where(q => q.NickName == userName).FirstOrDefault();

            if (user == null)
            {
                return Result.Failure(-11, "当前用户不存在");
            }
            if (user.Password != MD5Helper.Encrypt_MD5(password))
            {
                return Result.Failure(-11, "请输入正确密码");
            }
            if (!user.LoginEnable)
            {
                return Result.Failure(-11, "当前用户被禁止登陆");
            }


            // todo 
            // 购物车 等问题

            string userNewKey = AESHelper.AESEncrypt(user.Id);

            return Result.Succeed(userNewKey);

        }


    }
}
