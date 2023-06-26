using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Polylines.Queries
{
    public static class GetPolylines
    {
        public record Query() : IRequest<List<Domain.Entities.Polyline>>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, List<Domain.Entities.Polyline>>
        {
            public async Task<List<Domain.Entities.Polyline>> Handle(Query query, CancellationToken cancellationToken)
            {

                return await ApplicationDbContext.Polylines.Where(x => !x.IsDeleted).OrderBy(x=>x.Arrangement).ToListAsync();
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
