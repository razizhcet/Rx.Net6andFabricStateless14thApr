using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.Net4thCh
{
    public class NumberObserver : IObserver<int>
    {
        private IDisposable unsubscriber;
        private string instName;

        private bool isEvenObserver;

        public NumberObserver(string name, bool isEvenObserver)
        {
            this.instName = name;

            this.isEvenObserver = isEvenObserver;
        }

        public string Name
        { get { return this.instName; } }



        public virtual void Subscribe(IObservable<int> provider)
        {
            if (provider != null)
            {
                unsubscriber = provider.Subscribe(this);

            }//End-if (provider != null)
        }

        public virtual void OnCompleted()
        {
            Console.WriteLine("The Number Generator has completed generation {0}.", this.Name);
            this.Unsubscribe();
        }

        public virtual void OnError(Exception e)
        {
            Console.WriteLine("{0}: Error occured while generating number.", this.Name);
        }

        public virtual void OnNext(int value)
        {
            bool isEven = value % 2 == 0;

            if (this.isEvenObserver && isEven)
            {
                Console.WriteLine("{1}: The current number is Even. Value => {0}", value, this.Name);

            }
            else if (!this.isEvenObserver && !isEven)
            {
                Console.WriteLine("{1}: The current number is Odd. Value => {0}", value, this.Name);

            }//End-if (this.isEvenObserver && isEven)
        }

        public virtual void Unsubscribe()
        {
            Console.WriteLine("{0} unsubscribed.", this.Name);

            unsubscriber.Dispose();
        }
    }
}
