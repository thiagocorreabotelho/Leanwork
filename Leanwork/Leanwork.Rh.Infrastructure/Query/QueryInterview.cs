namespace Leanwork.Rh.Infrastructure.Query;
public static class QueryInterview
{
    public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION;

            INSERT INTO Interviews (CandidateId, JobOpeningId, CreationDate, ModificationDate)
            VALUES (@CandidateId, @JobOpeningId, @CreationDate, @ModificationDate)

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

                DELETE FROM Interviews 
                WHERE InterviewId = @InterviewId;

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
