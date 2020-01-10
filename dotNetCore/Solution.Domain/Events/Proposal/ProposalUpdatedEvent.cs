using System;

namespace Solution.Domain.Events.Proposal
{
   public class ProposalUpdatedEvent : EventBase
   {
      public decimal LoanValue { get; set; }
      public int NumberOfInstallments { get; set; }
      public ProposalUpdatedEvent(string[] messageData): base(messageData)
      {
         LoanValue = decimal.Parse(messageData[5]);
         NumberOfInstallments = Int32.Parse(messageData[6]);
      }

   }
}