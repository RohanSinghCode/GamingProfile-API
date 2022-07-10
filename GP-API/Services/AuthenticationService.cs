namespace GP_API.Services
{
    using DapperExtensions;
    using DapperExtensions.Predicate;
    using GP_API.Models;
    using GP_API.Models.Request;
    using GP_API.Models.Response;
    using GP_API.Repository.Interfaces;
    using GP_API.Services.Interfaces;
    using BCrypt.Net;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;
    using Microsoft.Extensions.Options;
    using GP_API.CustomExcecptions;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        public AuthenticationService(IUserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        public UserResponse Login(LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                throw new ArgumentNullException(nameof(loginRequest));
            }

            if (loginRequest.Username == null && loginRequest.Email == null)
            {
                throw new ArgumentNullException("Username and Email");
            }

            if (loginRequest.Password == null)
            {
                throw new ArgumentNullException("Password");
            }

            User user = null;

            if (!string.IsNullOrEmpty(loginRequest.Username))
            {
                user = _userRepository.GetByPredicate(Predicates.Field<User>(u => u.Username, Operator.Eq, loginRequest.Username)).FirstOrDefault();
            } 
            else 
            {
                user = _userRepository.GetByPredicate(Predicates.Field<User>(u => u.Email, Operator.Eq, loginRequest.Email)).FirstOrDefault();
            }

            if (user == null)
            {
                throw new InvalidRequestDataException($"{loginRequest.Username} or {loginRequest.Email}");
            }

            if (BCrypt.Verify(loginRequest.Password, user.Passowrd))
            {
                throw new InvalidRequestDataException("password");
            }

            var jwtToken = GenerateJwtToken(user);

            return new UserResponse()
            {
                Id = user.Id,
                AccessToken = jwtToken,
                Username = user.Username,
                Email = user.Email,
                About = user.About,
                ProfilePicture = user.ProfilePicture,
                CoverPicture = user.CoverPicture,
                Name = user.Name,
            };
        }

        public int SignUp(UserRequest userRequest)
        {
            ValidateSignUpRequest(userRequest);
            var hashedPassword = BCrypt.HashPassword(userRequest.Password);
            var userId = _userRepository.Add(new User()
            {
                Username = userRequest.Username,
                Name = userRequest.Name,
                Email = userRequest.Email,
                Passowrd = hashedPassword,
                DOB = userRequest.DOB,
                About = userRequest.About,
                CoverPicture = userRequest.CoverPicture,
                ProfilePicture = userRequest.ProfilePicture
            });
            return userId;
        }

        private void ValidateSignUpRequest(UserRequest userRequest)
        {
            if (string.IsNullOrWhiteSpace(userRequest.Email))
            {
                throw new ArgumentNullException("Email");
            }

            if (string.IsNullOrWhiteSpace(userRequest.Name))
            {
                throw new ArgumentNullException("Name");
            }

            if (string.IsNullOrEmpty(userRequest.Password))
            {
                throw new ArgumentNullException("Password");
            }

            if (userRequest.Password.Length <= 8)
            {
                throw new InvalidRequestDataException("password");
            }

            var groupPredicate = new PredicateGroup()
            {
                Operator = GroupOperator.Or,
                Predicates = new List<IPredicate>
                {
                    Predicates.Field<User>(u => u.Email, Operator.Eq, userRequest.Email)
                }
            };

            if (!string.IsNullOrEmpty(userRequest.Username))
            {
                if (userRequest.Username.Length > 20 || userRequest.Username.Any(ch => (!Char.IsLetterOrDigit(ch) || ch != '.')))
                {
                    throw new InvalidRequestDataException("Username");
                }
                groupPredicate.Predicates.Add(Predicates.Field<User>(u => u.Username, Operator.Eq, userRequest.Username));
            }

            var user = _userRepository.GetByPredicate(groupPredicate);

            if (user != null)
            {
                throw new UserAlreadyExistException();
            }
        }

        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userId", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
