using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common
{
    /// <summary>
    /// id 生成算法，顺序，性能还不错，一秒钟可以几百万次，最后一位是机器码，也就是可以同时支持16台机子
    /// </summary>
    public static class IDGenerator
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        public static extern int UuidCreateSequential(out Guid guid);
        private const int RPC_S_OK = 0;

        static Random random = new Random();

        public static string NewID()
        {
            string resultStr = "";
            Guid guid;
            int result = UuidCreateSequential(out guid);
            if (result == RPC_S_OK)
            {
                string str = guid.ToString();

                resultStr = resultStr + str.Substring(14, 4);
                resultStr = resultStr + str.Substring(9, 4);
                resultStr = resultStr + str.Substring(0, 8);

                //添加一位机器码
                resultStr = resultStr + GetMachineCode();



                return resultStr.ToString();
            }
            else
                throw new Exception("RPC is not OK");
        }

        private static string GetMachineCode()
        {
            string mCode = ""; // 0 到f

            if (mCode == "")
            {
                int n = random.Next(0, 15);
                mCode = n.ToString("x");

            }

            return mCode;

        }

    }
}
