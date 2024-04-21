namespace Leanwork.Rh.Infrastructure;

public static class QueryJobOpening
{
     public static string SelectAll = $@"
        SELECT 
            jobOpening.JobOpeningId,
            jobOpening.Title,
            jobOpening.Summary,
            jobOpening.Description,
            jobOpening.Available,
            jobOpening.CreationDate,
            jobOpening.ModificationDate
        FROM JobsOpenings jobOpening
    ";

     public static string SelectAllJobAvailable = $@"
        SELECT 
            jobOpening.JobOpeningId,
            jobOpening.Title,
            jobOpening.Summary,
            jobOpening.Description,
            jobOpening.CreationDate,
            jobOpening.ModificationDate
        FROM dbo.JobsOpenings jobOpening
        WHERE jobOpening.Available = 1
    ";

    public static string SelectById = @"
    
      SELECT 
            jobOpening.JobOpeningId,
            jobOpening.Title,
            jobOpening.Summary,
            jobOpening.Description,
            jobOpening.Available,
            jobOpening.CreationDate,
            jobOpening.ModificationDate
        FROM JobsOpenings jobOpening
        WHERE  jobOpening.JobOpeningId = @Id
    
    ";

    public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION;

            INSERT INTO JobsOpenings (Title, Summary, Description, Available, CreationDate, ModificationDate)
            VALUES (@Title, @Summary, @Description, @Available, @CreationDate, @ModificationDate)

            SET @Id = SCOPE_IDENTITY();

            COMMIT TRANSACTION; 
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION; 
            SET @Id = 0; 
        END CATCH;

        SELECT @Id 
    ";

    public static string Update = $@"
         BEGIN TRY
            BEGIN TRANSACTION; 

                UPDATE JobsOpenings 
                    SET 
                       Title = @Title,
                       Summary = @Summary,
                       Description = @Description,
                       Available = @Available,
                       ModificationDate = @ModificationDate
                    WHERE JobOpeningId = @JobOpeningId;

            SET @Id = @JobOpeningId; 

            COMMIT TRANSACTION; 
         END TRY
         BEGIN CATCH
            ROLLBACK TRANSACTION; 
            SET @Id = 0; 
         END CATCH;

         SELECT @Id ;
    ";

    public static string Delete = $@"
		 BEGIN TRY
            BEGIN TRANSACTION; 

                DELETE FROM JobsOpenings 
                WHERE JobOpeningId = @JobOpeningId;

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
