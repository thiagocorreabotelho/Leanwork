namespace Leanwork.Rh.Infrastructure;

public static class QueryGender
{
    public static string SelectAll = $@"
    
      SELECT 
        gender.GenderId,
        gender.Name,
        gender.CreationDate,
        gender.ModificationDate
      FROM dbo.Genders gender
    ";

    public static string SelectById = @"
         SELECT 
            gender.GenderId,
            gender.Name,
            gender.CreationDate,
            gender.ModificationDate
         FROM dbo.Genders gender
         WHERE gender.GenderId = @Id
    ";

    public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION; 

            INSERT INTO Genders (Name, CreationDate, ModificationDate)
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

                UPDATE Genders 
                    SET 
                        Name = @Name, 
                        ModificationDate = @ModificationDate
                WHERE GenderId = @GenderId;

                SET @Id = @GenderId; 

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

                DELETE FROM Genders 
                WHERE GenderId = @GenderId;

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
