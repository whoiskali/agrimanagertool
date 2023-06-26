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
    public static class GetEventById
    {
        public record Query(int Id) : IRequest<Result>;
        public record Result
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime Schedule { get; set; }
            public Domain.Entities.Land? Land { get; set; }
            public bool IsPublic { get; set; }
        };
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, Result>
        {
            public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
            {
                var data = await ApplicationDbContext.Events.Include(x => x.Land).Where(x => x.Id == query.Id && !x.IsDeleted).Select(x => new Result
                {
                    Category = x.Category.ToString(),
                    Description = x.Description,
                    Id = x.Id,
                    IsPublic = x.IsPublic,
                    Land = x.Land,
                    Schedule = x.Schedule,
                    Title = x.Title
                }).SingleOrDefaultAsync();
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
