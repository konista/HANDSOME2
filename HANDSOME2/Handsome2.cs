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
        public Handsome2()
        {
            InitializeComponent();
        }

        public void LoadMod(string type)
        {
            string mod = "";
            HANDSOME2.MODULE.MODLoader ml = new MODULE.MODLoader();
            ml.Load(mod);
        }

        private void Handsome2_Load(object sender, EventArgs e)
        {
            WB.ObjectForScripting = this;
            WB.Navigate(Application.StartupPath + "..\\..\\..\\PAGES/Home.html");
        }
    }
}
