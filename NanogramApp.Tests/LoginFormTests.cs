using System.Windows.Forms;
using NanogramGit;
using Xunit;

namespace NanogramGit.Tests
{
    public class LoginFormTests
    {
        [Fact]
        public void LoginForm_ShouldLoadCorrectly()
        {
            // Arrange
            var form = new LoginForm();

            // Act
            form.ShowDialog();  // Toon het formulier

            // Assert
            Assert.NotNull(form);  // Het formulier mag niet null zijn
            Assert.True(form.Visible);  // Het formulier moet zichtbaar zijn
        }
    }
}
