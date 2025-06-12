using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using robot_controller_api.Models;
using robot_controller_api.Persistence;
using Isopoh.Cryptography.Argon2;

namespace robot_controller_api.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserDataAccess _userRepo;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserDataAccess userRepo
        ) : base(options, logger, encoder, clock)
        {
            _userRepo = userRepo;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers["WWW-Authenticate"] = @"Basic realm=""Access to the robot controller.""";

            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing or malformed Authorization header"));
            }

            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            string decodedCredentials;

            try
            {
                var byteCredentials = Convert.FromBase64String(encodedCredentials);
                decodedCredentials = Encoding.UTF8.GetString(byteCredentials);
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Base64 encoding"));
            }

            var parts = decodedCredentials.Split(':', 2);
            if (parts.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid credential format"));
            }

            var email = parts[0];
            var password = parts[1];

            var user = _userRepo.GetUserByEmail(email);

            if (user == null)
            {
                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
            }

            if (!Argon2.Verify(user.PasswordHash, password))
            {
                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail("Invalid password."));
            }

            var claims = new[]
            {
                new Claim("name", $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
