using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IteratorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Iterator Pattern!");

            ICollection<int> numbers = new List<int> { 40, 6, 4, 7, 43, 49 };

            //for (int i = 0; i < numbers.Count(); i++)
            //{

            //}

            Customer customer = new Customer();

            foreach (var number in numbers)
            {
                Console.WriteLine(number);

                numbers.Add(100);
            }


        }

    }

    class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
    }

}
