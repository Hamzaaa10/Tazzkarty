namespace TAZZKARTY.Requests
{
    public class MatchRequest
    {
        public string Namev { get; set; }
        public string NameS { get; set; }
        public DateTime MatchTime { get; set; }
        public IFormFile? Image { get; set; }
        public string Tournament { get; set; }
        public string Location { get; set; }

        public int Price { get; set; }
    }
}
