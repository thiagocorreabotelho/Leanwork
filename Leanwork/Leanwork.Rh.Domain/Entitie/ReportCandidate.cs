namespace Leanwork.Rh.Domain.Entitie;

public sealed class ReportCandidate
{
    public ReportCandidate()
    {
        
    }
    public ReportCandidate(int candidateId, string fullName, string jobTitle, int totalScore)
    {
        CandidateId = candidateId;
        FullName = fullName;
        JobTitle = jobTitle;
        TotalScore = totalScore;
    }

    public int CandidateId { get; private set; }
    public string FullName { get; private set; }
    public string JobTitle { get; private set; }
    public int TotalScore { get; private set; }
}
