﻿using System;
using System.Windows.Forms;

namespace NonogramApp
{
    public class LoginForm : Form
    {
        private TextBox txtUser = new TextBox();
        private TextBox txtPass = new TextBox();
        private Button btnLogin = new Button();
        private Button btnGoRegister = new Button();
        private Label lblMessage = new Label();
        private UserManager userManager = new UserManager();

        public LoginForm()
        {
            this.Text = "Login";
            this.Size = new System.Drawing.Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            txtUser.PlaceholderText = "Username";
            txtUser.Top = 20;
            txtUser.Left = 50;
            txtUser.Width = 200;

            txtPass.PlaceholderText = "Password";
            txtPass.Top = 50;
            txtPass.Left = 50;
            txtPass.Width = 200;
            txtPass.PasswordChar = '*';

            btnLogin.Text = "Login";
            btnLogin.Top = 90;
            btnLogin.Left = 50;
            btnLogin.Click += Login;

            btnGoRegister.Text = "Register";
            btnGoRegister.Top = 90;
            btnGoRegister.Left = 130;
            btnGoRegister.Click += (s, e) =>
            {
                Hide();
                new RegisterForm().ShowDialog();
                Show();
            };

            lblMessage.Top = 130;
            lblMessage.Left = 50;
            lblMessage.Width = 200;

            Controls.Add(txtUser);
            Controls.Add(txtPass);
            Controls.Add(btnLogin);
            Controls.Add(btnGoRegister);
            Controls.Add(lblMessage);
        }

        private void Login(object? sender, EventArgs e)
        {
            var username = txtUser.Text.Trim();
            var password = txtPass.Text;

            var user = userManager.Login(username, password);

            if (user != null)
            {
                Hide();
                new MainForm(user).ShowDialog();
                Close();
            }
            else
            {
                lblMessage.Text = "Login failed.";
            }
        }
    }
}
