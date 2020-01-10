using System;

namespace Solution.Domain.Events
{
   public abstract class EventBase
   {
      public string Schema { get; }
      public string Action { get; }
      public Guid Id { get; set; }
      public DateTimeOffset Timestamp { get; set; }
      public Guid ProposalId { get; set; }
      public EventBase(string[] messageData){
         Schema = messageData[1];
         Action = messageData[2];
         Id = Guid.Parse(messageData[0]);
         Timestamp = DateTime.Parse(messageData[3]);
         ProposalId = Guid.Parse(messageData[4]);

      }

      public Solution.Domain.Proposal Proposal => throw new NotImplementedException();

      public void Run() => throw new NotImplementedException();
   }
}