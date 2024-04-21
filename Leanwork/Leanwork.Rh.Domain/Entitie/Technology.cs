namespace Leanwork.Rh.Domain.Entitie;

public sealed class Technology
{
    public Technology(int technologyId, string name)
    {
        TechnologyId = technologyId;
        Name = name;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
    }

    private Technology(){}

    public int TechnologyId {  get; private set; }
    public string Name {  get; private set; }
    public DateTime CreationDate {  get; private set; }
    public DateTime ModificationDate {  get; private set; }
}
