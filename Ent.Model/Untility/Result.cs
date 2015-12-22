﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Untility
{
    public class Result
    {

        public Result(int tag = 1, string msg = "", object data = null)
        {
            Message = msg;
            Tag = tag;
            Data = data;


        }
        public string Message { get; set; }

        /// <summary>
        /// 1 表示成功
        /// </summary>
        public int Tag { get; set; }

        public object Data { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string RedirectUrl { get; set; }

        public static Result Succeed()
        {

            return new Result(1, "", null);

        }

        public static Result Failure(string message)
        {

            return new Result(0, message, null);

        }

        public static Result Failure(int tag, string message)
        {

            return new Result(tag, message, null);

        }

        public static Result Succeed(object data)
        {
            return new Result(1, "", data);
        }

        /// <summary>
        /// 获取执行结果是否成功
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        public bool IsSucceed()
        {
            return this.Tag == 1;
        }

        /// <summary>
        /// 获取执行结果是否失败
        /// </summary>
        /// <returns>失败返回true，否则返回false</returns>
        public bool IsFailure()
        {
            return this.Tag == 0;
        }
    }
}
