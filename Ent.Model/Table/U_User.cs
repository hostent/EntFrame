using Ent.Model.Untility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Table
{
    public class U_User : BaseTable
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNum { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 是否能登陆
        /// </summary>
        public bool LoginEnable { get; set; }
    }
}
