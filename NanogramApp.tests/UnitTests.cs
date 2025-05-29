using System.Linq;
using System.Windows.Forms;
using Xunit;
using NonogramApp;
using System;

namespace NanogramApp.tests
{
    public class RegisterFormValidationTests
    {
        [Fact]
        public void Register_WithValidUser_ShouldSucceed()
        {
            // Arrange
            var manager = new UserManager();
            var uniqueUsername = "user_" + Guid.NewGuid().ToString("N").Substring(0, 6);
            var user = new User
            {
                Username = uniqueUsername,
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Settings = new UserSettings()
            };

            // Act
            var result = manager.Register(user, "securePass123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RegisterForm_ShouldContainAllInputControls()
        {
            var form = new RegisterForm(new UserManager());

            Assert.NotNull(GetTextBox(form, "txtUsername"));
            Assert.NotNull(GetTextBox(form, "txtPassword"));
            Assert.NotNull(GetTextBox(form, "txtFirstName"));
            Assert.NotNull(GetTextBox(form, "txtLastName"));
            Assert.NotNull(GetTextBox(form, "txtEmail"));
            Assert.NotNull(GetButton(form, "btnRegister"));
            Assert.NotNull(GetLabel(form, "lblStatus"));
        }

        // 🔧 Hulpmethodes
        private TextBox GetTextBox(Form form, string name)
        {
            return form.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == name);
        }

        private Button GetButton(Form form, string name)
        {
            return form.Controls.OfType<Button>().FirstOrDefault(b => b.Name == name);
        }

        private Label GetLabel(Form form, string name)
        {
            return form.Controls.OfType<Label>().FirstOrDefault(l => l.Name == name);
        }
    }
}
