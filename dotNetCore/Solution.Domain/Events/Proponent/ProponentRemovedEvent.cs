using System;

namespace Solution.Domain.Events.Proponent
{
  public class ProponentRemovedEvent : EventBase
    {
        public Guid ProponentId { get; set; }
        public ProponentRemovedEvent(string[] messageData) : base(messageData)
        {
            ProponentId = Guid.Parse(messageData[5]);
        }
    }
}
