using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common.Security
{
    public class Base64
    {
        /// <summary>
        /// base64 编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EnCode(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// base64 解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DeCode(string str)
        {
            byte[] outputb = Convert.FromBase64String(str);
            return Encoding.Default.GetString(outputb);
        }
    }
}
