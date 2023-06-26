using Domain.Enumerables;
using Domain.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : EntityModel, IEntityModel
    {
        public int Id { get; set; }
        public UserStatus Status { get; set; }
        public string Name { get; set; }
        public Usertype Type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
