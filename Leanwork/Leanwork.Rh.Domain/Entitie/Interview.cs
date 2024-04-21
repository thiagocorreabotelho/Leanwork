namespace Leanwork.Rh.Domain.Entitie;

public sealed class Interview
{
    public Interview()
    {
        
    }
    public Interview(int interviewId, int candidateId, int jobOpeningId)
    {
        InterviewId = interviewId;
        CandidateId = candidateId;
        JobOpeningId = jobOpeningId;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
    }

    public int InterviewId { get; private set; }
    public int CandidateId { get; private set; }
    public int JobOpeningId { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ModificationDate { get; private set; }
}
