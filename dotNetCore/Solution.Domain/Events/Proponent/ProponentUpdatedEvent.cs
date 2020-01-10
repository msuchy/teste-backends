using System;
using System.Globalization;

namespace Solution.Domain.Events.Proponent
{
  public class ProponentUpdatedEvent : EventBase
    {
        public Guid ProponentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal MonthlyIncome { get; set; }
        public bool IsMain { get; set; }
        public ProponentUpdatedEvent(string[] messageData) : base(messageData)
        {
            ProponentId = Guid.Parse(messageData[5]);
            Name = messageData[6].Trim();
            Age = Int32.Parse(messageData[7]);
            CultureInfo culture = new CultureInfo("en-US");
            MonthlyIncome = Convert.ToDecimal(messageData[8], culture);
            IsMain = bool.Parse(messageData[9]);
        }
    }
}
