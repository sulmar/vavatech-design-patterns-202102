using System;
using System.ComponentModel;

namespace MementoPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Customer customer = new Customer { FirstName = "John", LastName = "Smith" };

            Console.WriteLine(customer);

            customer.BeginEdit();

            customer.FirstName = "Bob";

            Console.WriteLine(customer);

            customer.CancelEdit();

            Console.WriteLine(customer);


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
