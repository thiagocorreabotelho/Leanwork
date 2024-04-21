namespace Leanwork.Rh.Domain;

public sealed class Candidate
{

    private Candidate()
    {
        
    }

    public Candidate(int candidateId, int companyId, int genderId, string firstName, string lastName, string cPF, string rG, DateTime dateOfBirth)
    {
        CandidateId = candidateId;
        CompanyId = companyId;
        GenderId = genderId;
        FirstName = firstName;
        LastName = lastName;
        CPF = cPF;
        RG = rG;
        DateOfBirth = dateOfBirth;
        CreationDate = DateTime.Now;
        ModificationDate = DateTime.Now;
    }

    public int CandidateId { get; private set; }    
    public int CompanyId { get; private set; }    
    public int GenderId { get; private set; }    
    public string FirstName { get; private set; }    
    public string LastName { get; private set; }    
    public string CPF { get; private set; }    
    public string RG { get; private set; }    
    public DateTime DateOfBirth { get; private set; }    
    public DateTime CreationDate { get; private set; }    
    public DateTime ModificationDate { get; private set; }    
}
