using Ent.Model.Colection;
using Ent.Model.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Ent.Service.User
{
    public class CUserInfo : IUserInfo
    {
        public U_User Get(string id)
        {

            //var list = _Db.MongoDb.First<U_User>(q =>true);


            //var list = _Db.MongoDb.Query<U_User>(q => q.Name.Contains("某某11"));


            //var list = _Db.MongoDb.Query<U_UserInfo>(q => q.FirendList[0].CarList[0].Runkm>555);


             

            return null;


        }

        public void Save(U_User user)
        {
            #region demo

            IList<U_User> list = new List<U_User>();

            list.Add(new U_User()
            {
                CreateDate = DateTime.Now,
                NickName = "811112"
            });
            list.Add(new U_User()
            {
                CreateDate = DateTime.Now,
                NickName = "811113"
            });
            list.Add(new U_User()
            {
                CreateDate = DateTime.Now,
                NickName = "811114"
            });
            list.Add(new U_User()
            {
                CreateDate = DateTime.Now,
                NickName = "811115"
            });
            list.Add(new U_User()
            {
                CreateDate = DateTime.Now,
                NickName = "811116"
            });
            _Db.MongoDb.Add(list);



            //var list = new List<string>();
            //list.Add("566a3611049ee61bdc394a62");
            //list.Add("566a3611049ee61bdc394a61");
            //_Db.MongoDb.Delete<U_User>(list);


            //var userUp = _Db.MongoDb.First<U_User>(q => q._id == "566a3611049ee61bdc394a63");

            //userUp.UpdateDate = DateTime.Now;
            //userUp.Name = "shit,update";

            //_Db.MongoDb.Update<U_User>(userUp);

            #endregion

            #region demo 性能
            //var t1 = new Thread(new ThreadStart(test));
            //var t2 = new Thread(new ThreadStart(test));
            //var t3 = new Thread(new ThreadStart(test));
            //var t4 = new Thread(new ThreadStart(test));
            //var t5 = new Thread(new ThreadStart(test));
            //var t6 = new Thread(new ThreadStart(test));
            //var t7 = new Thread(new ThreadStart(test));

            //t1.Start();
            //t2.Start();
            //t3.Start();
            //t4.Start();
            //t5.Start();
            //t6.Start();
            //t7.Start();

            #endregion

            //U_UserInfo info = new U_UserInfo()
            //{
            //    Tag = "0002",
            //    UserID = "566a622c049eec1bdc7fac11",
            //    FirstCar = new Car() { BName = "卡宴1", Runkm = (decimal)555555.56 },
            //    FirendList = new List<Firend>()
            //    {
            //        new Firend()
            //        {  
            //            Address = "广州1",
            //            Birday = new DateTime(1985, 5, 5),
            //            Name = "东邪1",
            //            Point = 1254,
            //            CarList = new List<Car>()
            //            {
            //                 new Car() { BName = "卡罗拉1", Runkm = (decimal)4545 },
            //            }
            //        },
            //        new Firend()
            //        {
            //            Address = "上海1",
            //            Birday = new DateTime(1986, 5, 5),
            //            Name = "西毒1",
            //            Point = 556,
            //            CarList = new List<Car>()
            //            {
            //                 new Car() { BName = "凯美瑞1", Runkm = (decimal)852 },
            //                 new Car() { BName = "福克斯1", Runkm = (decimal)85569}
            //            }
                         
            //        }                    
                     
            //    },
            //};

            //_Db.MongoDb.Add(info);

        }

        public void test()
        {
            for (int i = 0; i < 100000; i++)
            {
                U_User u = new U_User();

                u.NickName = "某某" + Thread.CurrentContext.ContextID.ToString() + i.ToString();

                _Db.MongoDb.Add<U_User>(u);
            }

        }
    }
}
