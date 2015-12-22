using Ent.Common.Log;
using Ent.Common.Thread;
using Ent.Model.Responsity;
using Ent.Model.Untility;
using MongoDB;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ent.DataAccess
{
    public class MongoResponsity : IResponsity
    {
        private IMongoDatabase _db { get; set; }

        private string Prefix { get; set; }

        public MongoResponsity(IMongoDatabase db, string prefix)
        {
            if (_db == null)
            {
                _db = db;
                Prefix = prefix;
            }
        }

        private void CheckPrefix<T>()
        {
            if ((typeof(T).Name.Split('_').First() != Prefix))
            {
                throw new Exception("不同模块的表不能调用，只能走接口");
            }

        }

        #region IResponsity
        public T Add<T>(T t) where T : BaseTable
        {
            CheckPrefix<T>();

            ((BaseTable)t).Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            ((BaseTable)t).CreateDate = DateTime.Now;

            ((BaseTable)t).CreateBy = LogHelp.OpUserName == null ? "" : LogHelp.OpUserName();
            


            _db.GetCollection<T>(t.GetType().Name).InsertOne(t);

            return t;
        }

        public IList<T> Add<T>(IList<T> list) where T : BaseTable
        {
            CheckPrefix<T>();
            foreach (var item in list)
            {
                ((BaseTable)item).Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
                ((BaseTable)item).CreateDate = DateTime.Now;
                ((BaseTable)item).CreateBy = LogHelp.OpUserID == null ? "" : LogHelp.OpUserID();
            }
            _db.GetCollection<T>(typeof(T).Name).InsertMany(list);
            return list;
        }

        public int Delete<T>(string id) where T : BaseTable, new()
        {
            CheckPrefix<T>();

            var result = _db.GetCollection<T>(typeof(T).Name).DeleteOne(q => q.Id == id);
            return (int)result.DeletedCount;
        }

        public int Delete<T>(IList<string> ids) where T : BaseTable, new()
        {
            CheckPrefix<T>();
            var result = _db.GetCollection<T>(typeof(T).Name).DeleteMany(q => ids.Contains(q.Id));
            return (int)result.DeletedCount;
        }

        public int Update<T>(T t) where T : BaseTable, new()
        {
            CheckPrefix<T>();

            ((BaseTable)t).UpdateDate = DateTime.Now;
            ((BaseTable)t).UpdateBy = LogHelp.OpUserID == null ? "" : LogHelp.OpUserName();

            var result = _db.GetCollection<T>(typeof(T).Name).ReplaceOne(q => q.Id == t.Id, t);
            return (int)result.MatchedCount;
        }

        public int Update<T>(IList<T> list) where T : BaseTable, new()
        {
            CheckPrefix<T>();

            foreach (var item in list)
            {
                ((BaseTable)item).UpdateDate = DateTime.Now;
                ((BaseTable)item).UpdateBy = LogHelp.OpUserID == null ? "" : LogHelp.OpUserName();

                _db.GetCollection<T>(typeof(T).Name).ReplaceOne(q => q.Id == item.Id, item);
            }
            return list.Count;

        }
        #endregion

        #region IQuery

        public T First<T>(System.Linq.Expressions.Expression<Func<T, bool>> where) where T : BaseTable
        {
            CheckPrefix<T>();

            var list = _db.GetCollection<T>(typeof(T).Name).Find<T>(where).ToList();
            if (list.Count == 0)
            {
                return null;
            }
            return list.First();
        }

        public IList<T> Query<T>(Expression<Func<T, bool>> where) where T : BaseTable
        {
            CheckPrefix<T>();
            return _db.GetCollection<T>(typeof(T).Name).Find(where).ToList();

            _db.GetCollection<T>(typeof(T).Name).AsQueryable();


        }

        public IList<T> QuerySortBy<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> sort) where T : BaseTable
        {
            CheckPrefix<T>();

            return _db.GetCollection<T>(typeof(T).Name).Find(where).SortBy(sort).ToList();
        }

        public IList<T> QuerySortByDescending<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> sort) where T : BaseTable
        {
            CheckPrefix<T>();

            return _db.GetCollection<T>(typeof(T).Name).Find(where).SortByDescending(sort).ToList();
        }

 


        public IQueryable<T> Query<T>() where T : BaseTable
        {
            CheckPrefix<T>();
            return _db.GetCollection<T>(typeof(T).Name).AsQueryable();
        }


        public IList<T> QueryPage<T>(PagePars<T> param) where T : BaseTable
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);

            IFindFluent<T, T> result = null;

            if (param.Where != null)
            {
                result = collection.Find(param.Where);
            }
            if (param.SortBys != null && param.SortBys.Count() > 0)
            {
                foreach (var item in param.SortBys)
                {
                    result = result.SortBy(item);
                }
            }
            if (param.SortByDescendings != null && param.SortByDescendings.Count() > 0)
            {
                foreach (var item in param.SortBys)
                {
                    result = result.SortByDescending(item);
                }
            }
            if (param.Skip != null)
            {
                result = result.Skip(param.Skip);
            }
            if (param.Limit != null)
            {
                result = result.Limit(param.Skip);
            }
            return result.ToList();


        }


        public IList<T> Query<T>(string reportName, IDictionary<string, object> par)
        {
            CheckPrefix<T>();
            // string strquery = 

            //_db.GetCollection<T>(typeof(T).Name).Find()


            throw new NotImplementedException();
        }
        public DataPage<T> QueryPage<T>(string reportName, PagePars param) where T : IPageModel
        {
            CheckPrefix<T>();
            throw new NotImplementedException();
        }

        public IList<T> QuerySql<T>(string sql, IDictionary<string, object> par)
        {
            CheckPrefix<T>();
            throw new NotImplementedException();
        }


        #endregion


        #region ITram
        public void Begin()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        #endregion





    }


}
