namespace Leanwork.Rh.Infrastructure;

public static class QueryAddress
{
     public static string SelectAllByCompany = $@"
        SELECT 
            address.AddressId,
            address.CompanyId,
            address.Name,
            address.ZipCode,
            address.Street,
            address.Number,
            address.Complement,
            address.Neighborhood,
            address.City,
            address.State
        FROM Address address
        WHERE address.CompanyId = @Id
    ";

     public static string SelectAllByCandidate = $@"
        SELECT 
            address.AddressId,
            address.CandidateId,
            address.Name,
            address.ZipCode,
            address.Street,
            address.Number,
            address.Complement,
            address.Neighborhood,
            address.City,
            address.State
        FROM Address address
        WHERE address.CandidateId = @Id
    ";

     public static string SelectById = $@"
        SELECT 
            address.AddressId,
            address.CandidateId,
            address.Name,
            address.ZipCode,
            address.Street,
            address.Number,
            address.Complement,
            address.Neighborhood,
            address.City,
            address.State
        FROM Address address
        WHERE address.AddressId = @Id
    ";

     public static string Insert = $@"
        BEGIN TRY
            BEGIN TRANSACTION; 

            IF @CandidateId = 0
            BEGIN
                SET @CandidateId = null;
            END

            IF @CompanyId = 0
            BEGIN
                SET @CompanyId = null;
            END

            INSERT INTO Address (CompanyId, CandidateId, Name, ZipCode, Street, Number, Complement, Neighborhood, City, State, CreationDate, ModificationDate)
            VALUES (@CompanyId, @CandidateId, @Name, @ZipCode, @Street, @Number, @Complement, @Neighborhood, @City, @State, @CreationDate, @ModificationDate)

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

                IF @CandidateId = 0
                BEGIN
                    SET @CandidateId = null;
                END

                IF @CompanyId = 0
                BEGIN
                    SET @CompanyId = null;
                END

                UPDATE Address 
                    SET 
                        CompanyId = @CompanyId,
                        CandidateId = @CandidateId,
                        Name = @Name,
                        ZipCode = @ZipCode,
                        Street = @Street,
                        Number = @Number,
                        Complement = @Complement,
                        Neighborhood = @Neighborhood,
                        City = @City,
                        State = @State,
                        ModificationDate = @ModificationDate
                WHERE AddressId = @AddressId;

                SET @Id = @AddressId; 

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

                DELETE FROM Address 
                WHERE AddressId = @AddressId;

                SET @Id = 1; 

                COMMIT TRANSACTION; 
            END TRY
            BEGIN CATCH
                ROLLBACK TRANSACTION; 
                SET @Id = 0; 
            END CATCH;

            SELECT @Id ;
	    ";

    public static string DeleteAllCompany = $@"
		 BEGIN TRY
            BEGIN TRANSACTION; 

                DELETE FROM Address 
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

     public static string DeleteAllCandidate = $@"
		 BEGIN TRY
            BEGIN TRANSACTION; 

                DELETE FROM Address 
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
