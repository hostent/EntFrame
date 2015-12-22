using Ent.Common;
using Ent.Common.Cache;
using Ent.Common.Thread;
using Ent.Model.Colection;
using Ent.Model.Service.User;
using Ent.Model.Untility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Service.User
{
    public class CReg : IReg
    {
        public string CreateValidCode(string phoneNum)
        {
            CacheHelp.Default.Add(phoneNum, "");
            throw new NotImplementedException();
        }

        public void CreateValidSms(string phoneNum)
        {
            throw new NotImplementedException();
        }

        public Result HasRegedit(string phoneNum)
        {
            int count = _Db.SqlDb.Query<Ent.Model.Table.U_User>().Where(q => q.PhoneNum == phoneNum).Count();

            if (count > 0)
            {
                return Result.Failure("该电话号码已经被注册");
            }
            return Result.Succeed();
        }

        public Result Regedit(string phoneNum, string psw)
        {
            //检查图像码，检查短信码

            //判断数据有效性
            if (HasRegedit(phoneNum).Tag == 1)
            {

            }

            //异步方法
            Asyn.Invork(() =>
            {


            });

            //添加数据入库

            //返回成功消息

            throw new NotImplementedException();
        }
    }
}
