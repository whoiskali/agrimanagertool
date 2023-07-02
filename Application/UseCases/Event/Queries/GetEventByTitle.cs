using Application.Interfaces;
using Domain.Enumerables;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries
{
    public static class GetEventByTitle
    {
        public record Query(string Title) : IRequest<Domain.Entities.Event>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, Domain.Entities.Event>
        {
            public async Task<Domain.Entities.Event> Handle(Query query, CancellationToken cancellationToken)
            {
                var data = await ApplicationDbContext.Events.Include(x => x.Land).Where(x => x.Title == query.Title && !x.IsDeleted).SingleOrDefaultAsync();
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
