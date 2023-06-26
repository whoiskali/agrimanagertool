using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Land.Queries
{
    public class GetNearestLandQuery : IRequest<List<Domain.Entities.Land>>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public static class GetNearestLand
    {
        public record Result();
        public record Handler(IApplicationDbContext ApplicationDbContext) : IRequestHandler<GetNearestLandQuery, List<Domain.Entities.Land>>
        {
            public async Task<List<Domain.Entities.Land>> Handle(GetNearestLandQuery query, CancellationToken cancellationToken)
            {
                double minDistance = double.MaxValue;

                var lands = await ApplicationDbContext.Lands.Include(x => x.Category).ToListAsync();

                CoordinateDistanceComparer comparer = new CoordinateDistanceComparer(query);
                List<Domain.Entities.Land> sortedCoordinates = lands.OrderBy(c => new GetNearestLandQuery { Latitude = c.Lat, Longitude = c.Lng}, comparer).ToList();
                return sortedCoordinates;
            }
        }
        public class Error : Exception
        {
            public Error(string message) : base(message)
            {

            }
        }
    }


    public class CoordinateDistanceComparer : IComparer<GetNearestLandQuery>
    {
        private double CalculateDistance(GetNearestLandQuery coord1, GetNearestLandQuery coord2)
        {
            const double earthRadius = 6371; // in kilometers

            double lat1Rad = Math.PI * coord1.Latitude / 180;
            double lon1Rad = Math.PI * coord1.Longitude / 180;
            double lat2Rad = Math.PI * coord2.Latitude / 180;
            double lon2Rad = Math.PI * coord2.Longitude / 180;

            double deltaLat = lat2Rad - lat1Rad;
            double deltaLon = lon2Rad - lon1Rad;

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = earthRadius * c;
            return distance;
        }

        private readonly GetNearestLandQuery _target;

        public CoordinateDistanceComparer(GetNearestLandQuery target)
        {
            _target = target;
        }

        public int Compare(GetNearestLandQuery x, GetNearestLandQuery y)
        {
            double distanceX = CalculateDistance(_target, x);
            double distanceY = CalculateDistance(_target, y);

            return distanceX.CompareTo(distanceY);
        }
    }
}
