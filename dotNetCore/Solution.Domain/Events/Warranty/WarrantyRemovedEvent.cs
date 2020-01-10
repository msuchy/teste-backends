using System;

namespace Solution.Domain.Events.Warranty
{
  public class WarrantyRemovedEvent : EventBase
   {
      public Guid WarrantyId { get; set; }
      public WarrantyRemovedEvent(string[] messageData) : base (messageData)
      {
         WarrantyId = Guid.Parse(messageData[5]);
      }
   }
}
