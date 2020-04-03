using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VK1.SCGExpress.Models;
using VK1.SCGExpress.Services.Data;

namespace VK1.SCGExpress.Services {
    public class AppQuery {
        private readonly AppQueryDb db;

        public AppQuery(AppQueryDb db) {
            this.db = db;
        }

        public async Task<List<Location>> LocationsAsync<T>(string sql) => (await db.QuerySqlAsync<Location>(sql)).ToList();

        public async Task<List<AlphaBookingViewModel>> AlphaBookingViewModelsAsync<T>(string sql) => (await db.QuerySqlAsync<AlphaBookingViewModel>(sql)).ToList();
    }
}
