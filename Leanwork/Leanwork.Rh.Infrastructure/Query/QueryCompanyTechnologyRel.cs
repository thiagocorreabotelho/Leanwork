namespace Leanwork.Rh.Infrastructure;

public static class QueryCompanyTechnologyRel
{
    public static string SelectAllTechnologyByCompany = $@"
        SELECT 
            technology.TechnologyId, 
            technology.Name
        FROM Companies companies
        INNER JOIN CompaniesTechnologiesRel ct ON companies.CompanyId = ct.CompanyId
        INNER JOIN Technologys technology ON technology.TechnologyId = ct.TechnologyId
        WHERE companies.CompanyId = @Id;
    ";

    public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION; 

            INSERT INTO CompaniesTechnologiesRel (CompanyId, TechnologyId, CreationDate, ModificationDate)
            VALUES (@CompanyId, @TechnologyId, @CreationDate, @ModificationDate);

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

                DELETE FROM CompaniesTechnologiesRel 
                WHERE CompanyTechnologyId = @CompanyTechnologyId;

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
