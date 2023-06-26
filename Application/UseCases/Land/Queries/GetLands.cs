using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Land.Queries
{
    public static class GetLands
    {
        public record Query() : IRequest<List<Domain.Entities.Land>>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, List<Domain.Entities.Land>>
        {
            public async Task<List<Domain.Entities.Land>> Handle(Query query, CancellationToken cancellationToken)
            {

                return await ApplicationDbContext.Lands.AsNoTracking().Include(x => x.Polylines).Include(x => x.Category).Include(x => x.AssignedUser).Where(x => !x.IsDeleted).ToListAsync();
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
