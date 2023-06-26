using Domain.Entities;
using Domain.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Land : EntityModel, IEntityModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public User? AssignedUser { get; set; }
        public LandCategory Category { get; set; }
        public ICollection<Polyline> Polylines { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
