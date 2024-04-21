namespace Leanwork.Rh.Infrastructure;

public static class QueryCandidate
{
    public static string SelectAll = $@"
        SELECT 
            candidate.CandidateId,
            candidate.CompanyId,
            candidate.GenderId,
            candidate.FirstName,
            candidate.LastName,
            candidate.CPF,
            candidate.RG,
            candidate.DateOfBirth,
            candidate.CreationDate,
            candidate.ModificationDate
        FROM dbo.Candidates candidate
    ";

    public static string SelectById = $@"
        SELECT 
            candidate.CandidateId,
            candidate.CompanyId,
            candidate.GenderId,
            candidate.FirstName,
            candidate.LastName,
            candidate.CPF,
            candidate.RG,
            candidate.DateOfBirth,
            candidate.CreationDate,
            candidate.ModificationDate
        FROM dbo.Candidates candidate
        WHERE candidate.CandidateId = @Id
    ";

    public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION; 

            INSERT INTO Candidates (CompanyId, GenderId, FirstName, LastName, CPF, RG, DateOfBirth, CreationDate, ModificationDate)
            VALUES (@CompanyId, @GenderId, @FirstName, @LastName, @CPF, @RG, @DateOfBirth, @CreationDate, @ModificationDate);

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

                UPDATE Candidates 
                    SET 
                        FirstName = @FirstName, 
                        LastName = @LastName, 
                        CPF = @CPF, 
                        RG = @RG, 
                        DateOfBirth = @DateOfBirth, 
                        ModificationDate = @ModificationDate 
                WHERE CandidateId = @CandidateId;

                SET @Id = @CandidateId; 

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

                DELETE FROM Candidates 
                WHERE CandidateId = @CandidateId;

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
