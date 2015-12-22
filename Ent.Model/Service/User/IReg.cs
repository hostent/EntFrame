
using Ent.Model.Untility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Service.User
{
    public interface IReg
    {
        /// <summary>
        /// 创建图像验证码
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        string CreateValidCode(string phoneNum);


        /// <summary>
        /// 生成短信验证码
        /// </summary>
        /// <param name="phoneNum"></param>
        void CreateValidSms(string phoneNum);

        /// <summary>
        /// 检查是否有注册
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        Result HasRegedit(string phoneNum);


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="psw"></param>
        /// <returns></returns>
        Result Regedit(string phoneNum, string psw);

    }
}
