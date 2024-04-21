namespace Leanwork.Rh.Domain;

public sealed class Gender
{
    private Gender(){}

    public Gender(int genderId, string name)
    {
        GenderId = genderId;
        Name = name;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
    }

    public int GenderId { get; private set; }
    public string Name { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ModificationDate { get; private set; }
}
