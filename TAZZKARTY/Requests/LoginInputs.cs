using TAZZKARTY.Models;

namespace TAZZKARTY.Requests
{
    public class LoginInputs
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

    }
}
