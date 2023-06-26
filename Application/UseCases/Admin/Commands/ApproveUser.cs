using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Admin.Commands
{
    public static class ApproveUser
    {
        public record Command(int UserId) : IRequest<Result>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await ApplicationDbContext.Users.SingleOrDefaultAsync(x => x.Id == command.UserId);
                    if (user == null)
                        throw new Exception("No user found.");
                    if (user.Status == Domain.Enumerables.UserStatus.Active)
                        throw new Exception("User already active.");
                    user.Status = Domain.Enumerables.UserStatus.Active;
                    await ApplicationDbContext.SaveChangesAsync();
                }
                catch (Exception)
                {

                    throw;
                }
                return new Result();
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
