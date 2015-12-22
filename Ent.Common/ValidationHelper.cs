//------------------------------------------------------------------------------------------
// <copyright file="ValidationHelper.cs" company="富银金融信息服务有限公司">
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
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 数据验证辅助类
    /// </summary>
    public class ValidationHelper
    {
        /// <summary>
        /// 待验证的数据
        /// </summary>
        private object _value;

        /// <summary>
        /// 数据显示名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 链式验证时，当前一个验证失败时，是否继续
        /// </summary>
        private bool _iscontinue;

        /// <summary>
        /// 验证失败提示消息
        /// </summary>
        private List<string> _message;

        /// <summary>
        /// 上一次验证结果
        /// </summary>
        private bool _lastResult = true;

        /// <summary>
        /// 待验证的数据
        /// </summary>
        public object Value
        {
            get
            {
                return this._value;
            }

            set
            {
                this._value = value;
            }
        }

        /// <summary>
        /// 数据显示名称
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
            }
        }

        /// <summary>
        /// 链式验证时，当前一个验证失败时，是否继续
        /// </summary>
        public bool IsContinue
        {
            get
            {
                return this._iscontinue;
            }

            set
            {
                this._iscontinue = value;
            }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public ValidationHelper()
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="value">待验证的数据</param>
        /// <param name="name">数据显示名称</param>
        /// <param name="iscontinue">链式验证时，当前一个验证失败时，是否继续</param>
        public ValidationHelper(object value, string name, bool iscontinue = false)
        {
            this._value = value;
            this._name = name;
            this._iscontinue = iscontinue;
        }

        /// <summary>
        /// 验证是否不为空
        /// </summary>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper IsNotEmpty()
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                if (this._value == null || string.IsNullOrEmpty(this._value.ToString()))
                {
                    this.ValidateFaill("{0}不能为空", this._name);
                }
                else
                {
                    this.ValidateSuccess();
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 验证是数字
        /// </summary>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper IsNumber()
        {
            return this.Regular(@"^[-]?[1-9]{1}\d*$|^[0]{1}$");
        }

        /// <summary>
        /// 验证是时间
        /// </summary>
        /// <param name="format">时间格式化字符串</param>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper IsDate(string format)
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                DateTime dt;
                try
                {
                    dt = DateTime.ParseExact(this._value.ToString(), format, CultureInfo.CurrentCulture);
                    this.ValidateSuccess();
                }
                catch
                {
                    this.ValidateFaill("{0}不是有效的时间格式", this._name);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 验证是邮箱
        /// </summary>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper IsEMail()
        {
            return this.Regular(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        /// <summary>
        /// 验证身份证是否有效
        /// </summary>
        /// <returns></returns>
        public ValidationHelper IsIDCard()
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                var idCard = FormatTo18IDCard(this._value.ToString());
                if (idCard.Length != 18)
                {//非18位
                    this.ValidateFaill("{0}不是有效的身份证格式", this._name);
                }
                var cardBase = idCard.Substring(0, 17);
                if (GetIDCardVerifyNum(cardBase) == idCard.Substring(17,1).ToUpper())
                {//校验码正确
                    this.ValidateSuccess();
                }
                else
                {//校验码不正确
                    this.ValidateFaill("{0}不是有效的身份证格式", this._name);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 验证最小值
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper Min(int minValue)
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                int value;
                try
                {
                    if (int.TryParse(this._value.ToString(), out value))
                    {
                        if (value >= minValue)
                        {
                            this.ValidateSuccess();
                        }
                        else
                        {
                            this.ValidateFaill("{0}不能小于{1}", this._name, minValue);
                        }
                    }
                    else
                    {
                        this.ValidateFaill("{0}不是有效的数字", this._name);
                    }
                }
                catch
                {
                    this.ValidateFaill("{0}不是有效的数字", this._name);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 验证最小值
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper Min(decimal minValue)
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                decimal value;
                try
                {
                    if (decimal.TryParse(this._value.ToString(), out value))
                    {
                        if (value >= minValue)
                        {
                            this.ValidateSuccess();
                        }
                        else
                        {
                            this.ValidateFaill("{0}不能小于{1}", this._name, minValue);
                        }
                    }
                    else
                    {
                        this.ValidateFaill("{0}不是有效的数字", this._name);
                    }
                }
                catch
                {
                    this.ValidateFaill("{0}不是有效的数字", this._name);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 验证最大值
        /// </summary>
        /// <param name="maxValue">最大值</param>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper Max(int maxValue)
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                int value;
                try
                {
                    if (int.TryParse(this._value.ToString(), out value))
                    {
                        if (value <= maxValue)
                        {
                            this.ValidateSuccess();
                        }
                        else
                        {
                            this.ValidateFaill("{0}不能大于{1}", this._name, maxValue);
                        }
                    }
                    else
                    {
                        this.ValidateFaill("{0}不是有效的数字", this._name);
                    }
                }
                catch
                {
                    this.ValidateFaill("{0}不是有效的数字", this._name);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 验证最大值
        /// </summary>
        /// <param name="maxValue">最大值</param>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper Max(decimal maxValue)
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                decimal value;
                try
                {
                    if (decimal.TryParse(this._value.ToString(), out value))
                    {
                        if (value <= maxValue)
                        {
                            this.ValidateSuccess();
                        }
                        else
                        {
                            this.ValidateFaill("{0}不能大于{1}", this._name, maxValue);
                        }
                    }
                    else
                    {
                        this.ValidateFaill("{0}不是有效的数字", this._name);
                    }
                }
                catch
                {
                    this.ValidateFaill("{0}不是有效的数字", this._name);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 验证最小长度
        /// </summary>
        /// <param name="mixlength">最小长度</param>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper MinLength(int mixlength)
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                if (this._value.ToString().Length >= mixlength)
                {
                    this.ValidateSuccess();
                }
                else
                {
                    this.ValidateFaill("{0}最少应为{1}个字符", this._name, mixlength);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 验证最大长度
        /// </summary>
        /// <param name="maxlength">最大长度</param>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper MaxLength(int maxlength)
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                if (this._value.ToString().Length <= maxlength)
                {
                    this.ValidateSuccess();
                }
                else
                {
                    this.ValidateFaill("{0}最多只能{1}个字符", this._name, maxlength);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 范围验证
        /// </summary>
        /// <param name="array">有效值集合</param>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper In(int[] array)
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                int value;
                try
                {
                    if (int.TryParse(this._value.ToString(), out value))
                    {
                        if (array.ToList().Contains(value))
                        {
                            this.ValidateSuccess();
                        }
                        else
                        {
                            this.ValidateFaill("{0}不在有效的范围内", this._name);
                        }
                    }
                    else
                    {
                        this.ValidateFaill("{0}不是有效的数字", this._name);
                    }
                }
                catch
                {
                    this.ValidateFaill("{0}不是有效的数字", this._name);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 范围验证
        /// </summary>
        /// <param name="array">有效值集合</param>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper In(string[] array)
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                try
                {
                    if (array.ToList().Contains(this._value.ToString()))
                    {
                        this.ValidateSuccess();
                    }
                    else
                    {
                        this.ValidateFaill("{0}不在有效的范围内", this._name);
                    }
                }
                catch
                {
                    this.ValidateFaill("{0}数据无效", this._name);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;
        }

        /// <summary>
        /// 正则验证
        /// </summary>
        /// <param name="reqular">正则表达式</param>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper Regular(string reqular)
        {
            if (!this.NeedValidate())
            {
                return this;
            }

            try
            {
                Regex re = new Regex(reqular);
                if (re.IsMatch(this._value.ToString()))
                {
                    this.ValidateSuccess();
                }
                else
                {
                    this.ValidateFaill("{0}格式错误", this._name);
                }
            }
            catch
            {
                this.AddMessage("{0}验证失败", this._name);
            }

            return this;

        }

        /// <summary>
        /// 清除验证结果
        /// </summary>
        /// <returns><see cref="ValidationHelper"/>验证对象</returns>
        public ValidationHelper Clear()
        {
            this._lastResult = true;
            this._message = new List<string>();
            return this;
        }

        /// <summary>
        /// 获取是否通过验证
        /// </summary>
        /// <returns>通过返回true，否则返回false</returns>
        public bool Valid()
        {
            return this._message == null || this._message.Count == 0;
        }

        /// <summary>
        /// 获取验证消息
        /// </summary>
        /// <param name="separator">消息分隔符</param>
        /// <returns>验证消息</returns>
        public string Message(string separator = ",")
        {
            StringBuilder result = new StringBuilder();
            if (this.Valid())
            {
                return string.Empty;
            }

            for (var i = 0; i < this._message.Count;i++ )
            {
                if (i > 0)
                {
                    result.Append(separator);
                }

                result.Append(this._message[i]);
            }
                        
            return result.ToString();
        }

        /// <summary>
        /// 添加验证消息
        /// </summary>
        /// <param name="format">消息格式化字符串</param>
        /// <param name="paras">参数列表</param>
        private void AddMessage(string format, params object[] paras)
        {
            if (this._message == null)
            {
                this._message = new List<string>();
            }

            this._message.Add(string.Format(format, paras));
        }

        /// <summary>
        /// 是否需要验证
        /// </summary>
        /// <returns>需要验证返回true，否则返回false</returns>
        private bool NeedValidate()
        {
            return this._lastResult || this._iscontinue;
        }

        /// <summary>
        /// 验证成功
        /// </summary>
        private void ValidateSuccess()
        {
            this._lastResult = true;
        }

        /// <summary>
        /// 验证失败
        /// </summary>
        /// <param name="format">消息格式化字符串</param>
        /// <param name="paras">参数列表</param>
        private void ValidateFaill(string format, params object[] paras)
        {
            this._lastResult = false;
            this.AddMessage(format, paras);
        }

        #region 身份证验证

        /// <summary>
        /// 格式化15位身份证号码为18位
        /// </summary>
        /// <param name="idCard">身份证号</param>
        /// <returns></returns>
        private string FormatTo18IDCard(string idCard)
        {
            idCard = idCard.Trim();//去除前后空格
            if (idCard.Length == 18)
            {//本身18位
                return idCard;
            }
            if (idCard.Length != 15)
            {//非15位
                return "";
            }

            var keyCode = idCard.Substring(12, 3);
            string[] keyCodeArrr = {"996","997","998","999"};//如果身份证顺序码时996、997、998、999，这些是为百岁以上老人的特殊编码
            if (keyCodeArrr.Contains(keyCode))
            {//包含关键顺序码
                idCard = idCard.Substring(0, 6) + "19" + idCard.Substring(6, 9);
            }
            else
            {//不包含
                idCard = idCard.Substring(0, 6) + "18" + idCard.Substring(6, 9);
            }
            idCard = idCard + GetIDCardVerifyNum(idCard);//加上校证码

            return idCard;
        }

        /// <summary>
        /// 计算身份证校验码，根据国际标准gb 11643-1999
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        private string GetIDCardVerifyNum(string idCard)
        {
            var IDCardVerifyNum = "";

            if (idCard.Length != 17)
            {//非17位
                return IDCardVerifyNum;
            }

            int[] factorArr = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };//加权因子
            string[] verifyNumArr = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };//校验码对应值
            var checksNum = 0;
            for (int i = 0; i < idCard.Length; i++)
            {
                checksNum += Convert.ToInt32(idCard.Substring(i, 1)) * factorArr[i];
            }
            var mod = checksNum % 11;
            IDCardVerifyNum = verifyNumArr[mod];

            return IDCardVerifyNum;
        }

        #endregion
    }
}
