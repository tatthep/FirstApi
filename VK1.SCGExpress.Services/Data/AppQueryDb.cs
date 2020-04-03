using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VK1.SCGExpress.Models;

namespace VK1.SCGExpress.Services.Data {
    public class AppQueryDb : DbContext {
        public AppQueryDb() {
            //
        }

        public AppQueryDb(DbContextOptions<AppQueryDb> options):base(options) {
            //
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<AlphaBookingViewModel> AlphaBookingViewModels { get; set; }

        public async Task<IQueryable<T>> QuerySqlAsync<T>(string sql) where T : class {
            return await Task.FromResult(Set<T>().FromSqlRaw(sql));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>().HasNoKey();
            modelBuilder.Entity<AlphaBookingViewModel>().HasNoKey();
        }

    }
}
