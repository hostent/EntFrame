using Ent.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ent.Common.Thread
{

    public class Job
    {


        public delegate void FunHandle(object par);

        delegate void FunHandleAsyn(FunHandle funHandle, object par, string identify, EventStartHander eventStartHander);

        public delegate void EventStartHander(string identify, object input);
        public delegate void EventEndHander(string identify, object input, object output);
        public delegate void ExceptionHander(string identify, object input, Exception ex);


 

        public static void Run(FunHandle funHandle, object par, string jobName)
        {
            string identify = "Thread.Asyn.Jobs." + jobName;
            FunHandleAsyn funHandleAsyn = Do;

            RunAsynObject runAsynObject = new RunAsynObject();
            runAsynObject.Identify = identify;
            runAsynObject.HandleAsyn = funHandleAsyn;
            runAsynObject.Par = par;
            runAsynObject.Handle = funHandle;

            //// 同步上下文
            //ThreadObject threadObject = ServiceContext.ThreadObject;
            //threadObject.StartTime = DateTime.Now;
            //ServiceContext.SaveThreadObject(identify, threadObject);

            funHandleAsyn.BeginInvoke(funHandle, par, identify, null, OnComplete, runAsynObject);
        }



        static void Do(FunHandle fun, object par, string identify, EventStartHander eventStartHander)
        {
            System.Threading.Thread.CurrentThread.Name = identify;

            Thread.ThreadContext.RunningState = true;

            if (eventStartHander != null)
            {
                eventStartHander(identify, par);
            }
            fun(par);

            Thread.ThreadContext.RunningState = false;
        }


        static void OnComplete(IAsyncResult asyncResult)
        {

            
 
        }

        class RunAsynObject
        {
            public string Identify { get; set; }

            public FunHandleAsyn HandleAsyn { get; set; }

            public FunHandle Handle { get; set; }

            public object Par { get; set; }

            public EventStartHander StartHander { get; set; }

            public EventEndHander EndHander { get; set; }

            public ExceptionHander ExceptionHander { get; set; }


        }
    }
}
