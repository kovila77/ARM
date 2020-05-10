using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Authentication;

namespace MainForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //fAuth fAuth = new fAuth(fAuth.TypeLoad.Authentication);
            //if (fAuth.ShowDialog() == DialogResult.OK)
            {
                //if (fAuth.UserRole == null) return;
                //Application.Run(new MainForm(fAuth.UserRole));
                Application.Run(new MainForm("admin"));
            }
        }
    }
}
