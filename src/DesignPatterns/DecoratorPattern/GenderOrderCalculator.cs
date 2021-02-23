namespace DecoratorPattern
{
    // Gender - 20% upustu dla kobiet
    public class GenderOrderCalculator
    {
        public decimal CalculateDiscount(Order order)
        {
            if (order.Customer.Gender == Gender.Female)
            {
                return order.Amount * 0.2m;
            }
            else
                return 0;
        }
    }

    public abstract class AbstractOrder
    {
        public decimal TotalAmount { get; set; }
        public abstract decimal CalculateDiscount();
    }

    public class ConcreteOrder : AbstractOrder
    {
        public override decimal CalculateDiscount()
        {
            return 0;
        }
    }

    public abstract class OrderDecorator : AbstractOrder
    {
        protected AbstractOrder order;

        protected OrderDecorator(AbstractOrder order)
        {
            this.order = order;
        }

        public override decimal CalculateDiscount()
        {
            return order.CalculateDiscount();
        }
    }

    public class PercentageOrderDecorator : OrderDecorator
    {
        private decimal percentage;
        
        public PercentageOrderDecorator(AbstractOrder order, decimal percentage)
            : base(order)
        {
            this.percentage = percentage;
        }

        public override decimal CalculateDiscount()
        {
            return base.CalculateDiscount() + order.TotalAmount * percentage;
        }
    }

    public class FixedOrderDecorator : OrderDecorator
    {
        private decimal amount;

        public FixedOrderDecorator(AbstractOrder order, decimal amount)
            : base(order)
        {
            this.amount = amount;
        }

        public override decimal CalculateDiscount()
        {
            return base.CalculateDiscount() + amount;
        }
    }
}
