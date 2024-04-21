namespace Leanwork.Rh.Domain;

public sealed class Company
{
    private Company() {}

    public Company(int companyId, string name, string cNPJ, DateTime openDate, string email)
    {
        CompanyId = companyId;
        Name = name;
        CNPJ = cNPJ;
        OpenDate = openDate;
        Email = email;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
    }

    public int CompanyId { get; private set; }
    public string Name { get; private set; }
    public string CNPJ { get; private set; }
    public DateTime OpenDate { get; private set; }
    public string Email { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ModificationDate { get; private set; }
}
