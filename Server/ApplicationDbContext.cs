using System;
using Microsoft.EntityFrameworkCore;
using BlazorCrud.Shared.Models;

namespace BlazorCrud.Server
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Person>? People { get; set; }
    }
}

