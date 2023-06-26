using Application.Interfaces;
using Domain.Entities;
using Domain.Enumerables;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.LandCategory.Commands
{
    public static class AddEditLandCategory
    {
        public record Command(int Id, string Name, string Color) : IRequest<Result>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                    var data = new Domain.Entities.LandCategory();
                    if (command.Id == 0)
                        data = command.Adapt<Domain.Entities.LandCategory>();
                    else
                    {
                        var @event = await ApplicationDbContext.LandCategories.SingleOrDefaultAsync(x => x.Id == command.Id);
                        if (@event == null)
                            throw new Exception("No category found.");
                        data = command.Adapt(@event);
                    }
                    ApplicationDbContext.LandCategories.Update(data);
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
