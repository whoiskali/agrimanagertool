using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Polylines.Queries
{
    public static class GetPolylinesById
    {
        public record Query(int Id) : IRequest<Domain.Entities.Polyline>;
        public record Result;
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, Domain.Entities.Polyline>
        {
            public async Task<Domain.Entities.Polyline> Handle(Query query, CancellationToken cancellationToken)
            {
                var data = await ApplicationDbContext.Polylines.Where(x => x.Id == query.Id && !x.IsDeleted).SingleOrDefaultAsync();
                if (data == null)
                    throw new Exception("No polylines found.");
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
