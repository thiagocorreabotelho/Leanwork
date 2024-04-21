namespace Leanwork.Rh.Infrastructure.Query;

public static class QueryJobInterviewWeight
{
    public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION;

            INSERT INTO JobsInterviewsWeight (TechnologyId, JobOpeningId, Weight, CreationDate, ModificationDate)
            VALUES (@TechnologyId, @JobOpeningId, @Weight, @CreationDate, @ModificationDate)

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

                DELETE FROM JobsInterviewsWeight 
                WHERE WeightInterviewVacancyId = @WeightInterviewVacancyId;

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
