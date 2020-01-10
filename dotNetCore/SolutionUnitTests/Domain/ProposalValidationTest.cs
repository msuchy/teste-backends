using Solution.Domain;
using Solution.Domain.Events.Proponent;
using Solution.Domain.Events.Proposal;
using Solution.Domain.Events.Warranty;
using Xunit;

namespace SolutionUnitTests.Domain
{
  public class ProposalValidationTest
  {
    public ProposalValidationTest()
    {
    }

    [Theory(DisplayName= "O valor do empréstimo deve estar entre R$ 30.000,00 e R$ 3.000.000,00")]
    [InlineData("30000")]
    [InlineData("3000000")]
    public void ProposalValueValid(string loanValue)
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e",loanValue,"72"
      });
      var proposal = new Proposal(ev);
      Assert.True(proposal.IsValueValid());
    }

    [Theory(DisplayName = "O valor do empréstimo deve estar entre R$ 30.000,00 e R$ 3.000.000,00")]
    [InlineData("29999")]
    [InlineData("3000001")]
    public void ProposalValueInValid(string loanValue)
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e",loanValue,"72"
      });
      var proposal = new Proposal(ev);
      Assert.False(proposal.IsValueValid());
    }

    [Theory(DisplayName = "O empréstimo deve ser pago em no mínimo 2 anos e no máximo 15 anos")]
    [InlineData("72")]
    [InlineData("180")]
    [InlineData("24")]
    public void MonthlyInstallmentsValid(string installments)
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","30000",installments
      });
      var proposal = new Proposal(ev);
      Assert.True(proposal.IsMonthlyInstallmentsValid());
    }

    [Theory(DisplayName = "O empréstimo deve ser pago em no mínimo 2 anos e no máximo 15 anos")]
    [InlineData("23")]
    [InlineData("181")]
    [InlineData("200")]
    public void MonthlyInstallmentsInvalid(string installments)
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","30000",installments
      });
      var proposal = new Proposal(ev);
      Assert.False(proposal.IsMonthlyInstallmentsValid());
    }

    [Fact(DisplayName = "Deve haver no mínimo 2 proponentes por proposta")]
    public void MinimumNumberProponents()
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","30000","72"
      });
      var proposal = new Proposal(ev);
      Assert.False(proposal.MinimumNumberProponents());

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","50","73300.95","true"
      })));
      Assert.False(proposal.MinimumNumberProponents());

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","50","73300.95","true"
      })));
      Assert.True(proposal.MinimumNumberProponents());
    }

    [Fact(DisplayName = "Deve haver exatamente 1 proponente principal por proposta")]
    public void SingleMainProponent()
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","30000","72"
      });
      var proposal = new Proposal(ev);

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","50","73300.95","true"
      })));

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","50","73300.95","true"
      })));
      Assert.False(proposal.SingleMainProponent());

      proposal.Proponents.Clear();
      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","50","73300.95","false"
      })));

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","50","73300.95","true"
      })));
      Assert.True(proposal.SingleMainProponent());
    }

    [Fact(DisplayName = "Todos os proponentes devem ser maiores de 18 anos")]
    public void ProponentAge()
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","30000","72"
      });
      var proposal = new Proposal(ev);

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","50","73300.95","true"
      })));

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","12","73300.95","true"
      })));
      Assert.False(proposal.ProponentAge());

      proposal.Proponents.Clear();

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","12","73300.95","true"
      })));

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","50","73300.95","true"
      })));
      Assert.False(proposal.ProponentAge());

      proposal.Proponents.Clear();

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","18","73300.95","true"
      })));

      proposal.Proponents.Add(new Proponent(new ProponentAddedEvent(new string[]{
        "05588a09-3268-464f-8bc8-03974303332a","proponent","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","5f9b96d2-b8db-48a8-a28b-f7ac9b3c8108","Kip Beer","18","73300.95","true"
      })));
      Assert.True(proposal.ProponentAge());
    }

    [Fact(DisplayName = "Dever haver no mínimo 1 garantia de imóvel por proposta")]
    public void MinimumWarrantiesValue()
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","30000","72"
      });
      var proposal = new Proposal(ev);
      var warranty = new Warranty(new WarrantyAddedEvent(new string[]
      {
        "27179730-5a3a-464d-8f1e-a742d00b3dd3","warranty","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","6b5fc3f9-bb6b-4145-9891-c7e71aa87ca2","60000","SP"
      }));
      proposal.Warranties.Add(warranty);
      Assert.True(proposal.MinimumWarrantiesValue());

      proposal.Warranties.Clear();
      Assert.False(proposal.MinimumWarrantiesValue());
    }

    [Fact(DisplayName = "A soma do valor das garantias deve ser maior ou igual ao dobro do valor do empréstimo")]
    public void MinimumWarrantiesNumber()
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","30000","72"
      });
      var proposal = new Proposal(ev);
      var warranty = new Warranty(new WarrantyAddedEvent(new string[]
      {
        "27179730-5a3a-464d-8f1e-a742d00b3dd3","warranty","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","6b5fc3f9-bb6b-4145-9891-c7e71aa87ca2","1967835.53","SP"
      }));
      proposal.Warranties.Add(warranty);
      Assert.True(proposal.MinimumWarrantiesNumber());

      proposal.Warranties.Clear();
      Assert.False(proposal.MinimumWarrantiesNumber());
    }

    [Theory(DisplayName = "As garantias de imóvel dos estados PR, SC e RS não são aceitas")]
    [InlineData("SP")]
    [InlineData("RJ")]
    [InlineData("MS")]
    [InlineData("MT")]
    [InlineData("GO")]
    [InlineData("AC")]
    [InlineData("BA")]
    [InlineData("ES")]

    public void ValidWarrantiesProvince(string province)
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","30000","72"
      });
      var proposal = new Proposal(ev);
      var warranty = new Warranty(new WarrantyAddedEvent(new string[]
      {
        "27179730-5a3a-464d-8f1e-a742d00b3dd3","warranty","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","6b5fc3f9-bb6b-4145-9891-c7e71aa87ca2","1967835.53",province
      }));
      proposal.Warranties.Add(warranty);
      Assert.True(proposal.ValidWarrantiesProvince());
    }

    [Theory(DisplayName = "As garantias de imóvel dos estados PR, SC e RS não são aceitas")]
    [InlineData("PR")]
    [InlineData("SC")]
    [InlineData("RS")]
    public void InvalidWarrantiesProvince(string province)
    {
      ProposalCreatedEvent ev = new ProposalCreatedEvent(new string[]{
        "c2d06c4f-e1dc-4b2a-af61-ba15bc6d8610","proposal","created","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","30000","72"
      });
      var proposal = new Proposal(ev);
      var warranty = new Warranty(new WarrantyAddedEvent(new string[]
      {
        "27179730-5a3a-464d-8f1e-a742d00b3dd3","warranty","added","2019-11-11T13:26:04Z","bd6abe95-7c44-41a4-92d0-edf4978c9f4e","6b5fc3f9-bb6b-4145-9891-c7e71aa87ca2","1967835.53",province
      }));
      proposal.Warranties.Add(warranty);
      Assert.False(proposal.ValidWarrantiesProvince());
    }

  }
}
