using System;
using System.Windows.Forms;

namespace NonogramApp
{
    public class LoginForm : Form
    {
        private TextBox txtUser;
        private TextBox txtPass;
        private Button btnLogin;
        private Button btnGoRegister;
        private Label lblMessage;
        private readonly UserManager userManager;

        public LoginForm(UserManager userManager)
        {
            this.userManager = userManager;

            this.Text = "Login";
            this.Size = new System.Drawing.Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            // ✅ Initieer en naam alle controls voor tests
            txtUser = new TextBox { Name = "txtUser", PlaceholderText = "Username", Location = new System.Drawing.Point(50, 20), Width = 200 };
            txtPass = new TextBox { Name = "txtPass", PlaceholderText = "Password", PasswordChar = '*', Location = new System.Drawing.Point(50, 50), Width = 200 };
            btnLogin = new Button { Name = "btnLogin", Text = "Login", Location = new System.Drawing.Point(50, 90), Width = 70 };
            btnGoRegister = new Button { Name = "btnGoRegister", Text = "Register", Location = new System.Drawing.Point(130, 90), Width = 100 };
            lblMessage = new Label { Name = "lblMessage", Text = "", Location = new System.Drawing.Point(50, 130), Width = 200 };

            // ✅ Events koppelen
            btnLogin.Click += Login;
            btnGoRegister.Click += (s, e) =>
            {
                Hide();
                new RegisterForm(userManager).ShowDialog();
                Show();
            };

            // ✅ Voeg alle controls toe
            this.Controls.AddRange(new Control[]
            {
                txtUser, txtPass, btnLogin, btnGoRegister, lblMessage
            });
        }

        private void Login(object? sender, EventArgs e)
        {
            var username = txtUser.Text.Trim();
            var password = txtPass.Text;

            var user = userManager.Login(username, password);
            if (user != null)
            {
                Hide();
                new MainForm(user, userManager).ShowDialog();
                Close();
            }
            else
            {
                lblMessage.Text = "Login failed. Check credentials.";
            }
        }
    }
}
