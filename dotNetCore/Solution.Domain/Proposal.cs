using System;
using System.Collections.Generic;
using Solution.Domain.Events;

namespace Solution.Domain
{
   public class Proposal
   {
      public Guid Id { get; set; }
      public decimal LoanValue { get; set; }
      public int NumberOfInstallments { get; set; }
      public IEnumerable<Proponent> Proponents { get; set; }
      public IEnumerable<Warranty> Warranties { get; set; }
      public IEnumerable<EventBase> Events { get; set; }

      public bool IsValid()
      {
         throw new NotImplementedException();
      }
   }
}