using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChainOfResponsibilityPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Chain of Responsibility Pattern!");

            // Test();

            IHandler bossHandler = new BossHandler();
           // IHandler secretaryHandler = new SecretaryHandler();
            IHandler trashHandler = new TrashHandler();

            bossHandler
               // .SetNext(secretaryHandler)
                    .SetNext(trashHandler);

            decimal amount = 2000;

            string result = bossHandler.Handle(amount);

            // CacheTest();
        }

        public interface IHandler
        {
            string Handle(decimal request);
            IHandler SetNext(IHandler handler);
        }

        // abstract handler
        public class AbstractHandler : IHandler
        {
            private IHandler next;

            public virtual string Handle(decimal request)
            {
                if (this.next != null)
                {
                    return this.next.Handle(request);
                }
                else
                {
                    return null;
                }
            }

            public IHandler SetNext(IHandler handler)
            {
                this.next = handler;

                return handler;
            }
        }

        // concrete handler
        public class BossHandler : AbstractHandler
        {
            public override string Handle(decimal request)
            {
                if (request > 5000)
                {
                    return "Send to Boss";
                }
                else
                { 
                    return base.Handle(request);
                }
            }
        }

        public class SecretaryHandler : AbstractHandler
        {
            public override string Handle(decimal request)
            {
                if (request <= 5000 & request > 1000)
                {
                    return "Send to Secretary";
                }
                else
                {
                    return base.Handle(request);
                }
            }
        }

        public class TrashHandler : AbstractHandler
        {
            public override string Handle(decimal request)
            {
                return "Trash";
            }
        }

        private static void Test()
        {
            decimal amount = 1000;

            if (amount > 5000)
            {
                Console.WriteLine("Send to Boss");
            }
            else
            if (amount <= 5000 & amount > 1000)
            {
                Console.WriteLine("Send to Secretary");
            }
            else
            {
                Console.WriteLine("Trash");
            }
        }

        private static void CacheTest()
        {
            CacheProductService cacheProductService = new CacheProductService();
            IProductService productService = new DbProductService();

            int productId = 1;

            for (int i = 0; i < 3; i++)
            {
                Product product = cacheProductService.Get(productId);

                if (product == null)
                {
                    product = productService.Get(productId);

                    cacheProductService.Set(productId, product);
                }

                Console.WriteLine(product);
            }
        }
    }

    public class Product
    {
        public Product(int id, string name, decimal unitPrice)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }

        public override string ToString()
        {
            return $"{Id} {Name} {UnitPrice:C2}";
        }
    }

    public interface IProductService
    {
        Product Get(int id);
    }

    public class CacheProductService
    {
        private readonly IDictionary<int, Product> products = new Dictionary<int, Product>();

        public void Set(int id, Product product)
        {
            products.TryAdd(id, product);
        }

        public Product Get(int id)
        {
            Console.WriteLine($"Get product Id={id} from cache");

            products.TryGetValue(id, out Product product);

            return product;
        }


    }

    public class DbProductService : IProductService
    {
        private readonly ICollection<Product> products;

        public DbProductService()
        {
            products = new Collection<Product>
            {
                new Product(1, "Książka C#", 100m),
                new Product(2, "Książka Praktyczne Wzorce projektowe w C#", unitPrice: 150m),
                new Product(3, "Zakładka do książki", unitPrice: 10m),
            };
        }

        public Product Get(int id)
        {
            Console.WriteLine($"Get product Id={id} from database");
            return products.SingleOrDefault(p => p.Id == id);
        }
    }

}
