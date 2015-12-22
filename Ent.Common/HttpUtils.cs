//------------------------------------------------------------------------------------------
// <copyright file="HttpUtils.cs" company="富银金融信息服务有限公司">
//     Copyright (c) 富银金融信息服务有限公司. All rights reserved.
// </copyright>
// <author>张伟</author>
//------------------------------------------------------------------------------------------
namespace Ent.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// Http辅助类
    /// </summary>
    public class HttpUtils
    {
        /// <summary>
        /// 以Get方式获取数据https
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="queryData">查询参数</param>
        /// <param name="encoding">encoding</param>
        /// <returns>返回结果</returns>
        public static string Gets(string url, Dictionary<string, string> queryData, Encoding encoding)
        {
            try
            {
                if (queryData != null && queryData.Count > 0)
                {
                    //向请求添加查询字符串               
                    url += "?";
                    url += DictToQueryString(queryData);
                }

                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = System.Net.CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据  
                string pageHtml = encoding.GetString(pageData);

                return pageHtml;
            }

            catch (System.Net.WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
                return null;
            }
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        /// <summary>
        /// 以Get方式获取数据
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="queryData">查询参数</param>
        /// <param name="encoding">encoding</param>
        /// <returns>返回结果</returns>
        public static string Get(string url, Dictionary<string, string> queryData, Encoding encoding)
        {
            try
            {
                if (queryData != null && queryData.Count > 0)
                {
                    //向请求添加查询字符串               
                    url += "?";
                    url += DictToQueryString(queryData);
                }

                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = System.Net.CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  
                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据  
                string pageHtml = encoding.GetString(pageData);

                return pageHtml;
            }

            catch (System.Net.WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// 以Get方式获取数据
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="queryData">查询参数</param>
        /// <returns>返回结果</returns>
        public static string Get(string url, Dictionary<string, string> queryData, int _type = 0)
        {
            try
            {
                if (queryData != null && queryData.Count > 0)
                {
                    //向请求添加查询字符串               
                    url += "?";
                    url += DictToQueryString(queryData);
                }

                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = System.Net.CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  
                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据  
                string pageHtml = System.Text.Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句              
                if (_type == 1)
                {
                    pageHtml = System.Text.Encoding.UTF8.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句    
                }
                return pageHtml;
            }

            catch (System.Net.WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// 以Post方式提交数据
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="queryData">查询参数</param>
        /// <param name="formData">表单参数</param>
        /// <returns>返回结果</returns>
        public static string Post(string url, IDictionary<string, string> queryData, IDictionary<string, string> formData)
        {
            try
            {
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                if (queryData != null && queryData.Count > 0)
                {
                    //向请求添加查询字符串               
                    url += "?";
                    url += DictToQueryString(queryData);
                }

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                //向请求添加表单数据
                string requestForm = DictToQueryString(formData);
                byte[] postdatabyte = Encoding.UTF8.GetBytes(requestForm);
                request.ContentLength = postdatabyte.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postdatabyte, 0, postdatabyte.Length); //设置请求主体的内容
                stream.Close();

                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader responseReader = new StreamReader(responseStream);
                return responseReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Log.LogHelp.Default.Error(e);
                return "";
            }
        }
        public static string Post(string url, string json)
        {
            try
            {
                HttpWebRequest request = null;
                HttpWebResponse response = null;

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                //向请求添加表单数据
                byte[] postdatabyte = Encoding.UTF8.GetBytes(json);
                request.ContentLength = postdatabyte.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postdatabyte, 0, postdatabyte.Length); //设置请求主体的内容
                stream.Close();

                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader responseReader = new StreamReader(responseStream);
                return responseReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Log.LogHelp.Default.Error(e);
                return "";
            }
        }

        /// <summary>
        /// 字典对象转为查询字符串
        /// </summary>
        /// <param name="dict">字典对象</param>
        /// <returns>查询字符串</returns>
        public static string DictToQueryString(IDictionary<string, string> dict)
        {
            if (dict == null || dict.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder queryString = new StringBuilder();
            foreach (KeyValuePair<string, string> item in dict)
            {
                queryString.AppendFormat("{0}={1}&", item.Key, HtmlUtils.UrlEncode(item.Value));
            }

            var queryStr = queryString.ToString();
            if (!string.IsNullOrEmpty(queryStr))
            {//不为空
                queryStr = queryStr.Substring(0, queryStr.Length - 1);
            }

            return queryStr;
        }

        /// <summary>
        /// 从请求中获取客户端提交的参数集合
        /// </summary>
        /// <param name="req">http请求对象</param>
        /// <returns>提交的参数集合</returns>
        public static Dictionary<string, object> GetRequest(HttpRequestBase req)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                foreach (string item in req.Form)
                {
                    try
                    {
                        dict[item] = req.Form[item];
                    }
                    catch
                    {
                        dict[item] = req.Unvalidated.Form[item];
                    }

                    if (dict[item].GetType() == typeof(string))
                    {
                        dict[item] = dict[item].ToString().Trim();
                    }
                }
                foreach (string item in req.QueryString)
                {
                    dict[item] = req.QueryString[item];
                    if (dict[item].GetType() == typeof(string))
                    {
                        dict[item] = dict[item].ToString().Trim();
                    }
                }

            }
            catch (Exception e)
            {

            }

            return dict;
        }


    }
}
