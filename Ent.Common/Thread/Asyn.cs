using Ent.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ent.Common.Thread
{

    public class Asyn
    {

        public static void Invork(Action act)
        {
            var result = act.BeginInvoke(null, null);

            try
            {
                act.EndInvoke(result);

            }
            catch (Exception e)
            {
                //log
                LogHelp.Default.Error(e);

            }
        }
    }
}
