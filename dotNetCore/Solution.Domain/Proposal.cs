using System;
using System.Collections.Generic;
using System.Linq;
using Solution.Domain.Events;
using Solution.Domain.Events.Proposal;

namespace Solution.Domain
{
    public class Proposal
    {
        public Guid Id { get; set; }
        public decimal LoanValue { get; set; }
        public int NumberOfInstallments { get; set; }
        public decimal InstallmentValue { get { return LoanValue / NumberOfInstallments; } }
        public List<Proponent> Proponents { get; set; }
        public List<Warranty> Warranties { get; set; }
        public List<EventBase> Events { get; set; }

        public Proposal(ProposalCreatedEvent ev)
        {
            Id = ev.ProposalId;
            LoanValue = ev.LoanValue;
            NumberOfInstallments = ev.NumberOfInstallments;
            Events = new List<EventBase>() { ev };
            Proponents = new List<Proponent>();
            Warranties = new List<Warranty>();
        }

        public bool IsValid()
        {
            return IsValueValid()
                  && IsMonthlyInstallmentsValid()
                  && IsProponentsValid()
                  && IsWarrantiesValid();
        }

        private bool IsWarrantiesValid()
        {
            return MinimumWarrantiesNumber() && MinimumWarrantiesValue() && ValidWarrantiesProvince();
        }

        private bool ValidWarrantiesProvince()
        {
            List<string> invalidProvinces = new List<string>() { "PR", "SC", "RS" };
            return Warranties.All(w => !invalidProvinces.Contains(w.Province));
        }

        private bool MinimumWarrantiesValue()
        {
            return Warranties.Sum(w => w.Value) > 2 * LoanValue;
        }

        private bool MinimumWarrantiesNumber()
        {
            return Warranties.Count > 1;
        }

        private bool IsProponentsValid()
        {
            return MinimumNumberProponents() && SingleMainProponent() && ProponentAge() && MainProponentIncome();
        }

        private bool SingleMainProponent()
        {
            var mainProponent = Proponents.SingleOrDefault(p => p.IsMain);
            return mainProponent != null;
        }

        private bool MainProponentIncome()
        {
            var mainProponent = Proponents.SingleOrDefault(p => p.IsMain);
            if (mainProponent == null)
                return false;

            if (mainProponent.Age >= 18 && mainProponent.Age < 24)
                return mainProponent.MonthlyIncome > 4 * InstallmentValue;

            if (mainProponent.Age >= 24 && mainProponent.Age < 50)
                return mainProponent.MonthlyIncome > 3 * InstallmentValue;

            if (mainProponent.Age >= 50)
                return mainProponent.MonthlyIncome > 2 * InstallmentValue;

            return false;
        }

        private bool MinimumNumberProponents()
        {
            return Proponents.Count >= 2;
        }

        private bool ProponentAge()
        {
            return Proponents.All(p => p.Age >= 18);
        }

        public bool IsValueValid()
        {
            return LoanValue > 30000 && LoanValue < 3000000;
        }

        public bool IsMonthlyInstallmentsValid()
        {
            return NumberOfInstallments >= 24 && NumberOfInstallments <= 180;
        }
    }
}
