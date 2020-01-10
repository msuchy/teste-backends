using System;
using System.Globalization;

namespace Solution.Domain.Events.Proposal
{
    public class ProposalUpdatedEvent : EventBase
    {
        public decimal LoanValue { get; set; }
        public int NumberOfInstallments { get; set; }
        public ProposalUpdatedEvent(string[] messageData) : base(messageData)
        {
            CultureInfo culture = new CultureInfo("en-US");
            LoanValue = Convert.ToDecimal(messageData[5], culture);
            NumberOfInstallments = Int32.Parse(messageData[6]);
        }
    }
}
