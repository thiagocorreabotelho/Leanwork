namespace Leanwork.Rh.Domain;

public class JobOpening
{

    private JobOpening(){}

    public JobOpening(int jobOpeningId, string title, string summary, string description, bool available)
    {
        JobOpeningId = jobOpeningId;
        Title = title;
        Summary = summary;
        Description = description;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
        Available = available;
        
    }

    public int JobOpeningId {get; private set;}
    public string Title {get; private set;}
    public string Summary {get; private set;}
    public string Description {get; private set;}
    public DateTime CreationDate {get; private set;}
    public DateTime ModificationDate {get; private set;}
    public bool Available {get; private set;}

}
