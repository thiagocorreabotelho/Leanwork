namespace Leanwork.Rh.Domain.Entitie;

public sealed class JobInterviewWeight
{
    public JobInterviewWeight()
    {
        
    }

    public JobInterviewWeight(int weightInterviewVacancyId, int technologyId, int jobOpeningId, int weight)
    {
        WeightInterviewVacancyId = weightInterviewVacancyId;
        TechnologyId = technologyId;
        JobOpeningId = jobOpeningId;
        Weight = weight;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
    }

    public int WeightInterviewVacancyId { get; private set; }
    public int TechnologyId { get; private set; }
    public int JobOpeningId { get; private set; }
    public int Weight { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ModificationDate { get; private set; }
}
