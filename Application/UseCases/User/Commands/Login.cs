using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.User.Commands
{
    public static class Login
    {
        public record Command(string Username, string Password) : IRequest<Result>;
        public record Result(string Token);
        public record Handler(IApplicationDbContext ApplicationDbContext, ICryptography Cryptography, ITokenClient TokenClient) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var user = await ApplicationDbContext.Users.SingleOrDefaultAsync(x => x.Username == command.Username && !x.IsDeleted);
                if (user == null)
                    throw new Exception("Username/Password is incorrect.");
                if (user.Status != Domain.Enumerables.UserStatus.Active)
                    throw new Exception("You account is " + user.Status.ToString());
                if (!Cryptography.BCryptVerify(command.Password, user.Password))
                    throw new Exception("Username/Password is incorrect.");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Type.ToString())
                };
                var identity = new ClaimsIdentity(claims);
                var principal = new ClaimsPrincipal(identity);
                var token = TokenClient.Generate(principal);

                return new Result(token);
            }
        }

        public class Error : Exception
        {
            public Error(string message) : base(message)
            {

            }
        }
    }
}
