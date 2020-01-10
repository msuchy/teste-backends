namespace Solution.Domain.Events.Proposal
{
   public class ProposalDeletedEvent : EventBase
   {
      public ProposalDeletedEvent(string[] messageData) : base(messageData){  }
   }
}