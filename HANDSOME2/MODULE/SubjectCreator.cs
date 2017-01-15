using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HANDSOME2.MODULE.UserDefine;
namespace HANDSOME2.MODULE
{
    class SubjectCreator
    {
        public Dictionary<string, Subject> ds;
        public SubjectCreator(string type)
        {
            ModLoader ml = new ModLoader(type);
            ds = ml.ds;
        }
        public string CreateSubject(string sbj_name)
        {
            Subject s = UserDefine.DeepClone<Subject>(ds[sbj_name]);
            string ret = s.template;
            s = DefineAnswer(s);
            Dictionary<string, object> vars = MakeVars(s);
            if (Assert(vars, s.conditions))
            {
                ret = RepaceVar(vars, ret);
            }
            else
            { return CreateSubject(sbj_name); }
            return ret;
        }
        private Subject DefineAnswer(Subject s)
        {
            int empty_num = s.empty_num;
            while (s.answer.Count > empty_num)
            {
                int delete_index = RandomInt("0", s.answer.Count.ToString());
                foreach (Var var in s.vars)
                {
                    if (var.name == s.answer[delete_index])
                    {
                        s.vars.Remove(var);
                        break;
                    }
                }
                s.answer.RemoveAt(delete_index);
            }
            return s;
        }
        private Dictionary<string,object> MakeVars(Subject s)
        {
            Dictionary<string, object> vars = new Dictionary<string, object>();
            foreach (Var var in s.vars)
            {
                switch (var.type)
                {
                    case "int":
                        {
                            vars.Add(var.name, RandomInt(var.min, var.max));
                            break;
                        }
                    default:
                        { break; }
                }
            }
            return vars;
        }
        private bool Assert(Dictionary<string,object> vars, List<List<string>> conditions)
        {
            foreach (List<string> condition in conditions)
            {
                bool ret = true;
                foreach (string con in condition)
                {
                    string con_str = RepaceVar(vars, con);
                    ret = ret && (bool)JScript.JScriptRun("condition_assert", new object[] { con_str });
                    if (!ret) { break; }
                }
                if (ret) { return true; }
            }
            return false;
        }
        private int RandomInt(string min, string max)
        {
            int imin = int.Parse(min);
            int imax = int.Parse(max);
            Random r = new Random();
            return r.Next(imin, imax);
        }
        private string RepaceVar(Dictionary<string, object> vars, string exp)
        {
            string s = exp;
            foreach (KeyValuePair<string, object> kv in vars)
            {
                string var_str = "${" + kv.Key + "}";
                if (s.Contains(var_str))
                {
                    s = s.Replace(var_str, kv.Value.ToString());
                }
            }
            return s;
        }
    }
}
