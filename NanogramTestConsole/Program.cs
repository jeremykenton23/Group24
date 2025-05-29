using System;
using System.IO;
using System.Linq;
using NonogramApp;

namespace NanogramTestConsole
{
    class Program
    {
        static void Main()
        {
            string filePath = "users.json";
            if (File.Exists(filePath))
                File.Delete(filePath); // Opschonen testdata

            var manager = new UserManager();

            RunUnitTests();
            RunIntegrationTests(manager);
            RunAcceptanceTests(manager);

            // Opruimen testdata
            if (File.Exists(filePath))
                File.Delete(filePath);

            Console.WriteLine("== Alle tests voltooid ==");
            Console.ReadKey();
        }

        // ✅ UNIT TESTS
        static void RunUnitTests()
        {
            Console.WriteLine("== Unit Test 1: Hashing consistentie ==");
            string pw = "abc123";
            string salt = UserManager.GenerateSalt();
            string hash1 = UserManager.HashPasswordWithSalt(pw, salt);
            string hash2 = UserManager.HashPasswordWithSalt(pw, salt);

            Console.WriteLine(hash1 == hash2 ? "✅ Geslaagd" : "❌ Gefaald");

            Console.WriteLine("== Unit Test 2: Hash verandert bij andere salt ==");
            string otherSalt = UserManager.GenerateSalt();
            string otherHash = UserManager.HashPasswordWithSalt(pw, otherSalt);
            Console.WriteLine(hash1 != otherHash ? "✅ Geslaagd" : "❌ Gefaald");
        }

        // ✅ INTEGRATIETESTS
        static void RunIntegrationTests(UserManager manager)
        {
            Console.WriteLine("== Integratietest 1: Register + Login ==");

            var user = new User
            {
                Username = "intuser",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Settings = new UserSettings()
            };

            bool reg = manager.Register(user, "pass123");
            var login = manager.Login("intuser", "pass123");

            Console.WriteLine(reg && login != null ? "✅ Geslaagd" : "❌ Gefaald");

            Console.WriteLine("== Integratietest 2: Verwijderen + herladen ==");

            manager.DeleteUser(user);
            manager.SaveUsers();

            var newManager = new UserManager();
            bool notFound = newManager.Users.All(u => u.Username != "intuser");

            Console.WriteLine(notFound ? "✅ Geslaagd" : "❌ Gefaald");
        }

        // ✅ ACCEPTATIETESTS
        static void RunAcceptanceTests(UserManager manager)
        {
            Console.WriteLine("== Acceptatietest 1: Registratie en login ==");

            var user = new User
            {
                Username = "acceptuser",
                FirstName = "Acc",
                LastName = "Test",
                Email = "acc@test.com",
                Settings = new UserSettings()
            };

            manager.Register(user, "accpass");
            var login = manager.Login("acceptuser", "accpass");

            Console.WriteLine(login != null ? "✅ Geslaagd" : "❌ Gefaald");

            Console.WriteLine("== Acceptatietest 2: Verwijderen na login ==");

            manager.DeleteUser(user);
            var result = manager.Login("acceptuser", "accpass");

            Console.WriteLine(result == null ? "✅ Geslaagd" : "❌ Gefaald");
        }
    }
}
