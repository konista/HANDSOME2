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
            WB.Navigate(Application.StartupPath + @"..\..\..\PAGES\Home.html");
        }
        public void GoToSubject()
        {
            WB.Navigate(Application.StartupPath + @"..\..\..\PAGES\Subject.html");
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
            if (s.vars != null)
            {

            }
            return ls[index].content;
        }
        public void SaveAnswer(int index, string name, string value)
        {
            MODULE.UserDefine.Subject s = ls[index];
            MODULE.UserDefine.Var var = s.vars.First(e => e.name == name);
            if ((object)var != null)
            {
                s.vars.Remove(var);
                s.assert = s.assert.Replace("{" + name + "}", value);
                var.value = MODULE.UserDefine.MakeInputHTML("int", "text", var.name, "width: 40px; text-align: center;", value);
                s.vars.Add(var);
            }
        }
        
    }
}
