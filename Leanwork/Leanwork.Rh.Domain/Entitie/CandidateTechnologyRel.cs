namespace Leanwork.Rh.Domain;

public sealed class CandidateTechnologyRel : spSelectById
{
    public CandidateTechnologyRel() { }

    public CandidateTechnologyRel(int candidateTechnologyRelId, int candidateId, int technologyId, string name)
    {
        CandidateTechnologyRelId = candidateTechnologyRelId;
        CandidateId = candidateId;
        TechnologyId = technologyId;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
       GetName(name);
    }

    public int CandidateTechnologyRelId { get; private set; }
    public int CandidateId { get; private set; }
    public int TechnologyId { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ModificationDate { get; private set; }

}

public partial class spSelectById
{
    public string Name { get; private set; }

    public void GetName(string name){
        Name = name;
    }
}
