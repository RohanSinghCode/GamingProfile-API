namespace GP_API.Services.Interfaces
{
    using GP_API.Models.Response;

    public interface IUserService
    {
        UserResponse Get(int id);
    }
}
