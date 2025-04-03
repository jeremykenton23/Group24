﻿namespace NonogramApp
{
    public class UserSettings
    {
        public string Theme { get; set; } = "light";
        public int GridSize { get; set; } = 10;
    }

    public class User
    {
        // Gebruikersgegevens
        public string Username { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;

        // Extra profielinformatie
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Persoonlijke instellingen
        public UserSettings Settings { get; set; } = new UserSettings();
    }
}
