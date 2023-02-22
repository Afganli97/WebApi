using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data.DAL
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }



        public override int SaveChanges()
        {
            var time = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in time)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedTime = DateTime.Now;
                }
                entry.Entity.LastModifiedTime = DateTime.Now;
            }
            return base.SaveChanges();
        }
    }
}