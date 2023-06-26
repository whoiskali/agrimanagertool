using Application.Interfaces;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.User.Commands
{
    public static class Registration
    {
        public record Command(string Name, string Username, string Password) : IRequest<Result>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext, ICryptography Cryptography) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                    if (await ApplicationDbContext.DuplicateUserNameCheckerAsync(command.Username))
                        throw new Exception("Username already existed.");
                    var user = command.Adapt<Domain.Entities.User>();
                    user.Name = Cryptography.Encrypt(command.Name);
                    user.Type = Domain.Enumerables.Usertype.Manager;
                    user.Status = Domain.Enumerables.UserStatus.Pending;
                    user.IsDeleted = false;
                    user.Password = Cryptography.BCryptEncrypt(command.Password);
                    var inserted = ApplicationDbContext.Users.Add(user);
                    await ApplicationDbContext.SaveChangesAsync(cancellationToken);
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
