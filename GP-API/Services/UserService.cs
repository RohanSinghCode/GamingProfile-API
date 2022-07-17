namespace GP_API.Services
{
    using GP_API.CustomExcecptions;
    using GP_API.Models;
    using GP_API.Models.Response;
    using GP_API.Repository.Interfaces;
    using GP_API.Services.Interfaces;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserResponse Get(int id)
        {
            if (id <=0)
            {
                throw new InvalidRequestDataException("id");
            }

            var user = _userRepository.Get(id);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return new UserResponse()
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                CoverPicture = user.CoverPicture,
                About = user.About,
                DOB = user.DOB ?? null,
            };
        }
    }
}
