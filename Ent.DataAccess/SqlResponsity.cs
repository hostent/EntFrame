using Ent.Common;
using Ent.Common.Thread;
using Ent.Model.Responsity;
using Ent.Model.Untility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.DataAccess
{
    public class SqlResponsity<TDb> : IResponsity where TDb : BaseDBContext
    {

        #region IResponsity
        public T Add<T>(T t) where T : BaseTable
        {
            if (typeof(ITable).IsAssignableFrom(typeof(T)))
            {
                var table = (BaseTable)t;

                table.Id = IDGenerator.NewID();

                table.CreateDate = DateTime.Now;
                table.UpdateDate = DateTime.Now;
                table.CreateBy = GetLoginUserName();
                table.UpdateBy = GetLoginUserName();
                
            }

            var result = _db.Set<T>().Add(t);
            _db.SaveChanges();
            return result;

        }

        public IList<T> Add<T>(IList<T> list) where T : BaseTable
        {
            if (typeof(ITable).IsAssignableFrom(typeof(T)))
            {
                foreach (var t in list)
                {
                    var table = (ITable)t;
                    table.Id = IDGenerator.NewID();

                    table.CreateDate = DateTime.Now;
                    table.UpdateDate = DateTime.Now;
                    table.CreateBy = GetLoginUserName();
                    table.UpdateBy = GetLoginUserName();
                }
            }

            var result = _db.Set<T>().AddRange(list);
            _db.SaveChanges();
            return result.ToList();
        }

        public int Delete<T>(string id) where T : BaseTable, new()
        {
            T tEntity = new T();
            var entity = _db.Entry<T>(tEntity);
            string idName = "ID";
            entity.Property(idName).CurrentValue = id;
            entity.State = System.Data.Entity.EntityState.Deleted;
            return _db.SaveChanges();
        }

        public int Delete<T>(IList<string> ids) where T : BaseTable, new()
        {
            throw new NotImplementedException();
        }


        public int Update<T>(T t) where T : BaseTable, new()
        {
            if (typeof(ITable).IsAssignableFrom(typeof(T)))
            {
                var table = (ITable)t;

                table.UpdateDate = DateTime.Now;
                var userName = GetLoginUserName();
                if (!string.IsNullOrEmpty(userName))
                {
                    table.UpdateBy = userName;
                }                
            }

            T tEntity = t;
            var entity = _db.Entry<T>(tEntity);
            entity.State = System.Data.Entity.EntityState.Modified;

            return _db.SaveChanges();
        }



        public IQueryable<T> Query<T>() where T : BaseTable
        {
            DbSet<T> dbset = _db.Set<T>();

            return dbset.AsNoTracking().AsQueryable();
        }

        public IList<T> Query<T>(string reportName, IDictionary<string, object> par)
        {

            int totalCount = 0;
            return ComplexSqlHelp.GetReportData<T>(_db, reportName, 0, 0, "", par, false, out totalCount);


        }

        public DataPage<T> QueryPage<T>(string reportName, PagePars param) where T : Model.Untility.IPageModel
        {
            DataPage<T> result = new DataPage<T>();
            int totalCount = 0;
            result.CurrentPage = ComplexSqlHelp.GetReportData<T>(_db, reportName, param.PageSize, param.PageIndex, param.Order, param.Where, true, out totalCount);
            result.TotalCounts = totalCount;
            return result;
        }


        #endregion


        #region ITran

        public void Begin()
        {
            if (string.IsNullOrEmpty(Ent.Common.Thread.ThreadContext.TranTag))
            {
                Ent.Common.Thread.ThreadContext.TranTag = Guid.NewGuid().ToString("N");
            }
        }


        public void Commit()
        {
            foreach (var item in Ent.Common.Thread.ThreadContext.DbContexts)
            {
                var dbItem = ((BaseDBContext)item.Value);
                if (dbItem.Tran != null)
                {
                    dbItem.Tran.Commit();
                    dbItem.Connection.Close();
                }
            }


            Ent.Common.Thread.ThreadContext.TranTag = string.Empty;

        }

        public void RollBack()
        {
            foreach (var item in Ent.Common.Thread.ThreadContext.DbContexts)
            {
                var dbItem = ((BaseDBContext)item.Value);
                if (dbItem.Tran != null)
                {
                    dbItem.Tran.Rollback();
                    dbItem.Connection.Close();
                }
            }


            Ent.Common.Thread.ThreadContext.TranTag = string.Empty;


        }



        #endregion

        private TDb _db { get; set; }

        public SqlResponsity(TDb db)
        {
            if (_db == null)
            {
                _db = db;
                _db.Configuration.ValidateOnSaveEnabled = false;
            }
        }


        private string GetLoginUserName()
        {
            
            return "";
        }


 


        public int Update<T>(IList<T> list) where T : BaseTable, new()
        {
            throw new NotImplementedException();
        }
    }
}
