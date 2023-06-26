using Application.Interfaces;
using Domain.Enumerables;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity.Migrations.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Event.Queries
{
    public static class GetEventPerMonth
    {
        public record Query(DateTime DateTime) : IRequest<List<Result>>;
        public record Result
        {
            public string EventName { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }
            public EventCategory Category { get; set; }
        };
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, List<Result>>
        {
            public async Task<List<Result>> Handle(Query query, CancellationToken cancellationToken)
            {
                var result = await ApplicationDbContext.Events.Where(x => x.Schedule.Year == query.DateTime.Year).Select(x => new Result
                {
                    Category = x.Category,
                    EventName = x.Title,
                    From = x.Schedule,
                    To = x.Schedule
                }).ToListAsync();
                return result;
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
