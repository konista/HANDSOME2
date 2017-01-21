using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HANDSOME2
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class Handsome2 : Form
    {
        private MODULE.SubjectCreator sc;
        private List<MODULE.UserDefine.Subject> ls = new List<MODULE.UserDefine.Subject>();
        public Handsome2()
        {
            InitializeComponent();
        }
        private MODULE.UserDefine.Subject CreateSubject(string subject)
        {
            return sc.CreateSubject(subject);
        }

        private void Handsome2_Load(object sender, EventArgs e)
        {
            WB.ObjectForScripting = this;
            GoHome();
        }
        public void GoHome()
        {
            WB.Navigate(Application.StartupPath + @"\PAGES\Home.html");
        }
        public void GoToSubject()
        {
            WB.Navigate(Application.StartupPath + @"\PAGES\Subject.html");
        }
        public void GoToJudge()
        {
            WB.Navigate(Application.StartupPath + @"\PAGES\Judge.html");
        }
        public void LoadMod(string type)
        {
            sc = new MODULE.SubjectCreator(type);
            string [] subjects = sc.ds.Keys.ToArray<string>();
            WB.Document.InvokeScript("loadsubject", subjects);
        }
        public void SaveSubject(string subject, string count)
        {
            int num = int.Parse(count);
            for (int i = 0; i < num; i++)
            {
                MODULE.UserDefine.Subject s = CreateSubject(subject);
                ls.Add(s);
            }
        }
        public int GetPuzzleCount()
        {
            return ls.Count;
        }
        public string ShowPazzle(int index)
        {
            MODULE.UserDefine.Subject s = ls[index];
            for (int i = 0; i < s.vars.Count; i++)
            {
                MODULE.UserDefine.Var var = s.vars[i];
                if (var.isanswer)
                {
                    string innerHTML = MODULE.UserDefine.MakeInputHTML("int", "text", var.name, var.value);
                    s.content = s.content.Replace(MODULE.UserDefine.MakeInputHTML("int", "text", var.name), innerHTML);
                }
            }
            ls[index] = s;
            return s.content;
        }
        public void SaveAnswer(int index, string name, string value)
        {
            MODULE.UserDefine.Subject s = ls[index];
            MODULE.UserDefine.Var var = s.vars.First(e => e.name == name);
            if ((object)var != null)
            {
                s.vars.Remove(var);
                var.value = value;
                s.vars.Add(var);
            }
            MODULE.UserDefine.Answer answer = s.answers.Find(e => e.name == name);
            if ((object)answer != null)
            {
                s.answers.Remove(answer);
                answer.user_value = value;
                s.answers.Add(answer);
            }
        }
        public void AssertAnswer(int index)
        {
            MODULE.UserDefine.Subject s = ls[index];
            string assert = s.assert;
            foreach (MODULE.UserDefine.Answer answer in s.answers)
            {
                if (answer.user_value == "" || answer.user_value == null) { return; }
                assert = assert.Replace("${" + answer.name + "}", answer.user_value);
            }
            s.correct = (bool)MODULE.JScript.JScriptRun("eval_expression", new object[] { assert });
            ls[index] = s;
        }
        public void ShowResult(int index)
        {
            MODULE.UserDefine.Subject s = ls[index];
            List<string> la = new List<string>();
            foreach (MODULE.UserDefine.Answer answer in s.answers)
            {
                la.Add(answer.user_value);
            }
            object[] param = new object[5] { index, s.name, s.text, string.Join(",", la), s.correct};
            WB.Document.InvokeScript("showresult", param);
        }
        public string GetCorrectAnswer(int index)
        {
            List<string> ret = new List<string>();
            foreach (MODULE.UserDefine.Answer answer in ls[index].answers)
            {
                ret.Add(answer.value);
            }
            return string.Join(",", ret);
        }
    }
}
