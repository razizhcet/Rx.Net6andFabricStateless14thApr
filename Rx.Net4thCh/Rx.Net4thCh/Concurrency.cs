using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rx.Net4thCh
{
    class Concurrency
    {
        static void Example1()
        {
            Console.WriteLine("Starting on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            var subject = new Subject<object>();
            subject.Subscribe(
            o => Console.WriteLine("Received {1} on threadId:{0}",
            Thread.CurrentThread.ManagedThreadId,
            o));
            ParameterizedThreadStart notify = obj =>
            {
                Console.WriteLine("OnNext({1}) on threadId:{0}",
                Thread.CurrentThread.ManagedThreadId,
                obj);
                subject.OnNext(obj);
            };
            notify(1);
            new Thread(notify).Start(2);
            new Thread(notify).Start(3);
        }
        static void Example2()
        {
            Console.WriteLine("Starting on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            var source = Observable.Create<int>(
            o =>
            {
                Console.WriteLine("Invoked on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
                o.OnNext(1);
                o.OnNext(2);
                o.OnNext(3);
                o.OnCompleted();
                Console.WriteLine("Finished on threadId:{0}",
                 Thread.CurrentThread.ManagedThreadId);
                return Disposable.Empty;
            });
            source
            .Subscribe(
            o => Console.WriteLine("Received {1} on threadId:{0}",
            Thread.CurrentThread.ManagedThreadId,
            o),
            () => Console.WriteLine("OnCompleted on threadId:{0}",
            Thread.CurrentThread.ManagedThreadId));
            Console.WriteLine("Subscribed on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }
        static void Example3()
        {

            Console.WriteLine("Starting on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            var source = Observable.Create<int>(
            o =>
            {
                Console.WriteLine("Invoked on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
                o.OnNext(1);
                o.OnNext(2);
                o.OnNext(3);
                o.OnCompleted();
                Console.WriteLine("Finished on threadId:{0}",
                 Thread.CurrentThread.ManagedThreadId);
                return Disposable.Empty;
            });
            source
            .SubscribeOn(Scheduler.ThreadPool)
            .Subscribe(
            o => Console.WriteLine("Received {1} on threadId:{0}",
            Thread.CurrentThread.ManagedThreadId,
            o),
            () => Console.WriteLine("OnCompleted on threadId:{0}",
            Thread.CurrentThread.ManagedThreadId));
            Console.WriteLine("Subscribed on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }
        static void Example4()
        {
            var delay = TimeSpan.FromSeconds(1);
            Console.WriteLine("Before schedule at {0:o}", DateTime.Now);
            Scheduler.NewThread.Schedule(delay,
            () => Console.WriteLine("Inside schedule at {0:o}", DateTime.Now));
            Console.WriteLine("After schedule at  {0:o}", DateTime.Now);
        }

        public static IDisposable Work(IScheduler scheduler, List<int> list)
        {
            var tokenSource = new CancellationTokenSource();
            var cancelToken = tokenSource.Token;
            var task = new Task(() =>
            {
                Console.WriteLine();
                for (int i = 0; i < 1000; i++)
                {
                    var sw = new SpinWait();
                    for (int j = 0; j < 3000; j++) sw.SpinOnce();
                    Console.Write(".");
                    list.Add(i);
                    if (cancelToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancelation requested");
                        //cancelToken.ThrowIfCancellationRequested();
                        return;
                    }
                }
            }, cancelToken);
            task.Start();
            return Disposable.Create(tokenSource.Cancel);
        }
        static void Example5()
        {
            var list = new List<int>();
            Console.WriteLine("Enter to quit:");
            var token = Scheduler.NewThread.Schedule(list, Work);
            Console.ReadLine();
            Console.WriteLine("Cancelling...");
            token.Dispose();
            Console.WriteLine("Cancelled");
            Console.ReadKey();
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

        static void Example6()
        {
            ScheduleTasks(Scheduler.CurrentThread);
            Console.WriteLine("===================");
            ScheduleTasks(Scheduler.Immediate);
            Console.ReadKey();
        }

        private static IDisposable OuterAction(IScheduler scheduler, string state)
        {
            Console.WriteLine("{0} start. ThreadId:{1}",
            state,
            Thread.CurrentThread.ManagedThreadId);
            scheduler.Schedule(state + ".inner", InnerAction);
            Console.WriteLine("{0} end. ThreadId:{1}",
            state,
            Thread.CurrentThread.ManagedThreadId);
            return Disposable.Empty;
        }
        private static IDisposable InnerAction(IScheduler scheduler, string state)
        {
            Console.WriteLine("{0} start. ThreadId:{1}",
            state,
            Thread.CurrentThread.ManagedThreadId);
            scheduler.Schedule(state + ".Leaf", LeafAction);
            Console.WriteLine("{0} end. ThreadId:{1}",
            state,
            Thread.CurrentThread.ManagedThreadId);
            return Disposable.Empty;
        }
        private static IDisposable LeafAction(IScheduler scheduler, string state)
        {
            Console.WriteLine("{0}. ThreadId:{1}",
            state,
            Thread.CurrentThread.ManagedThreadId);
            return Disposable.Empty;
        }

        static void Example7()
        {
            Console.WriteLine("Starting on thread :{0}",
            Thread.CurrentThread.ManagedThreadId);
            Scheduler.NewThread.Schedule("A", OuterAction);
            Console.ReadKey();

        }

        static void Example8()
        {
            Console.WriteLine("Starting on thread :{0}",
           Thread.CurrentThread.ManagedThreadId);
            Scheduler.NewThread.Schedule("A", OuterAction);
            Scheduler.NewThread.Schedule("B", OuterAction);
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            //Example1();
            Console.WriteLine("=======================");
            //Example2();
            Console.WriteLine("=======================");
            //Example3();
            Console.WriteLine("=======================");
            //Example4();
            Console.WriteLine("=======================");
            //Example5();
            Console.WriteLine("=======================");
            //Example6();
            Console.WriteLine("=======================");
            //Example7();
            Console.WriteLine("=======================");
            Example8();
        }
    }


}
