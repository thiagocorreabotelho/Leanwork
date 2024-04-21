namespace Leanwork.Rh.Infrastructure.Query;

public static class QueryReportCandidate
{
    public static string SelectReportCandidate = @"
        SELECT 
            c.CandidateId,
            c.FirstName + ' ' + c.LastName AS FullName,
            j.Title AS JobTitle,
            SUM(w.Weight) AS TotalScore
        FROM dbo.Candidates AS c
        JOIN dbo.CandidatesTechnologysRel AS ctr ON c.CandidateId = ctr.CandidateId
        JOIN dbo.Technologys AS t ON ctr.TechnologyId = t.TechnologyId
        JOIN dbo.JobsInterviewsWeight AS w ON t.TechnologyId = w.TechnologyId
        JOIN dbo.JobsOpenings AS j ON w.JobOpeningId = j.JobOpeningId
        WHERE j.Available = 1  
        GROUP BY  c.CandidateId, c.FirstName, c.LastName, j.Title
        ORDER BY  TotalScore DESC;
    ";
}
