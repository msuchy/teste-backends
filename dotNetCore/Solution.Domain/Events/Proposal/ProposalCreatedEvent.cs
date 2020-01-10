using System;

namespace Solution.Domain.Events.Proposal
{
   public class ProposalCreatedEvent : EventBase
   {
      public decimal LoanValue { get; set; }
      public int NumberOfInstallments { get; set; }
      public ProposalCreatedEvent(string[] messageData) : base(messageData)
      {
         LoanValue = decimal.Parse(messageData[5]);
         NumberOfInstallments = Int32.Parse(messageData[6]);
      }
   }
}