using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDSOME2.MODULE
{
    class SubjectCreator
    {
        public Dictionary<string, ModLoader.Subject> ds;
        public SubjectCreator(string type)
        {
            ModLoader ml = new ModLoader(type);
            ds = ml.ds;
        }
        public void CreateSubject(string sbj_name)
        {
            ModLoader.Subject s = ds[sbj_name];
            string ret = s.template;
            Dictionary<string, object> vars = MakeVars(s);
            if (Assert(vars, s.conditions))
            {

            }
            else
            { CreateSubject(sbj_name); }
        }
        private Dictionary<string,object> MakeVars(ModLoader.Subject s)
        {
            Dictionary<string, object> vars = new Dictionary<string, object>();
            foreach (ModLoader.Var var in s.vars)
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
            bool ret = false;
            return ret;
        }
        private int RandomInt(string min, string max)
        {
            int imin = int.Parse(min);
            int imax = int.Parse(max);
            Random r = new Random();
            return r.Next(imin, imax);
        }
    }
}
