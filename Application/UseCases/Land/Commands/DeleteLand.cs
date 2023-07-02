using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Land.Commands
{
    public static class DeleteLand
    {
        public record Command(int Id) : IRequest<Result>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var @Land = await ApplicationDbContext.Lands.SingleOrDefaultAsync(x => x.Id == command.Id);
                if (@Land == null)
                    throw new Exception("No LandCategory found.");
                @Land.IsDeleted = true;
                @Land.DeletedDate = DateTime.Now;
                ApplicationDbContext.Lands.Update(@Land);
                await ApplicationDbContext.SaveChangesAsync();
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
