using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common
{
   public  class ReplaceUrl
    {
      
       /// <summary>
       /// 修改图片地址
       /// </summary>
       /// <param name="url">原有upload图片地址</param>
       /// <returns></returns>
       public static string ReplaceImgUrl(string url) {

          if (!string.IsNullOrEmpty(url)&& url.ToLower().Contains("/upload/")) 
          {
               //过滤，只有非空，且图片地址包含有/upload/才可以修改
              if (!url.Contains("Cms"))
              {
                  url = RootUrlConfig.ResourceUrl + url.Replace("upload", "upload/Cms");
              }
              else {
                  url = RootUrlConfig.ResourceUrl +url;
              }
             
           }

           return url;
       }

       /// <summary>
       /// 修改内容中的图片地址
       /// </summary>
       /// <param name="Content">内容</param>
       /// <returns></returns>
       public static string ReplaceContentImgUrl(string Content)
       {
           if (Content.Contains("src=")||Content.Contains("Src=")) {//确定是否为图片

               if (Content.ToLower().Contains("/upload/")) {//确定图片地址有/upload/
                   if (Content.ToLower().Contains("/upload/cms"))
                   { Content = Content.Replace("/upload", RootUrlConfig.ResourceUrl + "/upload");
                   }
                   else {
                       Content = Content.Replace("/upload", RootUrlConfig.ResourceUrl + "/upload/Cms");
                   }
                 
               }
           }
           return Content;
       }
    }
}
