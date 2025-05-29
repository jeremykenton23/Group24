using System;
using System.Windows.Forms;

namespace NonogramApp
{
    public class RegisterForm : Form
    {
        private TextBox txtUsername = new();
        private TextBox txtPassword = new();
        private TextBox txtFirstName = new();
        private TextBox txtLastName = new();
        private TextBox txtEmail = new();
        private Button btnRegister = new();
        private Label lblStatus = new();
        private UserManager userManager;

        public RegisterForm(UserManager userManager)
        {
            this.userManager = userManager;

            this.Text = "Register";
            this.Size = new System.Drawing.Size(350, 330);
            this.StartPosition = FormStartPosition.CenterScreen;

            // ✅ Voeg unieke namen toe aan alle controls
            txtUsername.Name = "txtUsername";
            txtPassword.Name = "txtPassword";
            txtFirstName.Name = "txtFirstName";
            txtLastName.Name = "txtLastName";
            txtEmail.Name = "txtEmail";
            btnRegister.Name = "btnRegister";
            lblStatus.Name = "lblStatus";

            txtUsername.PlaceholderText = "Username";
            txtUsername.Top = 20;
            txtUsername.Left = 50;
            txtUsername.Width = 230;

            txtPassword.PlaceholderText = "Password";
            txtPassword.Top = 60;
            txtPassword.Left = 50;
            txtPassword.Width = 230;
            txtPassword.PasswordChar = '*';

            txtFirstName.PlaceholderText = "First Name";
            txtFirstName.Top = 100;
            txtFirstName.Left = 50;
            txtFirstName.Width = 230;

            txtLastName.PlaceholderText = "Last Name";
            txtLastName.Top = 140;
            txtLastName.Left = 50;
            txtLastName.Width = 230;

            txtEmail.PlaceholderText = "Email";
            txtEmail.Top = 180;
            txtEmail.Left = 50;
            txtEmail.Width = 230;

            btnRegister.Text = "Register";
            btnRegister.Top = 220;
            btnRegister.Left = 100;
            btnRegister.Width = 120;
            btnRegister.Click += Register;

            lblStatus.Top = 260;
            lblStatus.Left = 50;
            lblStatus.Width = 250;

            this.Controls.AddRange(new Control[]
            {
                txtUsername, txtPassword, txtFirstName, txtLastName, txtEmail, btnRegister, lblStatus
            });
        }

        private void Register(object? sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email))
            {
                lblStatus.Text = "Please fill in all fields.";
                return;
            }

            var newUser = new User
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Settings = new UserSettings()
            };

            bool success = userManager.Register(newUser, password);
            lblStatus.Text = success ? "Registration successful!" : "Username already exists.";
        }
    }
}
