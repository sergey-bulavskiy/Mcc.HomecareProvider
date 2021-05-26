using System;

namespace Mcc.HomecareProvider.Domain
{
    public class StatisticalDay
    {
        public Guid Id { get; set; }

        protected StatisticalDay()
        {
            
        }

        public StatisticalDay(double value)
        {
            Id = Guid.NewGuid();
            Value = value;
        }
        
        public double Value { get; set; }
    }
}