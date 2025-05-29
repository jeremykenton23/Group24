using System.Windows.Forms;
using Xunit;
using NonogramApp;
using System.Linq;

namespace NanogramTests
{
    public class LoginFormTests
    {
        private readonly UserManager _manager;

        public LoginFormTests()
        {
            _manager = new UserManager();

            var testUser = new User
            {
                Username = "testuser",
                FirstName = "Jan",
                LastName = "Test",
                Email = "test@example.com",
                Settings = new UserSettings()
            };

            if (_manager.Login("testuser", "testpass") == null)
                _manager.Register(testUser, "testpass");
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldSucceed()
        {
            var form = new LoginForm(_manager);
            var txtUser = GetControl<TextBox>(form, "txtUser");
            var txtPass = GetControl<TextBox>(form, "txtPass");
            var btnLogin = GetControl<Button>(form, "btnLogin");

            txtUser.Text = "testuser";
            txtPass.Text = "testpass";
            btnLogin.PerformClick();

            Assert.False(form.Visible);
        }

        [Fact]
        public void LoginForm_ShouldContainAllControls()
        {
            // Arrange
            var form = new LoginForm(_manager);

            // Act
            var controls = form.Controls;

            // Assert
            Assert.NotNull(controls["txtUser"]);
            Assert.NotNull(controls["txtPass"]);
            Assert.NotNull(controls["btnLogin"]);
            Assert.NotNull(controls["btnGoRegister"]);
            Assert.NotNull(controls["lblMessage"]);
        }

        private T GetControl<T>(Control parent, string name) where T : Control
        {
            return parent.Controls.OfType<T>().FirstOrDefault(c => c.Name == name)
                   ?? throw new System.Exception($"Control with name {name} not found.");
        }
    }
}
