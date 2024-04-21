namespace Leanwork.Rh.Infrastructure;

public static class QueryCandidateTechnologyRel
{
    public static string SelectAllTechnologyByCandidate = $@"
        SELECT 
            technology.TechnologyId, 
            technology.Name
        FROM Candidates candidate
        INNER JOIN CandidatesTechnologysRel ct ON candidate.CandidateId = ct.CandidateId
        INNER JOIN Technologys technology ON technology.TechnologyId = ct.TechnologyId
        WHERE candidate.CandidateId = @Id;
    ";

     public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION; 

            INSERT INTO CandidatesTechnologysRel (CandidateId, TechnologyId, CreationDate, ModificationDate)
            VALUES (@CandidateId, @TechnologyId, @CreationDate, @ModificationDate);

            SET @Id = SCOPE_IDENTITY();

            COMMIT TRANSACTION; 
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION; 
            SET @Id = 0; 
        END CATCH;

        SELECT @Id 
    ";

     public static string Delete = $@"
		 BEGIN TRY
            BEGIN TRANSACTION; 

                DELETE FROM CandidatesTechnologysRel 
                WHERE CandidateTechnologyId = @CandidateTechnologyId;

                SET @Id = 1; 

                COMMIT TRANSACTION; 
            END TRY
            BEGIN CATCH
                ROLLBACK TRANSACTION; 
                SET @Id = 0; 
            END CATCH;

            SELECT @Id ;
	    ";
}
