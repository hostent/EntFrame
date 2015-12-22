using Ent.Model.Untility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Colection
{
    public class U_UserInfo : BaseTable
    {

        public string UserID { get; set; }

        public string Tag { get; set; }

        public List<Firend> FirendList { get; set; }


        public Car FirstCar { get; set; }

         
    }

    public class Firend
    {
        public string Name { get; set; }

        public string Address { get; set; }


        public int Point { get; set; }

        public DateTime Birday { get; set; }

       public List<Car> CarList { get; set; }

    }

    public class Car
    {
        public string BName { get; set; }

         public decimal Runkm { get; set; }
    }



}
