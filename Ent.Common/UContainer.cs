using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common
{
    public class UContainer
    {
        /// <summary>
        /// 防止加载进来的时候，有概率性的空
        /// </summary>
        static IUnityContainer m_Container = null;
        static IUnityContainer _Container
        {
            get
            {
                if (m_Container == null)
                {
                    m_Container = new UnityContainer();
                }
                return m_Container;
            }
        }

        /// <summary>
        /// 初始化容器 返回对应对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>()
        {
            if (!_Container.IsRegistered<T>())
            {
                string[] className = typeof(T).FullName.Split('.');

                string dllName = "Ent.Service" + '.' + className[3];

                Assembly cAss = Assembly.Load(dllName);
                string interfaceTypeName = typeof(T).FullName.Replace("Ent.Model.Service." + className[3], "").Replace(".I", ".C");
                if (interfaceTypeName.StartsWith("."))
                {
                    interfaceTypeName = interfaceTypeName.Substring(1, interfaceTypeName.Length - 1);
                }

                string impTypeName = dllName + "." + interfaceTypeName;
                T obj = (T)cAss.CreateInstance(impTypeName);
                _Container.RegisterInstance<T>(obj);
            }
            T result = _Container.Resolve<T>();


            return result;
        }


        static IDictionary<string, ObjCache> objCache = new Dictionary<string, ObjCache>();



        private static object GetImpObjFromInterface(string interfaceFullName)
        {
            Assembly cAss = Assembly.Load("Ent.Service");
            string interfaceTypeName = interfaceFullName.Replace("Ent.Model.Service", "").Replace(".I", ".C");

            string impTypeName = "Ent.Service" + interfaceTypeName;
            return cAss.CreateInstance(impTypeName);
        }


        public static object Call(string methodPath, IDictionary<string, object> args)
        {
            BuildObjCache(methodPath);

            var tag = (ObjCache)objCache[methodPath.ToLower()];
            var argTypes = tag.ImpMethodInfo.GetParameters();

            Type fanType = null;
            //该方法是泛型
            if (tag.ImpMethodInfo.IsGenericMethod)
            {
                fanType = Type.GetType("Ent.Model." + args["T"] + ",Ent.Model");
            }

            List<object> argList = new List<object>();

            foreach (var item in argTypes)
            {
                var dictItem = args.AsQueryable().Where(q => q.Key.ToLower() == item.Name.ToLower());

                if (dictItem.Count() == 0)
                {
                    argList.Add(item.DefaultValue);
                }
                else
                {
                    string itemJson = Newtonsoft.Json.JsonConvert.SerializeObject(dictItem.FirstOrDefault().Value);

                    if (tag.ImpMethodInfo.IsGenericMethod && item.ParameterType.Name == "T")
                    {
                        argList.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(itemJson, fanType));
                    }
                    else
                    {
                        argList.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(itemJson, item.ParameterType));
                    }
                }
            }
            if (tag.ImpMethodInfo.IsGenericMethod)
            {
                return tag.ImpMethodInfo.MakeGenericMethod(fanType).Invoke(tag.TagsObj, argList.ToArray());
            }
            return tag.ImpMethodInfo.Invoke(tag.TagsObj, argList.ToArray());

        }

        private static void BuildObjCache(string methodPath)
        {
            if (!objCache.Keys.Contains(methodPath))
            {
                Type[] types = Assembly.Load("Ent.Model").GetTypes();
                foreach (Type t in types)
                {
                    if (t.FullName.IndexOf("Service") > 0 && t.IsInterface)
                    {
                        MethodInfo[] ms = t.GetMethods();
                        foreach (var memberInfo in ms)
                        {
                            Attribute att = memberInfo.GetCustomAttributes(typeof(ApiAttribute)).FirstOrDefault();
                            if (att != null)
                            {
                                string apiCode = t.FullName.Replace("Ent.Model.", "");
                                string methodName = memberInfo.Name;

                                int index = apiCode.IndexOf(".I");
                                apiCode = apiCode.Substring(index + 2, apiCode.Length - index - 2).ToLower() + "." + methodName.ToLower();

                                if (methodPath.ToLower() == apiCode)
                                {
                                    object impObj = GetImpObjFromInterface(t.FullName);
                                    MethodInfo method = impObj.GetType().GetMethod(methodName);
                                    objCache[apiCode] = new ObjCache() { ImpMethodInfo = method, TagsObj = impObj, InterfaceMethodInfo = memberInfo, Attribute = (ApiAttribute)att };
                                    break;
                                }
                            }
                        }
                    }
                    if (objCache.Keys.Contains(methodPath.ToLower()))
                    {
                        break;
                    }
                }
            }

            if (!objCache.Keys.Contains(methodPath.ToLower()))
            {
                throw new Exception("api is not exist");
            }
        }

        public static ApiAttribute GetApiAttribute(string methodPath)
        {
            BuildObjCache(methodPath);
            var tag = (ObjCache)objCache[methodPath.ToLower()];

            return tag.Attribute; ;
        }


        private class ObjCache
        {
            public ApiAttribute Attribute { get; set; }

            public MethodInfo InterfaceMethodInfo { get; set; }

            public MethodInfo ImpMethodInfo { get; set; }

            public object TagsObj { get; set; }

        }


    }


}
