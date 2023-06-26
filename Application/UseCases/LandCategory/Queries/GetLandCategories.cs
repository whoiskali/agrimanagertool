using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.LandCategory.Queries
{
    public static class GetLandCategories
    {
        public record Query() : IRequest<List<Domain.Entities.LandCategory>>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, List<Domain.Entities.LandCategory>>
        {
            public async Task<List<Domain.Entities.LandCategory>> Handle(Query query, CancellationToken cancellationToken)
            {
                
                return await ApplicationDbContext.LandCategories.Include(x => x.Lands).Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();
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
