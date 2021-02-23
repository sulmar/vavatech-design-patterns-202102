using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MementoPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Customer customer = new Customer { FirstName = "John", LastName = "Smith" };

            Originator originator = new Originator(customer);
            Caretaker caretaker = new Caretaker(originator);

            caretaker.Backup();
            customer.FirstName = "Bob";

            caretaker.Backup();
            customer.FirstName = "Jerzy";

            caretaker.ShowHistory();

            Console.WriteLine(customer);

            caretaker.Undo();
            Console.WriteLine(customer);

            caretaker.Undo();
            Console.WriteLine(customer);

            Console.WriteLine(customer);

            customer.BeginEdit();

            customer.FirstName = "Bob";

            Console.WriteLine(customer);

            customer.CancelEdit();

            Console.WriteLine(customer);


        }
    }


    public interface IMemento
    {
        string GetName();
        Customer GetState();
        DateTime GetDate();
    }


    public class ConcreteMemento : IMemento
    {
        private Customer state;
        private DateTime date;

        public ConcreteMemento(Customer state)
        {
            this.state = state;
            this.date = DateTime.Now;
        }

        public DateTime GetDate()
        {
            return this.date;
        }

        public string GetName()
        {
            return this.state.FullName;
        }

        public Customer GetState()
        {
            return this.state;
        }
    }

    public class Caretaker
    {

        private Stack<IMemento> _mementos = new Stack<IMemento>();

        private Originator _originator = null;

        public Caretaker(Originator originator)
        {
            this._originator = originator;
        }

        public void Backup()
        {
            this._mementos.Push(this._originator.Save());
        }

        public void Undo()
        {
            var memento = this._mementos.Pop();

            this._originator.Restore(memento);
        }

        public void ShowHistory()
        {
            foreach (var memento in this._mementos)
            {
                Console.WriteLine($"{memento.GetDate()} {memento.GetName()}");
            }
        }
    }


    public class Originator
    {
        private Customer state;

        public Originator(Customer state)
        {
            this.state = state;
        }

        public IMemento Save()
        {
            return new ConcreteMemento((Customer) this.state.Clone());
        }

        // Restores the Originator's state from a memento object.
        public void Restore(IMemento memento)
        {
            if (!(memento is ConcreteMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }

            this.state = memento.GetState();
            
        }
    }


    public class Customer : IEditableObject, ICloneable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public override string ToString()
        {
            return FullName;
        }

        private Customer last;

        public void BeginEdit()
        {
            last = (Customer) this.Clone();
        }

        public void CancelEdit()
        {
            this.FirstName = last.FirstName;
            this.LastName = last.LastName;
        }

        public void EndEdit()
        {
            last = null;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
