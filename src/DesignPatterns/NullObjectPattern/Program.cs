using System;

namespace NullObjectPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Null Object Pattern!");

            IProductRepository productRepository = new FakeProductRepository();

            // ProductBase product = productRepository.Get(2);

            if (productRepository.TryGetProduct(2, out Product product))
            {

            }

            // Problem: Zawsze musimy sprawdzać czy obiekt nie jest pusty (null).

            product.Rate(3);

        }
    }

    public interface IProductRepository
    {
        ProductBase Get(int id);

        bool TryGetProduct(int id, out Product product);
    }

    public class FakeProductRepository : IProductRepository
    {


        public ProductBase Get(int id)
        {
            if (id == 1)
            {
                return new Product();
            }

            return new NullProduct();
        }

        public bool TryGetProduct(int id, out Product product)
        {
            if (id == 1)
            {
                product = new Product();
                return true;
            }
            else
            {
                product = new Product();
                return false;
            }
        }
    }

    public abstract class ProductBase
    {
        protected int rate;
        public abstract void Rate(int rate);
    }

    public class Product : ProductBase
    {      
        public override void Rate(int rate)
        {
            this.rate = rate;
        }

    }

    // NullObject
    public class NullProduct : ProductBase
    {
        public override void Rate(int rate)
        {
            // nic nie rób            
        }
    }
}
