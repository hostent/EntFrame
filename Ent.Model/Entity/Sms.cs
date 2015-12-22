using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Entity
{

    /// <summary>
    /// 消息模型基类
    /// </summary>
    public class BaseMessageModel
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// 接收人手机号
        /// </summary>
        public string ReceiverMoblie { get; set; }

        /// <summary>
        /// 接收人邮箱
        /// </summary>
        public string ReceiverEmail { get; set; }

        /// <summary>
        /// 关联ID，业务表主键
        /// </summary>
        public long RelatedID { get; set; }
        /// <summary>
        ///发送批号
        /// </summary>
        public string BatchNumber { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string MessageTitle { get; set; }


        /// <summary>
        /// 消息业务类型
        /// </summary>
        public string BussinessType { get; set; }
    }

    /// <summary>
    /// 消息发送
    /// </summary>
    public class SendResult
    {
        /// <summary>
        /// 发送状态
        /// </summary>
        public SendState State
        {
            get;
            set;
        }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailReason
        {
            get;
            set;
        }

        /// <summary>
        /// 响应内容
        /// </summary>
        public string Respone
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 消息发送状态
    /// </summary>
    public enum SendState
    {
        /// <summary>
        /// 未发送
        /// </summary>
        UnSend = 0,

        /// <summary>
        /// 发送中
        /// </summary>
        Sending = 1,

        /// <summary>
        /// 发送成功
        /// </summary>
        SendSuccess = 2,

        /// <summary>
        /// 发送失败
        /// </summary>
        SendFail = 3
    }

    public enum MessageType
    {
        SMS = 1,
        Email = 2
    }


    #region 各种模板放这里

    public class SmsVerify : BaseMessageModel
    {
        [Description("验证码")]
        public string Code { get; set; }

    }




    #endregion


}
