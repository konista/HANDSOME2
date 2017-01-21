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
        [Serializable]
        public struct Var
        {
            public string name { get; set; }
            public string type { get; set; }
            public string max { get; set; }
            public string min { get; set; }
            public string value { get; set; }
            public bool isanswer { get; set; }
        }
        [Serializable]
        public struct Answer
        {
            public string name { get; set; }
            public string value { get; set; }
            public string user_value { get; set; }
            public string expression { get; set; }
        }
        [Serializable]
        public struct Subject
        {
            public string name { get; set; }
            public string template { get; set; }
            public string text { get; set; }
            public List<Var> vars { get; set; }
            public List<Answer> answers { get; set; }
            public int empty_num { get; set; }
            public List<List<string>> conditions { get; set; }
            public string assert { get; set; }
            public int timeout { get; set; }
            public string content { get; set; }
            public bool correct { get; set; }
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
        public static string MakeInputHTML(string valuetype, string type, string id, string value = "", string style = "font-family: 宋体, Arial, Helvetica, sans-serif; width: 40px; text-align: center; font-size: large; font-weight: bold;", string maxlength = "")
        {
            string ret = "<input ";
            switch (valuetype)
            {
                case "int":
                    {
                        ret += "type=\"" + type + "\" ";
                        ret += "id=\"" + id + "\" ";
                        ret += "style=\"" + style + "\" ";
                        ret += "value=\"" + value + "\" ";
                        ret += "onkeyup=\"this.value=this.value.replace(/\\D/g,'')\" ";
                        ret += "onchange=\"this.value=this.value.replace(/\\D/g,'')\" ";
                        ret += "maxlength=\"" + maxlength + "\" ";
                        break;
                    }
                default:
                    { break; }
            }
            ret += "/>";
            return ret;
        }
    }
}
