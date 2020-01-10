using System;
using System.Globalization;

namespace Solution.Domain.Events.Warranty
{
    public class WarrantyAddedEvent : EventBase
    {
        public Guid WarrantyId { get; set; }
        public decimal Value { get; set; }
        public string Province { get; set; }
        public WarrantyAddedEvent(string[] messageData) : base(messageData)
        {
            WarrantyId = Guid.Parse(messageData[5]);
            CultureInfo culture = new CultureInfo("en-US");
            Value = Convert.ToDecimal(messageData[6], culture);
            Province = messageData[7].Trim();
        }
    }
}
