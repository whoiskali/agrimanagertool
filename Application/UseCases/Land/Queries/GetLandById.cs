using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Land.Queries
{
    public static class GetLandById
    {
        public record Query(int Id) : IRequest<Domain.Entities.Land>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, Domain.Entities.Land>
        {
            public async Task<Domain.Entities.Land> Handle(Query query, CancellationToken cancellationToken)
            {
                var data = await ApplicationDbContext.Lands.Include(x => x.Category).Include(x => x.Polylines).Where(x => x.Id == query.Id && !x.IsDeleted).SingleOrDefaultAsync();
                if (data == null)
                    throw new Exception("No land found.");
                return data;
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
