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
            var user = Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return null;

            string hashed = HashPasswordWithSalt(password, user.Salt);
            return user.HashedPassword == hashed ? user : null;
        }

        public bool Register(User newUser, string plainPassword)
        {
            if (Users.Any(u => u.Username.Equals(newUser.Username, StringComparison.OrdinalIgnoreCase)))
                return false;

            newUser.Salt = GenerateSalt();
            newUser.HashedPassword = HashPasswordWithSalt(plainPassword, newUser.Salt);

            Users.Add(newUser);
            SaveUsers();
            return true;
        }

        public void UpdateUserSettings(User user)
        {
            var found = Users.FirstOrDefault(u => u.Username == user.Username);
            if (found != null)
            {
                found.Settings = user.Settings;
                SaveUsers();
            }
        }

        public void UpdateUser(User updatedUser, string? newPlainPassword = null)
        {
            var found = Users.FirstOrDefault(u => u.Username == updatedUser.Username);
            if (found != null)
            {
                found.FirstName = updatedUser.FirstName;
                found.LastName = updatedUser.LastName;
                found.Email = updatedUser.Email;

                if (!string.IsNullOrWhiteSpace(newPlainPassword))
                {
                    found.Salt = GenerateSalt();
                    found.HashedPassword = HashPasswordWithSalt(newPlainPassword, found.Salt);
                }

                found.Settings = updatedUser.Settings;
                SaveUsers();
            }
        }

        public void DeleteUser(User user)
        {
            Users.RemoveAll(u => u.Username == user.Username);
            SaveUsers();
        }

        private void SaveUsers()
        {
            var json = JsonSerializer.Serialize(Users, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(FilePath, json);
        }

        private List<User> LoadUsers()
        {
            if (!File.Exists(FilePath)) return new List<User>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public static string HashPasswordWithSalt(string password, string salt)
        {
            using var sha = SHA256.Create();
            var salted = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha.ComputeHash(salted);
            return Convert.ToBase64String(hash);
        }

        public static string GenerateSalt()
        {
            byte[] bytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
