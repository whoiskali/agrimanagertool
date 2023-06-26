using Application.Interfaces;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Polylines.Commands
{
    public static class AddEditPolylines
    {
        public record Command(int Id, int LandId, double Lng, double Lat, int Arrangement) : IRequest<Result>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var land = await ApplicationDbContext.Lands.SingleOrDefaultAsync(x => x.Id == command.LandId);
                var entity = new Polyline();
                if (command.Id == 0)
                    entity = command.Adapt<Polyline>();
                else
                {
                    var poly = await ApplicationDbContext.Polylines.SingleOrDefaultAsync(x => x.Id == command.Id);
                    if (poly == null)
                        throw new Exception("No polylines found");
                    entity = command.Adapt(poly);
                }
                if (land != null)
                    entity.Land = land;
                ApplicationDbContext.Polylines.Update(entity);
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
