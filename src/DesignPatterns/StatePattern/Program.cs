using Stateless;
using System;
using System.Collections.Generic;

namespace StatePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello State Pattern!");

            // OrderTest();

            Lamp lamp = new Lamp();

            Console.WriteLine(lamp.Graph);

            Console.WriteLine(lamp.State);

            while (true)
            {
                lamp.Toggle();
                Console.WriteLine(lamp.State);

                Console.ReadKey();
            }

            


        }

        private static void OrderTest()
        {
            Order order = Order.Create();

            order.Completion();

            if (order.Status == OrderStatus.Completion)
            {
                order.Status = OrderStatus.Sent;
                Console.WriteLine("Your order was sent.");
            }

            order.Cancel();
        }
    }

    #region Models

    public class Order
    {
        public Order(string orderNumber)
        {
            Status = OrderStatus.Created;

            OrderNumber = orderNumber;
            OrderDate = DateTime.Now;
         
        }

        public DateTime OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public OrderStatus Status { get; set; }

        private static int indexer;

        public static Order Create()
        {
            Order order = new Order($"Order #{indexer++}");

            if (order.Status == OrderStatus.Created)
            {
                Console.WriteLine("Thank you for your order");
            }

            return order;
        }

        public void Completion()
        {
            if (Status == OrderStatus.Created)
            {
                this.Status = OrderStatus.Completion;

                Console.WriteLine("Your order is in progress");
            }
        }

        public void Cancel()
        {
            if (this.Status == OrderStatus.Created || this.Status == OrderStatus.Completion)
            {
                this.Status = OrderStatus.Canceled;

                Console.WriteLine("Your order was cancelled.");
            }
        }

    }

    public enum OrderStatus
    {
        Created,
        Completion,
        Sent,
        Canceled,
        Done
    }


    // dotnet add package stateless

    public class Lamp
    {
        public LampState State => machine.State;

        private readonly StateMachine<LampState, LampTrigger> machine;

        public string Graph => Stateless.Graph.UmlDotGraph.Format(machine.GetInfo());

        private System.Timers.Timer timer;

        public bool IsWorkingHours => DateTime.Now.Hour < 17;

        public Lamp()
        {
            machine = new StateMachine<LampState, LampTrigger>(LampState.Off);

            machine.Configure(LampState.Off)
                .Permit(LampTrigger.Toggle, LampState.On);

            machine.Configure(LampState.On)
                .OnEntry(() => timer.Start())
                .OnEntry(() => Console.WriteLine("<xml>SET DEVICE ON</xml>"), "Set device on")
                .Permit(LampTrigger.Toggle, LampState.Blinking)
                .PermitIf(LampTrigger.Toggle, LampState.Off, () => !IsWorkingHours)
                .Permit(LampTrigger.ElapsedTime, LampState.Off)
                .OnExit(() => Console.WriteLine("<xml>SET DEVICE OFF</xml>"), "Set device off")
                .OnExit(() => timer.Stop());

            machine.Configure(LampState.Blinking)
                .Permit(LampTrigger.Toggle, LampState.Off);

            machine.OnTransitioned(t => Console.WriteLine($" {t.Trigger} : {t.Source} -> {t.Destination}"));

            timer = new System.Timers.Timer(TimeSpan.FromSeconds(5).TotalMilliseconds);
            timer.AutoReset = false;

            timer.Elapsed += (s, e) => machine.Fire(LampTrigger.ElapsedTime);

        }

        public void Toggle() => machine.Fire(LampTrigger.Toggle);

        public bool CanToggle => machine.CanFire(LampTrigger.Toggle);
        
        public IEnumerable<LampTrigger> GetPermittedTrigger => machine.GetPermittedTriggers();



    }

    public enum LampTrigger
    {
        Toggle,
        ElapsedTime
    }

    public enum LampState
    {
        On,
        Off,
        Blinking
    }

    #endregion

}
