using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ent.Model.Untility
{
    public interface ITable
    {

        string Id { get; set; }
        string CreateBy { get; set; }
        string UpdateBy { get; set; }
        DateTime? CreateDate { get; set; }
        DateTime? UpdateDate { get; set; }
    }

    public class BaseTable : ITable
    {

        public string Id { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
