using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.User.Queries
{
    public static class GetUsers
    {
        public record Query() : IRequest<List<Domain.Entities.User>>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, List<Domain.Entities.User>>
        {
            public async Task<List<Domain.Entities.User>> Handle(Query query, CancellationToken cancellationToken)
            {
                return await ApplicationDbContext.Users.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
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
