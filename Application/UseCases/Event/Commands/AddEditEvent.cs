using Application.Interfaces;
using Domain.Enumerables;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Commands
{
    public static class AddEditEvent
    {
        public record Command(int Id, DateTime? Schedule, string? Title, string? Description, bool? IsPublic, EventCategory Category, int? LandId) : IRequest<Result>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                    var data = new Domain.Entities.Event();
                    if (command.Id == 0)
                    {
                        var e = await ApplicationDbContext.Events.Where(x => x.Title == command.Title).ToListAsync();
                        if (e.Count > 0)
                            throw new Exception("Title already exist");
                        data = command.Adapt<Domain.Entities.Event>();
                    }
                    else
                    {
                        var @event = await ApplicationDbContext.Events.SingleOrDefaultAsync(x => x.Id == command.Id);
                        if (@event == null)
                            throw new Exception("No");
                        data = command.Adapt(@event);
                    }
                    var land = await ApplicationDbContext.Lands.SingleOrDefaultAsync(x => x.Id == command.LandId);
                    if(land != null)
                        data.Land = land;
                    ApplicationDbContext.Events.Update(data);
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
