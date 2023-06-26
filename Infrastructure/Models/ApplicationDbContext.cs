using Application.Interfaces;
using Domain.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class ApplicationDbContext: DbContext, IApplicationDbContext
    {
        readonly ClaimsPrincipal claimsPrincipal;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ClaimsPrincipal claimsPrincipal = null) :
            base(options)
        {
            if (claimsPrincipal != null) this.claimsPrincipal = claimsPrincipal;
        }

        public async Task<bool> DuplicateUserNameCheckerAsync(string username)
        {
            var _username = await this.Users.Where(x => x.Username == username && !x.IsDeleted).ToListAsync();
            if (_username.Count() > 0) return true;
            return false;
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Land> Lands { get; set; }
        public DbSet<Polyline> Polylines { get; set; }
        public DbSet<LandCategory> LandCategories { get; set; }
    }
}
