namespace NonogramApp
{
    public class UserSettings
    {
        public string Theme { get; set; } = "Light";
        public int GridSize { get; set; } = 5;
    }

    public class User
    {
        public string Username { get; set; } = "";
        public string Salt { get; set; } = "";
        public string HashedPassword { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public UserSettings Settings { get; set; } = new UserSettings();
    }
}
