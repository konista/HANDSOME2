using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HANDSOME2.MODULE
{
    class ModLoader
    {
        public Dictionary<string, UserDefine.Subject> ds = new Dictionary<string, UserDefine.Subject>();
        public ModLoader(string type)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                XmlReader reader = XmlReader.Create(System.AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\MOD\" + type + ".xml", settings);
                XmlDocument xd = new XmlDocument();
                xd.Load(reader);
                reader.Close();
                Parse(xd);
                reader.Close();
            }
            catch
            { }
            finally
            { }
        }

        private void Parse(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("SUBJECTS");
            XmlNodeList xns = root.ChildNodes;
            foreach (XmlNode xn in xns)
            {
                if (xn.Name != "SUBJECT") { continue; }
                UserDefine.Subject s = ParseSubject(xn);
                ds.Add(s.name, s);
            }
        }

        private UserDefine.Subject ParseSubject(XmlNode subject)
        {
            UserDefine.Subject s = new UserDefine.Subject();
            s.name = GetSubjectName(subject);
            XmlNodeList xnl = subject.ChildNodes;
            foreach (XmlNode xn in xnl)
            {
                switch (xn.Name)
                {
                    case "TEMPLATE":
                        {
                            s.template = GetTemplate(xn);
                            break;
                        }
                    case "VARS":
                        {
                            s.vars = GetVars(xn);
                            break;
                        }
                    case "ANSWER":
                        {
                            s.answers = GetAnswer(xn);
                            s.empty_num = GetEmptyNum(xn);
                            break;
                        }
                    case "CONDITIONS":
                        {
                            s.conditions = GetConditions(xn);
                            break;
                        }
                    case "ASSERT":
                        {
                            s.assert = GetAssert(xn);
                            break;
                        }
                    case "TIMEOUT":
                        {
                            s.timeout = GetTimeout(xn);
                            break;
                        }
                    default:
                        { break; }
                }
            }
            return s;
        }

        private string GetSubjectName(XmlNode subject)
        {
            return GetAttribute(subject, "name");
        }

        private string GetTemplate(XmlNode template)
        {
            return template.InnerText.ToUpper();
        }

        private List<UserDefine.Var> GetVars(XmlNode vars)
        {
            List<UserDefine.Var> lv = new List<UserDefine.Var>();
            XmlNodeList xnl = vars.ChildNodes;
            foreach (XmlNode xn in xnl)
            {
                if (xn.Name != "VAR") { continue; }
                UserDefine.Var var = new UserDefine.Var();
                var.type = GetAttribute(xn, "type");
                switch (var.type)
                {
                    case "int":
                        {
                            var.name = xn.InnerText.ToUpper();
                            var.max = GetAttribute(xn, "max");
                            var.min = GetAttribute(xn, "min");
                            var.value = GetAttribute(xn, "value");
                            string isanswer = GetAttribute(xn, "isanswer");
                            var.isanswer = isanswer == "" ? false : bool.Parse(isanswer);
                            break;
                        }
                    default:
                        { break; }
                }
                lv.Add(var);
            }
            return lv;
        }

        private List<string> GetAnswer(XmlNode answer)
        {
            return answer.InnerText.ToUpper().Split(new char[1] { '|' }).ToList<string>();
        }

        private int GetEmptyNum(XmlNode answer)
        {
            return int.Parse(GetAttribute(answer, "empty_num"));
        }

        private List<List<string>> GetConditions(XmlNode conditions)
        {
            List<List<string>> ls = new List<List<string>>();
            XmlNodeList xnl = conditions.ChildNodes;
            foreach (XmlNode xn in xnl)
            {
                if (xn.Name != "CONDITION") { continue; }
                ls.Add(xn.InnerText.ToUpper().Split(new char[1] { ',' }).ToList<string>());
            }
            return ls;
        }

        private string GetAssert(XmlNode assert)
        {
            return assert.InnerText.ToUpper();
        }

        private int GetTimeout(XmlNode timeout)
        {
            return int.Parse(timeout.InnerText);
        }

        private string GetAttribute(XmlNode xn, string attr)
        {
            return ((XmlElement)xn).GetAttribute(attr);
        }
    }
}