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
        private HANDSOME2.MODULE.SubjectCreator sc;
        public Handsome2()
        {
            InitializeComponent();
        }

        private void Handsome2_Load(object sender, EventArgs e)
        {
            WB.ObjectForScripting = this;
            WB.Navigate(Application.StartupPath + @"..\..\..\PAGES\Home.html");
        }

        public void LoadMod(string type)
        {
            sc = new MODULE.SubjectCreator(type);
            string [] subjects = sc.ds.Keys.ToArray<string>();
            WB.Document.InvokeScript("loadsubject", subjects);
        }

        public string CreateSubject(string subject)
        {
            return sc.CreateSubject(subject);
        }
        
    }
}
