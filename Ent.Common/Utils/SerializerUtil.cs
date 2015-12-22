using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ent.Common.Utils
{
    /// <summary>
    /// 序列化辅助类
    /// </summary>
    public class SerializerUtil
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static T DeserializeXML<T>(string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml.Trim()))
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(T));
                    return (T)xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return default(T);
            }
        }
    }
}
