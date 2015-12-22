//------------------------------------------------------------------------------------------
// <copyright file="DateUtils.cs" company="富银金融信息服务有限公司">
//     Copyright (c) 富银金融信息服务有限公司. All rights reserved.
// </copyright>
// <author>张伟</author>
//------------------------------------------------------------------------------------------
namespace Ent.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 时间操作辅助类
    /// </summary>
    public class DateUtils
    {
        /// <summary>
        /// 增加指定的天数
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <param name="days">增加的天数</param>
        /// <param name="ignoreHour">是否忽略小时，true将计算为当天的24点</param>
        /// <returns>增加后的时间</returns>
        public static DateTime AddDays(DateTime date, double days, bool ignoreHour)
        {
            if (ignoreHour)
            {
                return DateTime.Parse(date.AddDays(days+1).ToString("yyyy-MM-dd")+" 00:00:00");
            }

            return date.AddDays(days);
        }

        /// <summary>
        /// 将字符串根据指定的格式转为DateTime类型
        /// </summary>
        /// <param name="value">时间字符串</param>
        /// <param name="format">格式化字符串</param>
        /// <returns>DateTime类型</returns>
        public static DateTime ToDateTime(string value, string format)
        {
            return DateTime.ParseExact(value, format, CultureInfo.CurrentCulture);
        }
    }
}
