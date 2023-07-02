using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries
{
    public static class GetEvents
    {
        public record Query(bool? IsPublic, DateTime? Schedule, bool? IsLatest) : IRequest<List<Domain.Entities.Event>>;
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<Query, List<Domain.Entities.Event>>
        {
            public async Task<List<Domain.Entities.Event>> Handle(Query query, CancellationToken cancellationToken)
            {
                Expression<Func<Domain.Entities.Event, bool>> exp = x => ( query.IsPublic != null ? x.IsPublic == query.IsPublic : true) && ( query.Schedule != null ? x.Schedule.Date == query.Schedule.Value.Date : true) && !x.IsDeleted && (query.IsLatest != null ? x.Schedule > DateTime.Now : true);
               
                return await ApplicationDbContext.Events.Include(x => x.Land).Where(exp).OrderBy(x => x.Schedule).ToListAsync();
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
