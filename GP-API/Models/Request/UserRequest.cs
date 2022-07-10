namespace GP_API.Models.Request
{
    public class UserRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public DateTime DOB { get; set; }
        public string ProfilePicture { get; set; }
        public string CoverPicture { get; set; }
        public string About { get; set; }
    }
}
