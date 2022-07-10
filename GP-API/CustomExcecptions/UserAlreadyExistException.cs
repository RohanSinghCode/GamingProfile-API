namespace GP_API.CustomExcecptions
{
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException()
            : base("User already exist")
        {
        }
    }
}
