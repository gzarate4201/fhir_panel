using System;
using System.Collections.Generic;
using System.Text;
using AspStudio.Models;
using Microsoft.EntityFrameworkCore;

namespace AspStudio.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
