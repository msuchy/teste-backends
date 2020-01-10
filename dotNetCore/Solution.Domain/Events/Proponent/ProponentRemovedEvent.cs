using System;
using System.Linq;

namespace Solution.Domain.Events.Proponent
{
    public class ProponentRemovedEvent : EventBase
    {
        public Guid ProponentId { get; set; }
        public ProponentRemovedEvent(IProposalRepository repo, string[] messageData) : base(repo, messageData)
        {
            ProponentId = Guid.Parse(messageData[5]);
        }

        public override void Run()
        {
            var currentProposal = _repo.GetById(this.ProposalId);
            var warranty = currentProposal.Proponents.Single(w => w.Id == this.ProponentId);
            currentProposal.Proponents.Remove(warranty);
            _repo.Update(currentProposal);
        }
    }
}
