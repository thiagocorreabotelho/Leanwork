namespace Leanwork.Rh.Domain;

public class Responsibility
{

    public Responsibility(){}

    public Responsibility(int responsibilityId, string description, int jobOpeningId)
    {
        ResponsibilityId = responsibilityId;
        Description = description;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
        JobOpeningId = jobOpeningId;
    }

    public int ResponsibilityId {get; private set;}
    public int JobOpeningId { get; private set; }
    public string Description {get; private set;}
    public DateTime CreationDate {get; private set;}
    public DateTime ModificationDate {get; private set;}
}
