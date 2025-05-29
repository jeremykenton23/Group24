using System;
using System.Windows.Forms;

namespace NonogramApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var userManager = new UserManager(); // ✅ gedeelde UserManager
            Application.Run(new LoginForm(userManager));
        }
    }
}
