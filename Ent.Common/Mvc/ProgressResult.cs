using Ent.Common.Cache;
using Ent.Common.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ent.Common.Mvc
{
    public class ProgressResult : JsonResult
    {

        string JobName { get; set; }


        Job.FunHandle Handle { get; set; }


        public bool RunningState
        {
            get
            {
                string key = "Thread.Asyn.Jobs." + JobName + ".RunningState";
                var cache = CacheHelp.Default.Get(key);
                if (cache == null)
                {
                    return false;
                }
                return (bool)CacheHelp.Default.Get(key); ;
            }
            set
            {
                string key = "Thread.Asyn.Jobs." + JobName + ".RunningState";
                CacheHelp.Default.Add(key, value);
            }
        }


        decimal Percentage
        {
            get
            {
                string key = "Thread.Asyn.Jobs." + JobName + ".Percentage";
                var cache = CacheHelp.Default.Get(key);
                if (cache == null)
                {
                    return 0;
                }
                return (decimal)cache;
            }
        }

        string Message
        {
            get
            {
                string key = "Thread.Asyn.Jobs." + JobName + ".Message";
                var cache = CacheHelp.Default.Get(key);
                if (cache == null)
                {
                    return "";
                }
                return (string)cache;
            }
        }

        public ProgressResult(Job.FunHandle handle, string jobName)
        {
            JobName = jobName;
            Handle = handle;
        }

        public override void ExecuteResult(ControllerContext context)
        {

            if (Handle != null && RunningState == false)
            {
                IDictionary<string, object> dict = new Dictionary<string, object>();
                context.HttpContext.Request.Form.CopyTo(dict);
                Job.Run(Handle, dict, JobName);
                context.HttpContext.Response.Write(1);
            }
            else
            {
                var result = new { Percentage = Percentage, Message = Message };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                context.HttpContext.Response.Write(json);
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Flush();


        }

    }
}
