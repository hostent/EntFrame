using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.DataAccess
{
    public class BaseDBContext : DbContext
    {
        public BaseDBContext(string conn)
            : base(conn)
        {
            Database.SetInitializer<BaseDBContext>(null);
            //base.Configuration.ValidateOnSaveEnabled = false;
            //base.Configuration.LazyLoadingEnabled = false;
        }

        public DbConnection Connection
        {
            get
            {
                var objectContext = ((IObjectContextAdapter)this).ObjectContext;
                DbConnection con = objectContext.Connection;
                return con;
            }
        }

        public DbTransaction Tran { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();

            var ChangeTrackerData = base.ChangeTracker.Entries().Where(q => q.State == System.Data.Entity.EntityState.Unchanged).ToList();
            foreach (var item in ChangeTrackerData)
            {
                item.State = System.Data.Entity.EntityState.Detached;
            }
            return result;
        }

    }
}
