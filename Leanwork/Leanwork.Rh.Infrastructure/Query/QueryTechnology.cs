namespace Leanwork.Rh.Infrastructure.Query;

public static class QueryTechnology
{
    public static string SelectAll = $@"
        SELECT 
            technology.TechnologyId,
            technology.Name,
            technology.CreationDate,
            technology.ModificationDate
        FROM dbo.Technologys technology
    ";

    public static string SelectById = $@"
        SELECT 
            technology.TechnologyId,
            technology.Name,
            technology.CreationDate,
            technology.ModificationDate
        FROM dbo.Technologys technology
        WHERE technology.TechnologyId = @Id
    ";

    public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION; -- Inicia a transação

            INSERT INTO Technologys (Name, CreationDate, ModificationDate)
            VALUES (@Name, @CreationDate, @ModificationDate);

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

                UPDATE Technologys 
                    SET 
                        Name = @Name, 
                        ModificationDate = @ModificationDate 
                    WHERE TechnologyId = @TechnologyId;

            SET @Id = @TechnologyId; 

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

                DELETE FROM Technologys 
                WHERE TechnologyId = @TechnologyId;

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
