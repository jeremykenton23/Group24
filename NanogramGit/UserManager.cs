using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace NonogramApp
{
    public class UserManager
    {
        private const string FilePath = "users.json";
        public List<User> Users { get; private set; }

        public UserManager()
        {
            Users = LoadUsers();
        }

        public User? Login(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null) return null;

            string hashedInput = HashPasswordWithSalt(password, user.Salt);
            return hashedInput == user.HashedPassword ? user : null;
        }

        public bool Register(User newUser, string plainPassword)
        {
            if (Users.Any(u => u.Username.Equals(newUser.Username, StringComparison.OrdinalIgnoreCase)))
                return false;

            string salt = GenerateSalt();
            string hashedPassword = HashPasswordWithSalt(plainPassword, salt);

            newUser.Salt = salt;
            newUser.HashedPassword = hashedPassword;

            Users.Add(newUser);
            SaveUsers();
            return true;
        }

        public void UpdateUser(User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Username == updatedUser.Username);
            if (user != null)
            {
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;

                if (!string.IsNullOrWhiteSpace(updatedUser.HashedPassword))
                {
                    string newSalt = GenerateSalt();
                    user.Salt = newSalt;
                    user.HashedPassword = HashPasswordWithSalt(updatedUser.HashedPassword, newSalt);
                }

                user.Settings = updatedUser.Settings;
                SaveUsers();
            }
        }

        public void UpdateUserSettings(User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Username == updatedUser.Username);
            if (user != null)
            {
                user.Settings = updatedUser.Settings;
                SaveUsers();
            }
        }

        public void DeleteUser(User user)
        {
            Users.RemoveAll(u => u.Username == user.Username);
            SaveUsers();
        }

        public void SaveUsers()
        {
            var json = JsonSerializer.Serialize(Users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        private List<User> LoadUsers()
        {
            if (!File.Exists(FilePath))
                return new List<User>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public static string HashPasswordWithSalt(string password, string salt)
        {
            using var sha = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
    }
}
