namespace GP_API.Services.Interfaces
{
    using GP_API.Models.Request;
    using GP_API.Models.Response;

    public interface IAuthenticationService
    {
        /// <summary>
        /// Will take in the login model and authenticate the user
        /// </summary>
        /// <returns>User repsonse model</returns>
        UserResponse Login(LoginRequest loginRequest);

        /// <summary>
        /// Signs up.
        /// </summary>
        /// <param name="userRequest">The user request.</param>
        /// <returns>id of the user</returns>
        int SignUp(UserRequest userRequest);
    }
}
