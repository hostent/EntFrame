using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common.ApiLog
{
    public class ApiLogModel
    {
        public string ApiName { get; set; }
        public string RequestUrl { get; set; }
        public Dictionary<string, string> RequestParameter { get; set; }
        public Dictionary<string, string> RequestFormData { get; set; }
        public string ResponseData { get; set; }
    }
}
