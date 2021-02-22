using System;

namespace FactoryMethodPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Factory Method Pattern!");

            VisitCalculateAmountTest();
        }

        private static void VisitCalculateAmountTest()
        {
            while (true)
            {
                Console.Write("Podaj rodzaj wizyty: (N)FZ (P)rywatna (F)irma: ");
                string visitType = Console.ReadLine();

                Console.Write("Podaj czas wizyty w minutach: ");
                if (double.TryParse(Console.ReadLine(), out double minutes))
                {
                    TimeSpan duration = TimeSpan.FromMinutes(minutes);

                    VisitFactory visitFactory = new VisitFactory();

                    Visit visit = visitFactory.Create(duration, 100, visitType);

                    decimal totalAmount = visit.CalculateCost();

                    Console.ForegroundColor = ColorFactory.Create(totalAmount);

                    Console.WriteLine($"Total amount {totalAmount:C2}");

                    Console.ResetColor();
                }
            }

        }
    }

    #region Models


    public class ColorFactory
    {
        public static ConsoleColor Create(decimal totalAmount)
        {
            if (totalAmount == 0)
                return ConsoleColor.Green;
            else
                if (totalAmount >= 200)
                    return ConsoleColor.Red;
            else
               return ConsoleColor.White;
        }
    }

    public class NFZVisit : Visit
    {
        public NFZVisit(TimeSpan duration, decimal pricePerHour) : base(duration, pricePerHour)
        {
        }

        public override decimal CalculateCost()
        {
            return 0;
        }
    }

    public class PrivateVisit : Visit
    {
        public PrivateVisit(TimeSpan duration, decimal pricePerHour) : base(duration, pricePerHour)
        {
        }

        public override decimal CalculateCost()
        {
            return base.CalculateCost();
        }
    }


    public class CompanyVisitOptions
    {
        public decimal DiscountPercentage { get; set; }
    }

    public class CompanyVisit : Visit
    {
        private const decimal companyDiscountPercentage = 0.9m;

        public CompanyVisit(TimeSpan duration, decimal pricePerHour) : base(duration, pricePerHour)
        {
        }

        public override decimal CalculateCost()
        {
            return base.CalculateCost() * companyDiscountPercentage;
        }
    }

    public abstract class Visit
    {
        public DateTime VisitDate { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal PricePerHour { get; set; }

        public Visit(TimeSpan duration, decimal pricePerHour)
        {
            VisitDate = DateTime.Now;
            Duration = duration;
            PricePerHour = pricePerHour;
        }

        // public abstract decimal CalculateCost();

        public virtual decimal CalculateCost()
        {
            return (decimal)Duration.TotalHours * PricePerHour;
        }

        //public decimal CalculateCost(string kind)
        //{
        //    decimal cost = 0;

        //    if (kind == "N")
        //    {
        //        cost = 0;
        //    }
        //    else if (kind == "P")
        //    {
        //        cost = (decimal)Duration.TotalHours * PricePerHour;
        //    }
        //    else if (kind == "F")
        //    {
        //        cost = (decimal)Duration.TotalHours * PricePerHour * companyDiscountPercentage;
        //    }

        //    return cost;
        //}


    }

    public class VisitFactory
    {
        public Visit Create(TimeSpan duration, decimal pricePerHour,  string kind)
        {
            if (kind == "N")
            {
                return new NFZVisit(duration, pricePerHour);
            }
            else if (kind == "P")
            {
                return new PrivateVisit(duration, pricePerHour);
            }
            else if (kind == "F")
            {
                return new CompanyVisit(duration, pricePerHour);
            }
            else if (kind == "T")
            {
                return new TeleVisit(duration, pricePerHour);
            }

            throw new NotSupportedException($"Typ {kind} nie jest obsługiwany");
        }
    }


    public class TeleVisit : Visit
    {
        public TeleVisit(TimeSpan duration, decimal pricePerHour) : base(duration, pricePerHour)
        {
        }

        public override decimal CalculateCost()
        {
            return 100;
        }
    }

    #endregion
}
