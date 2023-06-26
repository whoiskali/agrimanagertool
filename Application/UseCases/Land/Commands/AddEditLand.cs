using Application.Interfaces;
using Domain.Enumerables;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.UseCases.Polylines.Commands;

namespace Application.UseCases.Lands.Commands
{
    public static class AddEditLand
    {
        public record Command(int Id, int CategoryId, string Title, string Description, int UserId, double Lat, double Lng, List<AddEditPolylines.Command> Polylines) : IRequest<Result>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await ApplicationDbContext.Users.SingleOrDefaultAsync(x => x.Id == command.UserId);
                    var category = await ApplicationDbContext.LandCategories.SingleOrDefaultAsync(x => x.Id == command.CategoryId);
                    if (category == null)
                        throw new Exception("No category found");
                    var data = new Domain.Entities.Land(); 
                    if (command.Id == 0)
                        data = command.Adapt<Domain.Entities.Land>();
                    else
                    {
                        var @Land = await ApplicationDbContext.Lands.SingleOrDefaultAsync(x => x.Id == command.Id);
                        if (@Land == null)
                            throw new Exception("No");
                        data = command.Adapt(@Land);
                    }
                    data.AssignedUser = user;
                    data.Category = category;
                    ApplicationDbContext.Polylines.RemoveRange(ApplicationDbContext.Polylines.Where(x => x.Land == data));
                    ApplicationDbContext.Lands.Update(data);
                    await ApplicationDbContext.SaveChangesAsync();
                    data.Polylines = command.Polylines.Adapt<List<Polyline>>();
                    ApplicationDbContext.Lands.Update(data);
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
