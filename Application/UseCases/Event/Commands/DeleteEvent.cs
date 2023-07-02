using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Commands
{
    public static class DeleteEvent
    {
        public record Command(int Id) : IRequest<Result>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var @event = await ApplicationDbContext.Events.SingleOrDefaultAsync(x => x.Id == command.Id);
                if (@event == null)
                    throw new Exception("No event found.");
                @event.IsDeleted = true;
                @event.DeletedDate = DateTime.Now;
                ApplicationDbContext.Events.Update(@event);
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
