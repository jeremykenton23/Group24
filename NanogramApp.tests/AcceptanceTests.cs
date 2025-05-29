using Xunit;
using NonogramApp;
using System.IO;

namespace NanogramApp.tests
{
    public class UserManagerAcceptanceTests
    {
        private const string TestFilePath = "test_users.json";

        private UserManager CreateUserManager()
        {
            // Zorg ervoor dat elke test met een lege lijst begint
            if (File.Exists(TestFilePath))
                File.Delete(TestFilePath);

            File.WriteAllText(TestFilePath, "[]");

            // Vervang het pad tijdelijk in UserManager
            typeof(UserManager)
                .GetField("FilePath", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, TestFilePath);

            return new UserManager();
        }

        [Fact]
        public void Register_And_Login_ShouldSucceed()
        {
            var manager = CreateUserManager();

            var user = new User
            {
                Username = "janedoe",
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                Settings = new UserSettings()
            };

            bool registered = manager.Register(user, "secure123");
            Assert.True(registered);

            var loginUser = manager.Login("janedoe", "secure123");
            Assert.NotNull(loginUser);
            Assert.Equal("janedoe", loginUser.Username);
        }
        [Fact]
        public void DeleteUser_ShouldPreventLogin()
        {
            var manager = CreateUserManager();

            var user = new User
            {
                Username = "todelete",
                FirstName = "To",
                LastName = "Delete",
                Email = "delete@me.com",
                Settings = new UserSettings()
            };

            manager.Register(user, "removeMe123");

            // Verwijder gebruiker
            manager.DeleteUser(user);

            // Moet null retourneren, want gebruiker is verwijderd
            var login = manager.Login("todelete", "removeMe123");
            Assert.Null(login);
        }
    }
}

   