using System;

namespace TemplateMethodPattern
{
    // Happy Hours - 10% upustu w godzinach od 9 do 15
    public class HappyHoursOrderCalculator : OrderCalculator
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal percentage;

        public HappyHoursOrderCalculator(TimeSpan from, TimeSpan to, decimal percentage)
        {
            this.from = from;
            this.to = to;
            this.percentage = percentage;
        }

        public override bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

        public override decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    // Gender - 20% upustu dla kobiet
    public class GenderOrderCalculator : OrderCalculator
    {
        private readonly Gender gender;
        private readonly decimal percentage;

        public GenderOrderCalculator(Gender gender, decimal percentage)
        {
            this.gender = gender;
            this.percentage = percentage;
        }

        public override bool CanDiscount(Order order)
        {
            return order.Customer.Gender == gender;
        }

        public override decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }


    public abstract class OrderCalculator
    {
        public abstract bool CanDiscount(Order order);
        public abstract decimal Discount(Order order);

        public decimal CalculateDiscount(Order order)
        {
            if (CanDiscount(order))
            {
                return Discount(order);
            }
            else
                return 0;
        }
    }
}
