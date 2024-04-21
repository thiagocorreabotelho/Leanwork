namespace Leanwork.Rh.Domain;

public class Address
{
    private Address (){}
    
    public Address(int addressId, int companyId, int candidateId, string name, string zipCode, string street, string number, string complement, string neighborhood, string city, string state)
    {
        AddressId = addressId;
        CompanyId = companyId;
        CandidateId = candidateId;
        Name = name;
        ZipCode = zipCode;
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
    }

    public int AddressId { get; private set; }
    public int CompanyId {get; private set; }   
    public int CandidateId { get; private set; }
    public string Name { get; private set; }
    public string ZipCode { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ModificationDate { get; private set; }

}
