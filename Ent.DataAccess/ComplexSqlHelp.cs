using Ent.Common.Thread;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ent.DataAccess
{
    public class ComplexSqlHelp
    {


        public static List<T> GetReportData<T>(BaseDBContext dbContext, string reportName, int pageSize, int pageIndex, string order, IDictionary<string, object> where, bool isPage, out int totalCount)
        {

            try
            {
                XElement root = XmlConfigManager.GetRepsConfig();
                XElement rep = root.Elements("Rep").Where(q => q.Attribute("key").Value == reportName).FirstOrDefault();
                if (rep == null)
                {
                    throw new Exception("xml没有相关配置");
                }

                string reportsql = rep.Element("ReportSql") == null ? "" : rep.Element("ReportSql").Value;
                string countSql = rep.Element("TotalSql") == null ? "" : rep.Element("TotalSql").Value;

                IDictionary<string, object> dataRes = new Dictionary<string, object>();
                Dictionary<string, object> data = new Dictionary<string, object>();
                foreach (var itemEle in rep.Elements("Dynamic"))
                {
                    List<ReportWhereEntity> list = FillReportWhereEntity(itemEle.ToString());
                    string dynamicStr = GetDynamicStr(list, where, ref dataRes);
                    string dyProperty = itemEle.Attribute("property").Value;
                    reportsql = reportsql.Replace(dyProperty, dynamicStr);
                    if (isPage)
                    {
                        countSql = countSql.Replace(dyProperty, dynamicStr);
                    }
                    if (data != null)
                    {
                        foreach (var item in dataRes)
                        {
                            if (data.Keys.Contains(item.Key))
                            {
                                continue;
                            }
                            data.Add(item.Key, item.Value);
                        }
                    }
                }
                if (isPage)
                {
                    if (string.IsNullOrEmpty(order))
                    {
                        throw new Exception("order must not be null!");
                    }

                    int start = (pageIndex - 1) * pageSize;
                    reportsql = reportsql + @"  order by {0} limit {1},{2}";

                    reportsql = string.Format(reportsql, order, start, pageSize);
                }

                if (where != null)
                {
                    foreach (var item in where)
                    {
                        if (!data.Keys.Contains(item.Key.ToLower()))
                        {
                            data.Add(item.Key, item.Value);
                        }
                    }
                }

                List<T> result = new List<T>();

                List<SqlParameter> parList = new List<SqlParameter>();
                foreach (var item in data)
                {
                    parList.Add(new SqlParameter(item.Key, item.Value));

                }

                result = dbContext.Database.SqlQuery<T>(reportsql, parList.ToArray()).ToList();


                if (isPage)
                {
                    List<SqlParameter> parCountList = new List<SqlParameter>();
                    foreach (var item in data)
                    {
                        parCountList.Add(new SqlParameter(item.Key, item.Value));

                    }
                    totalCount = dbContext.Database.SqlQuery<int>(countSql, parList.ToArray()).FirstOrDefault();
                }
                else
                {
                    totalCount = 0;
                }

                //if (!ThreadContext.IsInTran)
                //{
                //    dbContext.Dispose();
                //}
                return result;
            }
            catch (Exception e)
            {
                totalCount = 0;
                return null;
            }

        }

        public static int ExecReport(BaseDBContext dbContext, string reportName, IDictionary<string, object> where)
        {
            XElement root = XmlConfigManager.GetRepsConfig();
            XElement rep = root.Elements("Rep").Where(q => q.Attribute("key").Value == reportName).FirstOrDefault();
            if (rep == null)
            {
                throw new Exception("xml没有相关配置");
            }

            string reportsql = rep.Element("ReportSql") == null ? "" : rep.Element("ReportSql").Value;

            IDictionary<string, object> dataRes = new Dictionary<string, object>();
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (var itemEle in rep.Elements("Dynamic"))
            {
                List<ReportWhereEntity> list = FillReportWhereEntity(itemEle.ToString());
                string dynamicStr = GetDynamicStr(list, where, ref dataRes);
                string dyProperty = itemEle.Attribute("property").Value;
                reportsql = reportsql.Replace(dyProperty, dynamicStr);

                if (data != null)
                {
                    foreach (var item in dataRes)
                    {
                        if (data.Keys.Contains(item.Key))
                        {
                            continue;
                        }
                        data.Add(item.Key, item.Value);
                    }
                }
            }

            if (where != null)
            {
                foreach (var item in where)
                {
                    if (!data.Keys.Contains(item.Key.ToLower()))
                    {
                        data.Add(item.Key, item.Value);
                    }
                }
            }

            int result = 0;

            List<SqlParameter> parList = new List<SqlParameter>();
            foreach (var item in data)
            {
                parList.Add(new SqlParameter(item.Key, item.Value));

            }
            result = dbContext.Database.ExecuteSqlCommand(reportsql, parList.ToArray());

            //if (!ThreadContext.IsInTran)
            //{
            //    dbContext.Dispose();
            //}
            return result;
        }

        private static List<ReportWhereEntity> FillReportWhereEntity(string xmlStr)
        {
            XElement root = XElement.Parse(xmlStr);
            List<ReportWhereEntity> list = new List<ReportWhereEntity>();
            foreach (var dyItem in root.Elements("c"))
            {
                ReportWhereEntity result = new ReportWhereEntity();
                result.Data = dyItem.Value;
                result.Prepend = dyItem.Attribute("prepend").Value;
                result.Type = dyItem.Attribute("type").Value;
                result.Property = dyItem.Attribute("property").Value;
                list.Add(result);
            }
            return list;
        }

        private static string GetDynamicStr(List<ReportWhereEntity> list, IDictionary<string, object> condition, ref IDictionary<string, object> tag)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(" (1=1) ");
            tag = new Dictionary<string, object>();
            Dictionary<string, object> conditionDict = new Dictionary<string, object>();
            foreach (var item in condition)
            {
                conditionDict.Add(item.Key.ToLower(), item.Value);
            }
            bool isSet = false;
            foreach (var item in list)
            {
                item.Property = item.Property.ToLower();
                item.Data = item.Data.ToLower();
                if (!conditionDict.Keys.Contains(item.Property))
                {
                    continue;
                }
                if (tag.Keys.Contains(item.Property))
                {
                    continue;
                }
                if (conditionDict[item.Property] == null || (conditionDict[item.Property] is string && string.IsNullOrEmpty(conditionDict[item.Property].ToString())))
                {
                    continue;
                }
                if (item.Type.Trim() == string.Empty)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(item.Prepend);
                    }
                    if (item.Prepend.Trim() == ",")
                    {
                        isSet = true;
                        sb.Append(item.Data);
                    }
                    else
                    {
                        sb.Append(" ( ").Append(item.Data).Append(" ) ");
                    }
                    tag.Add(item.Property, conditionDict[item.Property]);
                }
                else if (item.Type.Trim().ToLower() == "in")
                {
                    var value = conditionDict[item.Property].ToString().Trim(',');

                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }
                    string str = " (";
                    for (int i = 0; i < value.Split(',').Count(); i++)
                    {
                        string keyStr = item.Property + "__" + i.ToString();
                        str = str + "@" + keyStr + ",";
                        tag.Add(keyStr, value.Split(',')[i]);
                    }
                    str = str.Trim(',') + ") ";
                    if (sb.Length != 0)
                    {
                        sb.Append(item.Prepend);
                    }
                    sb.Append(" ( ").Append(item.Data.Replace("@" + item.Property, str)).Append(" ) ");
                }
                else if (item.Type.Trim().ToLower() == "like")
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(item.Prepend);
                    }
                    sb.Append(" ( ").Append(item.Data).Append(" ) ");
                    tag.Add(item.Property, "%" + conditionDict[item.Property] + "%");
                }
                else if (item.Type.Trim().ToLower() == "leftlike")
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(item.Prepend);
                    }
                    sb.Append(" ( ").Append(item.Data).Append(" ) ");
                    tag.Add(item.Property, conditionDict[item.Property] + "%");
                }
            }
            if (!isSet && sb.Length == 0)
            {
                sb.Append("(1=1)");
            }
            string result = sb.ToString();
            return result;
        }

        #region 内部类

        class ReportWhereEntity
        {
            public string Property { get; set; }
            public string Prepend { get; set; }
            public string Type { get; set; }
            public string Data { get; set; }
        }

        #endregion
    }
}
