using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reversi.Models;

namespace Reversi.Data
{
    public class ReversiContext : IdentityDbContext<Speler>
    {
        public ReversiContext(DbContextOptions<ReversiContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Speler> Spelers { get; set; }
        public DbSet<Spel> Spellen { get; set; }
        public DbSet<SpelSpeler> SpelSpelers { get; set; }
        public DbSet<Coordinate> Coordinates { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
