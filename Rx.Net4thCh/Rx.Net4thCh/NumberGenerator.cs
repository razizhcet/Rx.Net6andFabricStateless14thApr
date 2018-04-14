using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rx.Net4thCh
{
    public class NumberGenerator : IObservable<int>
    {
        public NumberGenerator()
        {
            observers = new List<IObserver<int>>();
        }

        private List<IObserver<int>> observers;

        public IDisposable Subscribe(IObserver<int> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);

            }//End-if (!observers.Contains(observer))

            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<int>> _observers;
            private IObserver<int> _observer;

            public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }//End-if (_observer != null && _observers.Contains(_observer))
            }
        }

        public void GenerateNumbers()
        {
            for (int i = 0; ; i++)
            {
                Thread.Sleep(250);

                foreach (var observer in observers.ToArray())
                {
                    if (i == 10)
                    {
                        observer.OnError(new NumberNotGeneratedException());
                    }
                    else
                    {
                        observer.OnNext(i);

                    }//End-if-else (i == 10)

                }//End-foreach (var observer in observers.ToArray())

            }//End-for (int i = 0; ; i++)
        }

        public void StopGeneration()
        {
            foreach (var observer in observers.ToArray())
            {
                if (observers.Contains(observer))
                {
                    observer.OnCompleted();

                }//End-if (observers.Contains(observer))

            }//End-for-each (var observer in observers.ToArray())

            observers.Clear();
        }
    }
}
