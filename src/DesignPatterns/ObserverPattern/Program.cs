using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace ObserverPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Observer Pattern!");

            //ObservableTest();

            ReactiveCpuTest();


            // CpuTest();
        }

        private static void ObservableTest()
        {

            // Covid19Test();

            

            ConsoleObserver observer1 = new ConsoleObserver("Marcin");
            ConsoleObserver observer2 = new ConsoleObserver("Bartek");

            MessagesSource source = new MessagesSource();

            source.Subscribe(observer1);


            source.OnNext("Break coffee");

            source.Subscribe(observer2);

            source.OnNext("Lunch");
        }

        private static void Covid19Test()
        {
            IObservationService observationService = new FakeObservationService();

            var observations = observationService.Get();

            foreach (Observation observation in observations)
            {
                Console.WriteLine(observation);

                if (observation.Country == "Poland" && observation.Confirmed > 30)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Poland ALERT");
                    Console.ResetColor();
                }

                if (observation.Country == "Germany" && observation.Confirmed > 10)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Germany ALERT");
                    Console.ResetColor();
                }
            }
        }

        private static void ReactiveCpuTest()
        {
            // dotnet add package System.Reactive

            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            IObservable<float> source = Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(_ => cpuCounter.NextValue());

            


         //   source.Subscribe(cpu => Console.WriteLine($"{cpu}"));

            IObservable<float> alertCpuSource1 = source.Where(cpu => cpu > 40);
            IObservable<float> alertCpuSource2 = source.Where(cpu => cpu > 30);

            IObservable<float> alertCpuSource = source
                // .Merge(alertCpuSource2)
                .Do( cpu => Console.WriteLine(cpu))
                .Buffer(TimeSpan.FromSeconds(10))
                .Select(m => m.Average())               
                ;

            alertCpuSource.Subscribe(cpu =>
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"CPU {cpu} %");
                Console.ResetColor();
            });

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();



        }

        private static void CpuTest()
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            while (true)
            {
                float cpu = cpuCounter.NextValue();

                if (cpu < 30)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"CPU {cpu} %");
                    Console.ResetColor();
                }
                else
                if (cpu > 40)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"CPU {cpu} %");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"CPU {cpu} %");
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }


        public class MessagesSource : IObservable<string>, IObserver<string>
        {
            private ICollection<IObserver<string>> observers = new Collection<IObserver<string>>();

            public void OnCompleted()
            {
                foreach (var observer in observers)
                {
                    observer.OnCompleted();
                }
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(string value)
            {
                foreach (IObserver<string> observer in observers)
                {
                    observer.OnNext(value);
                }
            }

            public IDisposable Subscribe(IObserver<string> observer)
            {
                observers.Add(observer);

                observer.OnNext("Welcome!");

                observer.OnNext("Hello World!");

                observer.OnNext("Design Patterns!");

                observer.OnCompleted();

                return null;

            }
        }

        public class ConsoleObserver : IObserver<string>
        {
            public ConsoleObserver(string name)
            {
                Name = name;
            }

            public string Name { get; set; }

            public void OnCompleted()
            {
                Console.WriteLine("EOF");
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(string value)
            {
                Console.WriteLine($"OnNext: {Name} {value}");
            }
        }


        #region COVID19

        public class Observation
        {
            public string Country { get; set; }
            public int Confirmed { get; set; }
            public int Recovered { get; set; }
            public int Deaths { get; set; }

            public override string ToString()
            {
                return $"{Country} {Confirmed}/{Recovered}/{Deaths}";
            }

        }

        public interface IObservationService
        {
            IEnumerable<Observation> Get();
        }

        public class FakeObservationService : IObservationService
        {
            public IEnumerable<Observation> Get()
            {
                yield return new Observation { Country = "China", Confirmed = 2 };
                yield return new Observation { Country = "Germany", Confirmed = 1 };
                yield return new Observation { Country = "China", Confirmed = 20 };
                yield return new Observation { Country = "Germany", Confirmed = 60, Recovered = 4, Deaths = 2 };
                yield return new Observation { Country = "Poland", Confirmed = 10, Recovered = 5 };
                yield return new Observation { Country = "China", Confirmed = 30 };
                yield return new Observation { Country = "Poland", Confirmed = 50, Recovered = 15 };
                yield return new Observation { Country = "US", Confirmed = 10, Recovered = 5, Deaths = 1 };
                yield return new Observation { Country = "US", Confirmed = 11, Recovered = 3, Deaths = 4 };
                yield return new Observation { Country = "Poland", Confirmed = 45, Recovered = 25 };
                yield return new Observation { Country = "Germany", Confirmed = 52, Recovered = 4, Deaths = 1 };
            }
        }

        #endregion



    }



}
