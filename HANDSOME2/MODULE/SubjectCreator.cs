using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace HANDSOME2.MODULE
{
    class SubjectCreator
    {
        private static RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
        private static byte[] rb = new byte[4];
        public Dictionary<string, UserDefine.Subject> ds;
        public SubjectCreator(string type)
        {
            ModLoader ml = new ModLoader(type);
            ds = ml.ds;
        }
        public UserDefine.Subject CreateSubject(string sbj_name)
        {
            UserDefine.Subject s = UserDefine.DeepClone<UserDefine.Subject>(ds[sbj_name]);
            DefineEmpty(s);
            MakeVars(s);
            if (AssertCondition(s))
            {
                s = MakePuzzle(s);
            }
            else
            { return CreateSubject(sbj_name); }
            return s;
        }
        private void DefineEmpty(UserDefine.Subject s)
        {
            int empty_num = s.empty_num;
            while (s.answers.Count > empty_num)
            {
                int delete_index = RandomInt("0", s.answers.Count.ToString());
                UserDefine.Var var = s.vars.First(e => e.name == s.answers[delete_index]);
                if ((object)var != null)
                {
                    if (!var.isanswer) { s.answers.RemoveAt(delete_index); }
                }
            }
            foreach (string answer in s.answers)
            {
                UserDefine.Var var = s.vars.First(e => e.name == answer);
                s.vars.Remove(var);
                var.isanswer = true;
                s.vars.Add(var);
            }
        }
        private void MakeVars(UserDefine.Subject s)
        {
            for (int i = 0; i < s.vars.Count; i++)
            {
                UserDefine.Var var = s.vars[i];
                if (var.isanswer)
                {
                    var.value = UserDefine.MakeInputHTML("int", "text", var.name, "width: 40px; text-align: center;");
                }
                else
                {
                    switch (var.type)
                    {
                        case "int":
                            {
                                if (s.vars[i].value == "")
                                {
                                    var.value = RandomInt(var.min, var.max).ToString();
                                }
                                break;
                            }
                        default:
                            { break; }
                    }
                }
                s.vars[i] = var;
            }
        }
        private bool AssertCondition(UserDefine.Subject s)
        {
            foreach (List<string> condition in s.conditions)
            {
                bool ret = true;
                foreach (string con in condition)
                {
                    string con_str = RepaceVarAssert(s.vars, con);
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
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            if (value < 0) { value = -value; }
            Random r = new Random(value);
            return r.Next(imin, imax);
        }
        private UserDefine.Subject MakePuzzle(UserDefine.Subject s)
        {
            string exp = s.template;
            string assert = s.assert;
            foreach (UserDefine.Var var in s.vars)
            {
                string var_str = "${" + var.name + "}";
                exp = exp.Replace(var_str, var.value);
                assert = assert.Replace(var_str, var.value);
            }
            s.content = exp;
            s.assert = assert;
            return s;
        }
        private string RepaceVarAssert(List<UserDefine.Var> vars, string exp)
        {
            string s = exp;
            foreach (UserDefine.Var var in vars)
            {
                string var_str = "${" + var.name + "}";
                if (var.isanswer)
                { s = s.Replace(var_str, "null"); }
                else
                { s = s.Replace(var_str, var.value); }
            }
            return s;
        }
    }
}
