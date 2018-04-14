using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rx.Net4thCh
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            //Define number provider 
            NumberGenerator NumberProvider = new NumberGenerator();

            //Have two observers.
            NumberObserver reporter1 = new NumberObserver("EvenObserver", true);
            reporter1.Subscribe(NumberProvider);

            NumberObserver reporter2 = new NumberObserver("OddObserver", false);
            reporter2.Subscribe(NumberProvider);

            Console.WriteLine("Press any key to stop observering.");

            Task.Factory.StartNew(() => NumberProvider.GenerateNumbers());

            Console.ReadKey();

            NumberProvider.StopGeneration();

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();


        }
    }
}
