namespace TAZZKARTY.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string Namev { get; set; }
        public string NameS { get; set; }
        public DateTime MatchTime { get; set; }
        public string Image { get; set; } = "";
        public string Tournament { get; set; }
        public string Location { get; set; }

        public int Price { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}