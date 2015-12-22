using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser;

namespace Ent.Common
{
    public class RazorTemplate
    {
        static Type Compile<T>(string templateContent)
        {
            //准备临时类名，读取模板文件和Razor代码生成器
            var class_name = "c" + Guid.NewGuid().ToString("N");
            var base_type = typeof(TemplateBase<>).MakeGenericType(typeof(T));
            //var template = File.ReadAllText(template_path);

            var host = new RazorEngineHost(new CSharpRazorCodeLanguage(), () => new HtmlMarkupParser())
            {

                DefaultBaseClass = base_type.FullName,
                DefaultClassName = class_name,
                DefaultNamespace = "Ent.Common.dynamic",
                GeneratedClassContext =
                    new GeneratedClassContext("Execute", "Write", "WriteLiteral", "WriteTo",
                                              "WriteLiteralTo",
                                              "YourNameSpace.TemplateBase")

            };
            host.NamespaceImports.Add("System");
            host.NamespaceImports.Add("Ent.Common");

            //生成代码
            CodeCompileUnit code;
            using (var reader = new StringReader(templateContent))
            {
                var generatedCode = new RazorTemplateEngine(host).GenerateCode(reader);
                code = generatedCode.GeneratedCode;
            }
            //准备编译参数
            var @params = new CompilerParameters
            {
                IncludeDebugInformation = false,
                TempFiles = new TempFileCollection(AppDomain.CurrentDomain.DynamicDirectory),
                CompilerOptions = "/target:library /optimize",
                GenerateInMemory = false
            };

            var assemblies = AppDomain.CurrentDomain
               .GetAssemblies()
               .Where(a => !a.IsDynamic)
               .Select(a => a.Location)
               .ToArray();
            @params.ReferencedAssemblies.AddRange(assemblies);

            //编译
            var provider = new CSharpCodeProvider();
            var compiled = provider.CompileAssemblyFromDom(@params, code);

            if (compiled.Errors.Count > 0)
            {
                var compileErrors = string.Join("\r\n", compiled.Errors.Cast<object>().Select(o => o.ToString()));
                throw new ApplicationException("Failed to compile Razor:" + compileErrors);
            }

            //编译成功后， 返回编译后的动态Type
            return compiled.CompiledAssembly.GetType("Ent.Common.dynamic." + class_name);


        }

        public static IDictionary<string, dynamic> TemplateInstance
        {
            get
            {
                string key = "RazorTemplate" + ".TemplateInstance";
                var cache = Cache.CacheHelp.Default.Get(key);
                if (cache == null)
                {
                    Cache.CacheHelp.Default.Add(key, new Dictionary<string, dynamic>());
                }
                return (IDictionary<string, dynamic>)Cache.CacheHelp.Default.Get(key);
            }
        }

        public static string Render<T>(T model, string templateContent)
        {
            if (string.IsNullOrEmpty(templateContent))
            {
                return string.Empty;
            }

            Type type = null;

            string key = typeof(T).FullName + "." + templateContent.GetHashCode();
            if (TemplateInstance.ContainsKey(key))
            {
                type = (Type)(TemplateInstance[key]);
            }
            else
            {
                type = Compile<T>(templateContent);
                //创建视图实例
               TemplateInstance[key]=type;
            }

            TemplateBase<T> instance = (TemplateBase<T>)Activator.CreateInstance(type);

            //执行模板（把数据嵌入文件）
            instance.Model = model;
            instance.Execute();
            //输出最终结果
            var result = instance.Result;

            return result;
        }


        public class TemplateBase
        {
            public string Layout { get; set; }
            public UrlHelper Url { get; set; }
            public Func<string> RenderBody { get; set; }
            public string Path { get; internal set; }
            public string Result { get { return Writer.ToString(); } }

            protected TemplateBase()
            {
            }

            public TextWriter Writer
            {
                get
                {
                    if (writer == null)
                    {
                        writer = new StringWriter();
                    }
                    return writer;
                }
                set
                {
                    writer = value;
                }
            }

            private TextWriter writer;

            public void Clear()
            {
                Writer.Flush();
            }

            public virtual void Execute() { }

            public void Write(object @object)
            {
                if (@object == null)
                {
                    return;
                }
                Writer.Write(@object);
            }

            public void WriteLiteral(string @string)
            {
                if (@string == null)
                {
                    return;
                }
                Writer.Write(@string);
            }

            public static void WriteLiteralTo(TextWriter writer, string literal)
            {
                if (literal == null)
                {
                    return;
                }
                writer.Write(literal);
            }

            public static void WriteTo(TextWriter writer, object obj)
            {
                if (obj == null)
                {
                    return;
                }
                writer.Write(obj);
            }
        }

        public class TemplateBase<T> : TemplateBase
        {
            public T Model { get; set; }
        }
    }
}
