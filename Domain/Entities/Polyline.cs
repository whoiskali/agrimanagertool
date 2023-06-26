using Domain.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Polyline : EntityModel, IEntityModel
    {
        public int Id { get; set; }
        public Land Land { get; set; }
        public int Arrangement { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
    }
}
