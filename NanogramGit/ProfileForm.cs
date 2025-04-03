using System;
using System.Drawing;
using System.Windows.Forms;

namespace NonogramApp
{
    public class ProfileForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtEmail;
        private TextBox txtNewPassword;
        private Button btnSave;
        private Button btnDelete;

        private User currentUser;
        private Action<User> onSave;
        private Action onDelete;

        public ProfileForm(User user, Action<User> onSave, Action onDelete)
        {
            currentUser = user;
            this.onSave = onSave;
            this.onDelete = onDelete;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Profile";
            this.Size = new Size(320, 320);
            this.StartPosition = FormStartPosition.CenterParent;

            txtUsername = new TextBox
            {
                Text = currentUser.Username,
                Location = new Point(50, 20),
                Width = 200,
                ReadOnly = true
            };

            txtFirstName = new TextBox
            {
                Text = currentUser.FirstName ?? "",
                PlaceholderText = "First Name",
                Location = new Point(50, 50),
                Width = 200
            };

            txtLastName = new TextBox
            {
                Text = currentUser.LastName ?? "",
                PlaceholderText = "Last Name",
                Location = new Point(50, 80),
                Width = 200
            };

            txtEmail = new TextBox
            {
                Text = currentUser.Email ?? "",
                PlaceholderText = "Email",
                Location = new Point(50, 110),
                Width = 200
            };

            txtNewPassword = new TextBox
            {
                PlaceholderText = "New Password (optional)",
                Location = new Point(50, 140),
                Width = 200,
                UseSystemPasswordChar = true
            };

            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(50, 190),
                Width = 90
            };
            btnSave.Click += Save;

            btnDelete = new Button
            {
                Text = "Delete Account",
                Location = new Point(150, 190),
                Width = 100
            };
            btnDelete.Click += Delete;

            this.Controls.Add(txtUsername);
            this.Controls.Add(txtFirstName);
            this.Controls.Add(txtLastName);
            this.Controls.Add(txtEmail);
            this.Controls.Add(txtNewPassword);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnDelete);
        }

        private void Save(object? sender, EventArgs e)
        {
            currentUser.FirstName = txtFirstName.Text.Trim();
            currentUser.LastName = txtLastName.Text.Trim();
            currentUser.Email = txtEmail.Text.Trim();

            // Alleen bij een nieuw wachtwoord: herhashen met nieuwe salt
            if (!string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                var newSalt = UserManager.GenerateSalt();
                currentUser.Salt = newSalt;
                currentUser.HashedPassword = UserManager.HashPasswordWithSalt(txtNewPassword.Text, newSalt);
            }

            onSave(currentUser);
            MessageBox.Show("Profile updated.");
            this.Close();
        }

        private void Delete(object? sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to delete your account?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                onDelete();
                MessageBox.Show("Account deleted.");
                this.Close();
                Application.Restart();
            }
        }
    }
}
