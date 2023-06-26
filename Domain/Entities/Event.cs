using Domain.Enumerables;
using Domain.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Event : EntityModel, IEntityModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EventCategory Category { get; set; }
        public DateTime Schedule { get; set; }
        public Land? Land  { get; set; }
        public bool IsPublic { get; set; }
    }
}
