namespace Leanwork.Rh.Infrastructure;

public static class QueryCompany
{
    public static string SelectAll = $@"
    
      SELECT
        company.CompanyId,
        company.Name,
        company.CNPJ,
        company.OpenDate,
        company.Email,
        company.CreationDate,
        company.ModificationDate
      FROM dbo.Companies company
    ";

    public static string SelectById = @"
        SELECT
            company.CompanyId,
            company.Name,
            company.CNPJ,
            company.OpenDate,
            company.Email,
            company.CreationDate,
            company.ModificationDate
        FROM dbo.Companies company
        WHERE company.CompanyId = @Id
    ";

    public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION; 

            INSERT INTO Companies (Name, CNPJ, OpenDate, Email, CreationDate, ModificationDate)
            VALUES (@Name, @CNPJ, @OpenDate, @Email, @CreationDate, @ModificationDate);

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

                UPDATE Companies 
                    SET 
                        Name = @Name, 
                        CNPJ = @CNPJ,
                        OpenDate = @OpenDate,
                        Email = @Email,
                        ModificationDate = @ModificationDate
                WHERE CompanyId = @CompanyId;

                SET @Id = @CompanyId; 

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

                DELETE FROM Companies 
                WHERE CompanyId = @CompanyId;

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
