using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProxyPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Proxy Pattern!");
            GetProductTest();

            SaveProductTest();

        }


        private static void GetProductTest()
        {
            Console.ResetColor();

            IProductRepository productRepository = new DbProductRepository();
            IProductRepository cacheProductRepository = new CacheProductRepository(productRepository);

            while (true)
            {

                Console.Write("Podaj id produktu: ");

                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    Product product = cacheProductRepository.Get(productId);

                    Console.WriteLine($"{product.Id} {product.Name} {product.UnitPrice:C2}");
                }
            }


        }


        //private IActionResult Get(int productId)
        //{
        //    return Ok(productRepository.Get(productId));
        //}

        private static void SaveProductTest()
        {
            ProductsDbContext context = new ProductsDbContext();

            Product product = new Product(1, "Design Patterns w C#", 150m);

            context.Add(product);

            product.UnitPrice = 99m;

            context.MarkAsChanged();

            context.SaveChanges();
        }
    }

    #region Models
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
    }


    public class CacheProductRepository : IProductRepository
    {
        private ICollection<Product> products;

        // RealSubject
        private IProductRepository db;

        public CacheProductRepository(IProductRepository productRepository)
        {
            this.db = productRepository;

            products = new Collection<Product>();
        }

        public void Add(Product product)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Add product {product.Id} to cache");
            Console.ResetColor();
            products.Add(product);
        }

        public Product Get(int id)
        {
            // Check
            Product product = products.SingleOrDefault(p => p.Id == id);

            if (product != null)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Get product {id} from cache");
                Console.ResetColor();
                return product;
            }
            else
            {
                product = db.Get(id);

                if (product != null)
                {
                    products.Add(product);
                }
            }

            return product;
        }

    }

    public interface IProductRepository
    {
        Product Get(int id);
    }

    public interface ICacheProductRepository : IProductRepository
    {

    }


    public class ProductsController
    {
        private IProductRepository productRepository;

        public ProductsController(ICacheProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
    }


    public class DbProductRepository : IProductRepository
    {
        private ICollection<Product> products;

        public DbProductRepository()
        {
            products = new Collection<Product>()
            {
                new Product(1, "Product 1", 10),
                new Product(2, "Product 2", 10),
                new Product(3, "Product 3", 10),
            };
        }

        public Product Get(int id)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"Get product {id} from database");
            Console.ResetColor();
            return products.SingleOrDefault(p => p.Id == id);
        }
    }

    // REDIS

    public class ProductsDbContext
    {
        private Product product;
        private bool changed;

        public void Add(Product product)
        {
            this.product = product;
        }

        public Product Get()
        {
            return product;
        }

        public void SaveChanges()
        {
            if (changed)
            {
                Console.WriteLine($"UPDATE dbo.Products SET UnitPrice = {product.UnitPrice} WHERE ProductId = {product.Id}");
            }
        }

        public void MarkAsChanged()
        {
            changed = true;
        }
    }

    #endregion
}
