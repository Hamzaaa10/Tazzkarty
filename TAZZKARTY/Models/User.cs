namespace TAZZKARTY.Models
{
    public class User
    {
        public int Id { get; set; }
        public string NationalId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public ICollection<Match> Matches { get; set; }
        public string Image { get; set; } = "";

    }

    public enum Role
    {
        Admin,
        User


    }
}
