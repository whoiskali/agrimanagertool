using Application.Interfaces;
using Domain.Enumerables;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.LandCategory.Queries
{
    public static class GetLandCategoryById
    {
        public record Query(int Id) : IRequest<Domain.Entities.LandCategory>;
        public record Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
        };
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, Domain.Entities.LandCategory>
        {
            public async Task<Domain.Entities.LandCategory> Handle(Query query, CancellationToken cancellationToken)
            {
                var data = await ApplicationDbContext.LandCategories.Include(x => x.Lands).Where(x => x.Id == query.Id && !x.IsDeleted).SingleOrDefaultAsync();
                if (data == null)
                    throw new Exception("No land category found.");
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
