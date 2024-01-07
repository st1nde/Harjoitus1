using Harjoitus1.Middleware;
using Harjoitus1.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Core.Types;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Harjoitus1.Models;

namespace Harjoitus1
{
    public class BasicAuthenticationHandler: AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserAuthenticationService _authenticationService;
        private readonly IUserRepository _repository;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions>
            options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserAuthenticationService
            authenticationService, IUserRepository repository) : base(options, logger, encoder, clock)
        {
            _repository = repository;
            _authenticationService = authenticationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
           
        
            string userName = "";
            string password = "";
            User? user;
            var endpoint = Context.GetEndpoint();
            if (endpoint? .Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return AuthenticateResult.NoResult();
            }

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authorization header missing");
            }
            
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialData = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialData).Split(new[] { ':' }, 2);
                userName = credentials[0];
                password = credentials[1];


                user = await _authenticationService.Authenticate(userName, password);

                if (userName =="tunnus" && password == "salasana")
                {
                    return AuthenticateResult.Fail("Unauthorized");
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Unauthorized");   
            }


            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);

        }
        

    }


}
