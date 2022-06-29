namespace GP_API.Services
{
    using GP_API.Models.Request;
    using GP_API.Models.Response;
    using GP_API.Services.Interfaces;

    public class AuthenticationService : IAuthenticationService
    {
        public UserResponse Login(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public int SignUp(UserRequest userRequest)
        {
            throw new NotImplementedException();
        }
    }
}
