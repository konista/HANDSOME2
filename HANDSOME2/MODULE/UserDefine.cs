using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HANDSOME2.MODULE
{
    class UserDefine
    {
        public struct Var
        {
            public string name { get; set; }
            public string type { get; set; }
            public string max { get; set; }
            public string min { get; set; }
        }
        public struct Subject
        {
            public string name { get; set; }
            public string template { get; set; }
            public List<Var> vars { get; set; }
            public List<string> answer { get; set; }
            public int empty_num { get; set; }
            public List<List<string>> conditions { get; set; }
            public string assert { get; set; }
            public int timeout { get; set; }
        }
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
