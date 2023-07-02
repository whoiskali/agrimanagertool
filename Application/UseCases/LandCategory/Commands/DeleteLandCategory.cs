using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.LandCategory.Commands
{
    public static class DeleteLandCategory
    {
        public record Command(int Id) : IRequest<Result>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var @LandCategory = await ApplicationDbContext.LandCategories.SingleOrDefaultAsync(x => x.Id == command.Id);
                if (@LandCategory == null)
                    throw new Exception("No LandCategory found.");
                @LandCategory.IsDeleted = true;
                @LandCategory.DeletedDate = DateTime.Now;
                ApplicationDbContext.LandCategories.Update(@LandCategory);
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
