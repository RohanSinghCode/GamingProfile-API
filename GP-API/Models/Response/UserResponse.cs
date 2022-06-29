namespace GP_API.Models.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string  AccessToken { get; set; }
        public DateTime DOB { get; set; }
        public string ProfilePicture { get; set; }
        public string CoverPicture { get; set; }
        public string About { get; set; }
    }
}
