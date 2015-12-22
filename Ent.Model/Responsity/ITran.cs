using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Responsity
{
    public interface ITran
    {
        void Begin();

        void Commit();

        void RollBack();
    }
}
