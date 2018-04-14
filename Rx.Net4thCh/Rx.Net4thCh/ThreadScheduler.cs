using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.Net4thCh
{
    
    class ThreadScheduler
    {
        

        static void Main(string[] args)
        {
            //CurrentThreadExample();
            ImmediateExample();
            Console.ReadKey();
        }

        static void CurrentThreadExample()
        {
            ScheduleTasks(Scheduler.CurrentThread);
        }
        static void ImmediateExample()
        {
            ScheduleTasks(Scheduler.Immediate);
        }

        private static void ScheduleTasks(IScheduler scheduler)
        {
            Action leafAction = () => Console.WriteLine("----leafAction.");
            Action innerAction = () =>
            {
                Console.WriteLine("--innerAction start.");
                scheduler.Schedule(leafAction);
                Console.WriteLine("--innerAction end.");
            };
            Action outerAction = () =>
            {
                Console.WriteLine("outer start.");
                scheduler.Schedule(innerAction);
                Console.WriteLine("outer end.");
            };
            scheduler.Schedule(outerAction);
        }

       
        
    }
}
