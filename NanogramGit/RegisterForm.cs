using System;
using System.Windows.Forms;

namespace NonogramApp
{
    public class RegisterForm : Form
    {
        private TextBox txtUsername = new TextBox();
        private TextBox txtPassword = new TextBox();
        private TextBox txtFirstName = new TextBox();
        private TextBox txtLastName = new TextBox();
        private TextBox txtEmail = new TextBox();
        private Button btnRegister = new Button();
        private Label lblStatus = new Label();
        private UserManager userManager = new UserManager();

        public RegisterForm()
        {
            this.Text = "Register";
            this.Size = new System.Drawing.Size(350, 330);
            this.StartPosition = FormStartPosition.CenterScreen;

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

            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Controls.Add(txtFirstName);
            Controls.Add(txtLastName);
            Controls.Add(txtEmail);
            Controls.Add(btnRegister);
            Controls.Add(lblStatus);
        }

        private void Register(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblStatus.Text = "Please fill in all fields.";
                return;
            }

            var newUser = new User
            {
                Username = txtUsername.Text.Trim(),
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Settings = new UserSettings()
            };

            bool success = userManager.Register(newUser, txtPassword.Text);
            lblStatus.Text = success ? "Registration successful!" : "Username already exists.";
        }
    }
}
