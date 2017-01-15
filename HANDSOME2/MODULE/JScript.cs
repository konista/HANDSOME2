using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HANDSOME2.MODULE
{
    class JScript
    {
        private static readonly CodeDomProvider _provider = new Microsoft.JScript.JScriptCodeProvider();
        private static Type _evaluateType;
        private const string scriptStr = @"package j2cs  
        {  
            public class MyJs  
            {  
　　            public static function condition_assert(exp)  
　　            {   
                    return eval(exp);  
　　            }  
            }  
        }";
        public static object JScriptRun(string jsMethodName, object[] input_params)
        {
            //编译的参数  
            CompilerParameters parameters = new CompilerParameters();  
            parameters.GenerateInMemory = true;  
            CompilerResults results = _provider.CompileAssemblyFromSource(parameters, scriptStr);  
            Assembly assembly = results.CompiledAssembly;  
  
            //动态编译脚本中的内容  
            _evaluateType = assembly.GetType("j2cs.MyJs"); 
  
            //执行指定的方法并传参数
            object retObj = _evaluateType.InvokeMember(jsMethodName, BindingFlags.InvokeMethod, null, null, input_params); 
            return retObj;  
        }
    }
}
