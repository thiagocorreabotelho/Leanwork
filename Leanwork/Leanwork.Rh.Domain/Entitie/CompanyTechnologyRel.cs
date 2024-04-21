namespace Leanwork.Rh.Domain;

public class CompanyTechnologyRel : SelectById
{

    private CompanyTechnologyRel(){}

    public CompanyTechnologyRel(int companyTechnologyRelId, int companyId, int technologyId, string name)
    {
        CompanyTechnologyRelId = companyTechnologyRelId;
        CompanyId = companyId;
        TechnologyId = technologyId;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
        GetName(name);
    }

    public int CompanyTechnologyRelId { get; private set; }
    public int CompanyId { get; private set; }
    public int TechnologyId { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ModificationDate { get; private set; }
}

public partial class SelectById
{
    public string Name { get; private set; }

    public void GetName(string name)
    {
        Name = name;
    }
}
