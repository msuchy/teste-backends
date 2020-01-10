using System;
using System.Globalization;

namespace Solution.Domain.Events.Proposal
{
    public class ProposalCreatedEvent : EventBase
    {
        public decimal LoanValue { get; set; }
        public int NumberOfInstallments { get; set; }
        public ProposalCreatedEvent(string[] messageData) : base(messageData)
        {
            CultureInfo culture = new CultureInfo("en-US");
            LoanValue = Convert.ToDecimal(messageData[5], culture);
            NumberOfInstallments = Int32.Parse(messageData[6]);
        }
    }
}
