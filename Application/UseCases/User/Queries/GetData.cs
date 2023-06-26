using Application.Interfaces;
using Domain.Entities;
using Domain.Enumerables;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.User.Queries
{
    public static class GetData
    {
        public record Command(int Id) : IRequest<Result>;
        public record Result
        {
            public int Id { get; set; }
            public string Status { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Username { get; set; }
            public string Token { get; set; }
        };
        public record Handler(IApplicationDbContext ApplicationDbContext, ICryptography Cryptography, ITokenClient TokenClient) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command query, CancellationToken cancellationToken)
            {
                var data = await ApplicationDbContext.Users.SingleOrDefaultAsync(x => x.Id == query.Id);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, data.Id.ToString()),
                    new Claim(ClaimTypes.Name, data.Username),
                    new Claim(ClaimTypes.Role, data.Type.ToString())
                };
                var identity = new ClaimsIdentity(claims);
                var principal = new ClaimsPrincipal(identity);
                var token = TokenClient.Generate(principal);
                return new Result
                {
                    Id = data.Id,
                    Name = Cryptography.Decrypt(data.Name),
                    Status = data.Status.ToString(),
                    Type = data.Type.ToString(),
                    Username = data.Username,
                    Token = token
                };
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
