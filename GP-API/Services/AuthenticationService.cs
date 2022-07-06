namespace GP_API.Services
{
    using DapperExtensions;
    using DapperExtensions.Predicate;
    using GP_API.Models;
    using GP_API.Models.Request;
    using GP_API.Models.Response;
    using GP_API.Repository.Interfaces;
    using GP_API.Services.Interfaces;
    using System.Security.Cryptography;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using System;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserResponse Login(LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                throw new ArgumentNullException("loginRequest");
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
                throw new ArgumentException("No user found");
            }

            // check the password from the login request and from the user object
            var hashedPassword = GenerateHashPassword(loginRequest.Password);
            if (user.Passowrd != hashedPassword)
            {
                throw new UnauthorizedAccessException("Passoword");
            }
        }

        public int SignUp(UserRequest userRequest)
        {
            throw new NotImplementedException();
        }

        private string GenerateHashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
#pragma warning disable SYSLIB0023 // Type or member is obsolete
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
#pragma warning restore SYSLIB0023 // Type or member is obsolete
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }
    }
}
