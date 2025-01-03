namespace AuthorizationApi.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // In a real app, never store plain text passwords!
    }
}
