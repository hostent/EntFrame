using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Ent.Common
{
    public class DataFormat
    {
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        /// <summary>
        /// 删除分隔符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveSplit(object value)
        {
            if (value == null)
            {
                return "";
            }
            string newValue = value.ToString().Trim();
            newValue = newValue.Replace("-", "");
            newValue = newValue.Replace("/", "");
            newValue = newValue.Replace("(", "");
            newValue = newValue.Replace(")", "");
            return newValue;
        }
        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateTimeMMDDYYYY(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return "";
            }
            else
            {
                return DateTime.Parse(value.ToString()).ToString("MM/dd/yyyy");
            }
        }

        /// <summary>
        /// dd/MM/yyyy
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateTimeDDMMYYYY(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return "";
            }
            else
            {
                return DateTime.Parse(value.ToString()).ToString("dd/MM/yyyy");
            }
        }

        /// <summary>
        /// HTML JQ DateControl (dd/MM/yyyy) 转为 Date (MM/dd/yyyy)
        /// </summary>
        /// <param name="jqDateTControlValue"></param>
        /// <returns></returns>
        public static string DateTimeStringForMY(string MYCulturDate)
        {
            try
            {
                CultureInfo culture = new CultureInfo("ms-MY");
                return Convert.ToDateTime(MYCulturDate.Trim(), culture).ToString("MM/dd/yyyy");
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// dd/MM/yyyy hh:mm
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateAndTimeString(string value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return "";
            }
            else
            {
                return DateTime.Parse(value.ToString()).ToString("dd/MM/yyyy HH:mm");
            }
        }

        /// <summary>
        /// 格式化为美国格式的日期
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static string ToUsaDateString(string dateString)
        {
            if ((dateString.IndexOf("/") == -1) && (dateString.Length == 8) && (dateString.IndexOf('-') == -1))
            {
                dateString = dateString.Substring(0, 2) + "/" + dateString.Substring(2, 2) + "/" + dateString.Substring(4, 4);
            }
            return dateString;
        }
        /// <summary>
        /// 格式化为美国格式的日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToUsaDateString(DateTime? dateTime)
        {
            if (!dateTime.HasValue) return string.Empty;
            DateTime value = dateTime.Value;
            string month = value.Month < 10 ? "0" + value.Month : value.Month.ToString();
            string day = value.Day < 10 ? "0" + value.Day : value.Day.ToString();
            return string.Format("{0}/{1}/{2}", month, day, value.Year);
        }

        /// <summary>
        /// 获取指定长度的字符串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Substring(string content, int startIndex, int length)
        {
            StringInfo value = new StringInfo(content);
            if (value.LengthInTextElements > length) return value.SubstringByTextElements(startIndex, length) + "...";
            return content;
        }

        /// <summary>
        /// 中英字符截取。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static string SubstringForZhEn(string s, int l)
        {
            string temp = s;
            if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l)
            {
                return temp;
            }
            for (int i = temp.Length; i >= 0; i--)
            {
                temp = temp.Substring(0, i);
                if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l - 3)
                {
                    return temp + "...";
                }
            }
            return "";
        }
        /// <summary>
        /// 格式化HTML格式
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string FormatTitle(object content)
        {
            if (content == null) return string.Empty;
            if (string.IsNullOrEmpty(content.ToString())) return content.ToString();
            return HttpUtility.HtmlEncode(content.ToString());
        }

        /// <summary>
        /// 比较指定时间和现在的距离
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string CompareToNow(string dateTime)
        {
            if (!DataConvert.IsDateTimeFormat(dateTime)) return string.Empty;
            DateTime now = DateTime.Now;
            DateTime? compareTime = DataConvert.ToDateTime(dateTime);
            TimeSpan timeSpan = now.Subtract(compareTime.Value);
            int minutes = timeSpan.Minutes;
            if (minutes < 60) return string.Format("{0} 分钟前", minutes);
            return string.Format("{0} 小时前", minutes / 60);
        }
        /// <summary>
        /// 是否比今天早
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool BeforeToday(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime)) return false;
            if (!DataConvert.IsDateTimeFormat(dateTime)) return false;
            DateTime compareTime = DataConvert.ToDateTime(dateTime).Value;
            if (DateTime.Today.CompareTo(compareTime) > 0) return true;
            return false;
        }
        /// <summary>
        /// 保留2位小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToFloat(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return string.Empty;
            return String.Format("{0:F}", value);
        }
        /// <summary>
        /// 删除字符中的HTML内容
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveHtml(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            Regex regex1 = new Regex(@"<script[\s\S]+</script *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@" href *= *[\s\S]*script *:", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" no[\s\S]*=", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex(@"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase);
            Regex regex6 = new Regex(@"\<img[^\>]+\>", RegexOptions.IgnoreCase);
            Regex regex7 = new Regex(@"</p>", RegexOptions.IgnoreCase);
            Regex regex8 = new Regex(@"<p>", RegexOptions.IgnoreCase);
            Regex regex9 = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
            Regex regex10 = new Regex(@"<.*?>", RegexOptions.IgnoreCase);
            value = regex1.Replace(value, ""); //过滤<script></script>标记 
            value = regex2.Replace(value, ""); //过滤href=javascript: (<A>) 属性 
            value = regex3.Replace(value, " _disibledevent="); //过滤其它控件的on...事件 
            value = regex4.Replace(value, ""); //过滤iframe 
            value = regex5.Replace(value, ""); //过滤frameset 
            value = regex6.Replace(value, ""); //过滤frameset 
            value = regex7.Replace(value, ""); //过滤frameset 
            value = regex8.Replace(value, ""); //过滤frameset 
            value = regex9.Replace(value, "");
            value = regex10.Replace(value, "");
            value = value.Replace("</strong>", "");
            value = value.Replace("<strong>", "");
            value = value.Replace("&nbsp;", " ");
            value = value.Trim();
            return value;
        }

        /// <summary> 
        /// 取得HTML中所有图片的 URL。 
        /// </summary> 
        /// <param name="sHtmlText">HTML代码</param> 
        /// <returns>图片的URL列表</returns> 
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }

        /// <summary>
        /// 将文本框的内容转化成HTML输出，比如空格和换行
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHtml(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return string.Empty;
            return value.ToString().Replace("\r\n", "<br/>").Replace("\n", "<br/>").Replace(" ", "&nbsp;");
        }

        /// <summary>
        /// 格式化会员昵称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatSensitiveString(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length < 3) return "***";
            return string.Format("{0}***{1}", value.Substring(0, 2), value.Substring(value.Length - 1, 1));
        }

        /// <summary>
        /// 格式化姓名
        /// </summary>
        /// <param name="realName">姓名</param>
        /// <returns></returns>
        public static string FormatRealName(string realName)
        {
            if (string.IsNullOrEmpty(realName))
            {
                return realName;
            }
            return string.Format("{0}*{1}", realName.Substring(0, 1), realName.Substring(realName.Length - 1));
        }

        /// <summary>
        /// 格式化敏感会员Email,替换掉@前面的三位 AAAAAA***@gmail.com
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatSensitiveEmail(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length < 4) return string.Format("{0}**", value.Substring(0, 2));
            if (!value.Contains("@")) return value;
            string accountStr = value.Substring(0, value.IndexOf("@", StringComparison.OrdinalIgnoreCase));
            if (accountStr.Length < 3) accountStr = accountStr + "***";
            string mailStr = value.Substring(value.IndexOf("@", StringComparison.OrdinalIgnoreCase), value.Length - accountStr.Length);
            return string.Format("{0}***{1}", accountStr.Substring(0, accountStr.Length - 3), mailStr);
        }

        /// <summary>
        /// 格式化敏感电话号码,替换掉中间两位数 88**8888
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatSensitivePhone(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length < 4) return string.Format("{0}**", value.Substring(0, 2));
            return string.Format("{0}**{1}", value.Substring(0, 2), value.Substring(4, value.Length - 4));
        }

        /// <summary>
        /// 格式化手机号码，例：188****8888
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public static string FormatPhoneNum(string phoneNum)
        {
            if (string.IsNullOrEmpty(phoneNum))
            {
                return phoneNum;
            }
            return string.Format("{0}****{1}", phoneNum.Substring(0, 3), phoneNum.Substring(7));
        }

        /// <summary>
        /// 格式化身份证号,显示前三位和后四位
        /// </summary>
        /// <param name="idcard">身份证号</param>
        /// <returns>格式化后的身份证号</returns>
        public static string FormatIDCard(string idcard)
        {
            if (string.IsNullOrEmpty(idcard))
            {
                return string.Empty;
            }

            int length = idcard.Length;
            if (length < 7)
            {
                return idcard;
            }

            return idcard.Substring(0, 3) + "".PadLeft(length - 7, '*') + idcard.Substring(length - 4, 4);
        }

        /// <summary>
        /// 格式化银行卡号,显示前两位和后四位
        /// </summary>
        /// <param name="bankcard">银行卡号</param>
        /// <returns>格式化后的银行卡号</returns>
        public static string FormatBankCard(string bankcard)
        {
            if (string.IsNullOrEmpty(bankcard))
            {
                return string.Empty;
            }

            int length = bankcard.Length;
            if (length < 6)
            {
                return bankcard;
            }

            return bankcard.Substring(0, 2) + "".PadLeft(length - 6, '*') + bankcard.Substring(length - 4, 4);
        }

        /// <summary>
        /// 格式化银行卡号,显示后四位
        /// </summary>
        /// <param name="bankcard">银行卡号</param>
        /// <returns>格式化后的银行卡号</returns>
        public static string FormatShortBankCard(string bankcard)
        {
            if (string.IsNullOrEmpty(bankcard))
            {
                return string.Empty;
            }

            int length = bankcard.Length;
            if (length < 4)
            {
                return bankcard;
            }

            return bankcard.Substring(length - 4, 4);
        }
        /// <summary>
        /// 格式化用户名称
        /// </summary>
        /// <param name="name">用户名称</param>
        /// <returns>张**三</returns>
        public static string FormatName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            if (name.Length <= 2)
            {
                return name.Substring(0, 1) + "*";
            }

            return name.Substring(0, 1) + "*" + name.Substring(name.Length - 1, 1);
        }

        /// <summary>
        /// 正则截取字符串中的中文
        /// </summary>
        /// <param name="strSource">字符串</param>
        /// <returns></returns>
        public static string GetChinese(string strSource)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]");
            int nLength = strSource.Length;
            string chineseString = string.Empty;
            for (int i = 0; i < strSource.Length; i++)
            {
                if (regex.IsMatch(strSource.Substring(i, 1)))
                {
                    chineseString = chineseString + strSource.Substring(i, 1);
                }
            }
            return chineseString; 
        }

        /// <summary>
        /// 生成主键字符串
        /// </summary>
        /// <returns>主键字符串</returns>
        public static string GetGuId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }


    public class DataConvert
    {

        public static DateTime? ToDateTime(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                return Convert.ToDateTime(value);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static DateTime? ToNullableDateTime(object value)
        {
            if (value == null) return null;
            DateTime dateTime;
            if (DateTime.TryParse(value.ToString(), out dateTime)) return dateTime;
            else return null;
        }

        /// <summary>
        /// 将秒转换成天时分秒
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        public static string ToTimeStr(int seconds)
        {
            string TimeStr = "";

            double value = seconds / (24 * 60 * 60);
            int days = (int)value;//天
            seconds -= days * (24 * 60 * 60);//剩余秒数
            value = seconds / (60 * 60);
            if (value < 0)
            {//<0
                value = 0;    
            }
            int hours = (int)value;//时
            seconds -= hours * (60 * 60);//剩余秒数
            value = seconds / 60;
            if (value < 0)
            {//<0
                value = 0;
            }
            int minutes = (int)value;//分
            seconds -= minutes * 60;//剩余描述
            if (seconds < 0)
            {
                seconds = 0;
            }

            if (days > 0)
            {//天
                TimeStr += days.ToString() + "天";
            }
            if (hours > 0)
            {//时
                TimeStr += hours.ToString() + "小时";
            }
            if (minutes > 0)
            {//分
                TimeStr += minutes.ToString() + "分";
            }
            if (seconds > 0)
            {//秒
                TimeStr += seconds.ToString() + "秒";
            }

            return TimeStr;
        }

        public static int ToInt32(string value)
        {
            int num;
            if (int.TryParse(value, out num))
            {
                return num;
            }
            return 0;
        }

        public static int ToInt32(int? value)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return 0;
            }
            return Convert.ToInt32(value);
        }

        public static long ToInt64(string value)
        {
            long num;
            if (long.TryParse(value, out num))
            {
                return num;
            }
            return 0;
        }

        public static decimal ToDecimal(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            return ToNullableDecimal(value).Value;
        }

        public static double? ToNullableDouble(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return Convert.ToDouble(value);
        }

        public static decimal? ToNullableDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return Convert.ToDecimal(value);
        }

        public static double ToDouble(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            return ToNullableDouble(value).Value;
        }

        public static double ToDouble(double? value)
        {
            if (value.HasValue)
                return value.Value;
            return 0;
        }

        public static int ToInt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            return Convert.ToInt32(value);
        }

        public static int ToZeroInt(object value)
        {
            if (value == null) return 0;
            if (string.IsNullOrEmpty(value.ToString())) return 0;
            return Convert.ToInt32(value);
        }

        public static int ToInt32(object value)
        {
            if (value == null)
                return -1;
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return -1;
            }
            return Convert.ToInt32(value);
        }

        public static string ToString(DateTime value)
        {
            if (value == DateTime.MinValue)
            {
                return "";
            }
            else
            {
                return value.ToShortDateString();
            }
        }

        public static string ToString(object value)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateString(DateTime? value)
        {
            if (value == null || value == DateTime.MinValue)
            {
                return "";
            }
            else
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                return Convert.ToDateTime(value).ToString("yyyy-MM-dd", culture);
            }
        }

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateStringOrDefault(DateTime value)
        {
            if (value == null || value == DateTime.MinValue)
            {
                return "- -";
            }
            else
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                return value.ToString("yyyy-MM-dd", culture);
            }
        }

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateStringOrDefault(DateTime? value)
        {
            if (!value.HasValue || value.Value == null || value.Value == DateTime.MinValue)
            {
                return "- -";
            }
            else
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                return value.Value.ToString("yyyy-MM-dd", culture);
            }
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString(DateTime? value)
        {
            if (value == null || value == DateTime.MinValue)
            {
                return "";
            }
            else
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                return Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm", culture);
            }
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="value"></param>
        /// <returns>yyyy-MM-dd HH:mm/- -</returns>
        public static string ToDateTimeStringOrDefault(DateTime? value)
        {
            if (!value.HasValue || value.Value == null || value.Value == DateTime.MinValue)
            {
                return "- -";
            }
            else
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                return value.Value.ToString("yyyy-MM-dd HH:mm", culture);
            }
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="value"></param>
        /// <returns>yyyy-MM-dd HH:mm/- -</returns>
        public static string ToDateTimeStringOrDefault(DateTime value)
        {
            if (value == null || value == DateTime.MinValue)
            {
                return "- -";
            }
            else
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                return value.ToString("yyyy-MM-dd HH:mm", culture);
            }
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeFullString(DateTime? value)
        {
            if (value == null || value == DateTime.MinValue)
            {
                return "";
            }
            else
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                return Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss", culture);
            }
        }

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            else
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                return Convert.ToDateTime(value).ToString("yyyy-MM-dd", culture);
            }
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            else
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                return Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm", culture);
            }
        }
        public static DateTime NullToDateNow(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return DateTime.Now;
            }
            else
            {
                return Convert.ToDateTime(value);
            }
        }
        public static bool ToBoolean(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Trim().ToUpper().Equals("Y")) return true;
                else if (value.Trim().ToUpper().Equals("TRUE")) return true;
                else if (value.Trim().ToUpper().Equals("1")) return true;
            }
            return false;
        }

        public static bool ToBoolean(object value)
        {
            if (value == null) return false;
            if (value.ToString().ToUpper() == "Y") return true;
            if (value.ToString().ToUpper() == "TRUE") return true;
            if (value.ToString().ToUpper() == "1") return true;
            return false;
        }

        public static string ToBooleanString(bool value)
        {
            if (value)
            {
                return "Y";
            }
            else
            {
                return "N";
            }
        }

        public static string ToBooleanString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "N";
            }
            if (value == "Y" || value == "N")
            {
                return value;
            }
            try
            {
                if (Convert.ToBoolean(value))
                {
                    return "Y";
                }
                else
                {
                    return "N";
                }
            }
            catch
            {
                return "N";
            }
        }

        public static string ToString(int? value)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                return value.ToString();
            }
        }

        public static string ToString(int value)
        {
            if (value == null)
                return "";
            else
                return value.ToString();
        }

        public static string ToString(decimal value)
        {
            if (value == 0)
            {
                return "";
            }
            else
            {
                return value.ToString();
            }
        }

        public static string ToString(char value)
        {
            if (value == ' ' || value == '\0')
            {
                return "";
            }
            else
            {
                return value.ToString();
            }
        }

        public static string ToString(decimal? value)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                return value.ToString();
            }
        }

        public static char? ToChar(string value)
        {
            if (value == null || value.Equals(""))
            {
                return null;
            }
            else
            {
                return value[0];
            }
        }
        public static int? ToNullableInt32(object value)
        {
            if (value == null) return null;
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
            return Convert.ToInt32(value, CultureInfo.CurrentCulture);
        }

        public static bool? ToNullableBoolean(string value)
        {
            if (value == null) return null;
            return DataConvert.ToBoolean(value);
        }

        public static string ToNullableBooleanString(bool? value)
        {
            if (value == null) return null;
            return DataConvert.ToBooleanString(value.Value);
        }

        public static int? ToNullableInt32(string value)
        {
            if (value == null) return null;
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
            return Convert.ToInt32(value, CultureInfo.CurrentCulture);
        }
        /// <summary>
        /// 转化为可空的长整数类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long? ToNullableInt64(string value)
        {
            if (value == null) return null;
            long key;
            if (string.IsNullOrEmpty(value.ToString()) || !long.TryParse(value, out key))
            {
                return null;
            }
            return key;
        }

        public static string ConvertToNull(string value)
        {
            if (value.Trim() == "")
            {
                return null;
            }
            return value.Trim();
        }

        public static bool IsDateTimeFormat(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) return false;
                Convert.ToDateTime(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDecimal(string value)
        {
            try
            {
                Convert.ToDecimal(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInteger(string value)
        {
            try
            {
                Convert.ToInt32(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsLong(string value)
        {
            try
            {
                Convert.ToInt64(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static double GetMaxValue(double value1, double value2)
        {
            return (value1 > value2) ? value1 : value2;
        }

        public static decimal Rounding(double value)
        {
            if (value < 0) return -RoundingPostive(-value);
            return RoundingPostive(value);
        }
        /// <summary>
        /// 正数四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static decimal RoundingPostive(double value)
        {
            if (value < 0) throw new Exception("方法只能格式化正数。");
            double vt = Math.Pow(10, 2);
            double vx = value * vt;

            vx += 0.5;
            return DataConvert.ToDecimal((Math.Floor(vx) / vt).ToString());
        }

        /// <summary>
        /// 截取小数(非四舍五入)
        /// </summary>
        /// <param name="value">数值字符串</param>
        /// <param name="length">小数位数</param>
        /// <returns></returns>
        public static string InterceptDecimal(string value, int length)
        {
            if (!string.IsNullOrEmpty(value))
            {//为空
                var len = value.IndexOf(".");
                if (len == -1)
                {//不含小数点
                    value += ".";
                    for (int i = 0; i < length; i++)
                    {
                        value += "0";
                    }
                }
                else
                {//含小数点
                    var num = value.Length - len - 1;
                    if (num < length)
                    {
                        for (int i = num; i < length; i++)
                        {
                            value += "0";
                        }
                    }
                    else
                    {
                        value = value.Substring(0, len + length + 1);
                    }
                }                
            }

            return value;
        }

        public static decimal Rounding(decimal value)
        {
            return Convert.ToDecimal(Rounding(Convert.ToDouble(value)));
        }

        public static decimal Rounding(decimal? value)
        {
            return Convert.ToDecimal(Rounding(Convert.ToDouble(value)));
        }

        /// <summary>
        /// 金额精度处理
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static decimal Money(decimal? money)
        {
            if (!money.HasValue)
            {
                return 0;
            }

            return Money(money);
        }

        /// <summary>
        /// 金额精度处理
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static decimal Money(decimal money)
        {
            if (money == 0)
            {
                return 0;
            }

            var temp = (money * 100).ToString().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];

            return decimal.Parse(temp) / 100;
        }
    }

}
