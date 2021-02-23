using System;

namespace StrategyPattern
{
    // Happy Hours - 10% upustu w godzinach od 9 do 15
    public class HappyHoursOrderCalculator
    {
        public decimal CalculateDiscount(Order order)
        {
            if (order.OrderDate.Hour >= 9 && order.OrderDate.Hour <= 15)
            {
                return order.Amount * 0.1m;
            }
            else
                return 0;
        }
    }

    // abstract strategy
    public interface IDiscountStrategy
    {
        bool CanDiscount(Order order);
        decimal Discount(Order order);
    }

    public interface ICanDiscountStrategy
    {
        bool CanDiscount(Order order);
    }

    public interface ICalculateDiscountStrategy
    {
        decimal Discount(Order order);
    }

    public class ProductGratis : IProductDiscountStrategy
    {
        private readonly Product product;

        public ProductGratis(Product product)
        {
            this.product = product;
        }

        public Product Discount(Order order)
        {
            return product;
        }
    }

    public interface IProductDiscountStrategy : ICalculateDiscountStrategy<Product>
    {

    }
    public interface IMoneyDiscountStrategy : ICalculateDiscountStrategy<decimal>
    {

    }

    public class FixedMoneyDiscountStrategy : IMoneyDiscountStrategy
    {
        private readonly decimal amount;

        public FixedMoneyDiscountStrategy(decimal amount)
        {
            this.amount = amount;
        }

        public decimal Discount(Order order)
        {
            return amount;
        }
    }

    public interface ICalculateDiscountStrategy<T>
    {
        T Discount(Order order);
    }

    // concrete strategy
    public class HappyHoursPercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal percentage;

        public HappyHoursPercentageDiscountStrategy(TimeSpan from, TimeSpan to, decimal percentage)
        {
            this.from = from;
            this.to = to;
            this.percentage = percentage;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

        public decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }

   

    public class SkinColorPercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly ConsoleColor color;
        private readonly decimal percentage;

        public SkinColorPercentageDiscountStrategy(ConsoleColor color, decimal percentage)
        {
            this.color = color;
            this.percentage = percentage;
        }

        public bool CanDiscount(Order order)
        {
            return order.Customer.SkinColor == color;
        }

        public decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class SkinColorFixedDiscountStrategy : IDiscountStrategy
    {
        private readonly ConsoleColor color;
        private readonly decimal amount;

        public SkinColorFixedDiscountStrategy(ConsoleColor color, decimal amount)
        {
            this.color = color;
            this.amount = amount;
        }

        public bool CanDiscount(Order order)
        {            
            return order.Customer.SkinColor == color;
        }

        public decimal Discount(Order order)
        {
            return amount;
        }
    }

    public class SkinColorDiscountStrategy : ICanDiscountStrategy
    {
        private readonly ConsoleColor color;

        public SkinColorDiscountStrategy(ConsoleColor color)
        {
            this.color = color;
        }

        public bool CanDiscount(Order order)
        {
            return order.Customer.SkinColor == color;
        }
    }

    public class HappyHoursDiscountStrategy : ICanDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;

        public HappyHoursDiscountStrategy(TimeSpan from, TimeSpan to)
        {
            this.from = from;
            this.to = to;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

       
    }

    public class PercentageDiscountStrategy : ICalculateDiscountStrategy
    {
        private readonly decimal percentage;

        public PercentageDiscountStrategy(decimal percentage)
        {
            this.percentage = percentage;
        }

        public decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class FixedDiscountStrategy : ICalculateDiscountStrategy
    {
        private readonly decimal amount;

        public FixedDiscountStrategy(decimal amount)
        {
            this.amount = amount;
        }

        public decimal Discount(Order order)
        {
            return amount;
        }
    }

    public class OrderCalculator
    {
        private readonly IDiscountStrategy discountStrategy;

        public OrderCalculator(IDiscountStrategy discountStrategy)
        {
            this.discountStrategy = discountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (discountStrategy.CanDiscount(order))
            {
                return discountStrategy.Discount(order);
            }
            else
                return 0;
        }
    }

     public class SecondOrderCalculator
    {
        private readonly ICanDiscountStrategy canDiscountStrategy;
        private readonly ICalculateDiscountStrategy calculateDiscountStrategy;

        public SecondOrderCalculator(
            ICanDiscountStrategy canDiscountStrategy, 
            ICalculateDiscountStrategy calculateDiscountStrategy)
        {
            this.canDiscountStrategy = canDiscountStrategy;
            this.calculateDiscountStrategy = calculateDiscountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (canDiscountStrategy.CanDiscount(order))
            {
                return calculateDiscountStrategy.Discount(order);
            }
            else
                return 0;
        }
    }


}
