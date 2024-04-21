namespace Leanwork.Rh.Infrastructure;

public static class QueryResponsibility
{
    public static string SelectAll = $@"
        SELECT 
            responsibility.ResponsibilityId,
            responsibility.JobOpeningId,
            responsibility.Description,
            responsibility.CreationDate,
            responsibility.ModificationDate
        FROM Responsibilitys responsibility
    ";

    public static string SelectAllByJobOpening = $@"
        SELECT 
            responsibility.ResponsibilityId,
            responsibility.JobOpeningId,
            responsibility.Description,
            responsibility.CreationDate,
            responsibility.ModificationDate
        FROM Responsibilitys responsibility
        WHERE responsibility.JobOpeningId = @Id
    ";

   

    public static string SelectById = $@"
        SELECT 
            responsibility.JobOpeningId,
            responsibility.Description,
            responsibility.CreationDate,
            responsibility.ModificationDate
        FROM Responsibilitys responsibility
        WHERE responsibility.ResponsibilityId = @Id
    ";

    public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION;

            INSERT INTO Responsibilitys (JobOpeningId, Description, CreationDate, ModificationDate)
            VALUES (@JobOpeningId, @Description, @CreationDate, @ModificationDate);

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

                UPDATE Responsibilitys 
                    SET 
                       Description = @Description,
                       ModificationDate = ModificationDate
                    WHERE ResponsibilityId = @ResponsibilityId;

            SET @Id = @ResponsibilityId; 

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

                DELETE FROM Responsibilitys 
                WHERE ResponsibilityId = @ResponsibilityId;

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
