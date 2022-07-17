namespace GP_API.MiddleWares
{
    using GP_API.Models;
    using GP_API.Repository.Interfaces;
    using GP_API.Services.Interfaces;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;

    public class JwtMiddleWare
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly AppSettings _appSettings;
        public JwtMiddleWare(RequestDelegate requestDelegate, IOptions<AppSettings> appSettings)
        {
            _requestDelegate = requestDelegate;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context,IUserRepository userRepository)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                attachUserToContext(context, userRepository, token);
            }
            await _requestDelegate(context);
        }

        private void attachUserToContext(HttpContext context, IUserRepository userRepository, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);

                // attach user to context on successful jwt validation
                context.Items["User"] = userRepository.Get(userId);
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
