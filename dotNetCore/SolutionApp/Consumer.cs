using Solution.Domain;
using Solution.Domain.Events;
using Solution.Domain.Events.Proponent;
using Solution.Domain.Events.Proposal;
using Solution.Domain.Events.Warranty;
using System;
using System.Linq;

namespace SolutionApp
{
  public class Consumer
  {
    private readonly IProposalRepository _repo;

    public Consumer(IProposalRepository repository)
    {
      _repo = repository;

    }

    internal void Consume(EventBase ev)
    {
      typeof(Consumer).GetMethod("Consume", new Type[] { ev.GetType() }).Invoke(this, new object[] { ev });
    }

    public bool IsValid(EventBase ev)
    {
      var currentProposal = _repo.GetById(ev.ProposalId);

      if (currentProposal == null && ev is ProposalCreatedEvent)
        return true;

      if (currentProposal == null)
        return false;

      if (currentProposal.Events.Any(e => e.Id == ev.Id))
        return false;

      if (currentProposal.Events.Any(e => e.Timestamp > ev.Timestamp))
        return false;

      return true;
    }

    public void Consume(WarrantyAddedEvent ev)
    {
      var currentProposal = _repo.GetById(ev.ProposalId);
      var warranty = new Warranty(ev);
      currentProposal.Warranties.Add(warranty);
      currentProposal.Events.Add(ev);
      _repo.Update(currentProposal);
    }
    public void Consume(WarrantyUpdatedEvent ev)
    {
      var currentProposal = _repo.GetById(ev.ProposalId);
      var warranty = currentProposal.Warranties.Single(w => w.Id == ev.WarrantyId);
      warranty.Value = ev.Value;
      warranty.Province = ev.Province;
      currentProposal.Events.Add(ev);
      _repo.Update(currentProposal);
    }
    public void Consume(WarrantyRemovedEvent ev)
    {
      var currentProposal = _repo.GetById(ev.ProposalId);
      var warranty = currentProposal.Warranties.Single(w => w.Id == ev.WarrantyId);
      currentProposal.Warranties.Remove(warranty);
      currentProposal.Events.Add(ev);
      _repo.Update(currentProposal);
    }
    public void Consume(ProponentAddedEvent ev)
    {
      var currentProposal = _repo.GetById(ev.ProposalId);
      var proponent = new Proponent(ev);
      currentProposal.Proponents.Add(proponent);
      currentProposal.Events.Add(ev);
      _repo.Update(currentProposal);
    }
    public void Consume(ProponentUpdatedEvent ev)
    {
      var currentProposal = _repo.GetById(ev.ProposalId);
      var proponent = currentProposal.Proponents.Single(w => w.Id == ev.ProponentId);
      proponent.Name = ev.Name;
      proponent.Age = ev.Age;
      proponent.MonthlyIncome = ev.MonthlyIncome;
      proponent.IsMain = ev.IsMain;
      currentProposal.Events.Add(ev);
      _repo.Update(currentProposal);
    }

    public void Consume(ProponentRemovedEvent ev)
    {
      var currentProposal = _repo.GetById(ev.ProposalId);
      var warranty = currentProposal.Proponents.Single(w => w.Id == ev.ProponentId);
      currentProposal.Proponents.Remove(warranty);
      currentProposal.Events.Add(ev);
      _repo.Update(currentProposal);
    }
    public void Consume(ProposalCreatedEvent ev)
    {
      _repo.Save(new Proposal(ev));
    }
    public void Consume(ProposalUpdatedEvent ev)
    {
      var currentProposal = _repo.GetById(ev.Id);
      currentProposal.LoanValue = ev.LoanValue;
      currentProposal.NumberOfInstallments = ev.NumberOfInstallments;
      currentProposal.Events.Add(ev);
      _repo.Update(currentProposal);
    }
    public void Consume(ProposalDeletedEvent ev)
    {
      _repo.Delete(ev.ProposalId);
    }
  }
}
