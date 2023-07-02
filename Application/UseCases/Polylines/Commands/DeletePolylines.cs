using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Polylines.Commands
{
    public static class DeletePolylines
    {
        public record Command(int Id) : IRequest<Result>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var @Polylines = await ApplicationDbContext.Polylines.SingleOrDefaultAsync(x => x.Id == command.Id);
                if (@Polylines == null)
                    throw new Exception("No Polylines found.");
                @Polylines.IsDeleted = true;
                @Polylines.DeletedDate = DateTime.Now;
                ApplicationDbContext.Polylines.Update(@Polylines);
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
