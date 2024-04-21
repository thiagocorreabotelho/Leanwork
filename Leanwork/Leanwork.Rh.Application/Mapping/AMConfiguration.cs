using AutoMapper;
using Leanwork.Rh.Application.DTO.Interview;
using Leanwork.Rh.Application.DTO.JobInterviewWeight;
using Leanwork.Rh.Application.DTO.ReportCandidate;
using Leanwork.Rh.Application.DTO.Technology;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Entitie;

namespace Leanwork.Rh.Application.Mapping;

public class AMConfiguration : Profile
{
    public AMConfiguration()
    {
        CreateMap<TechnologyDTO, CompanyTechnologyRelDTO>().ReverseMap();
        CreateMap<TechnologyDTO, CandidateTechnologyRelDTO>().ReverseMap();
        CreateMap<TechnologyDTO, Technology>().ReverseMap();
        CreateMap<CompanyDTO, Company>().ReverseMap();
        CreateMap<CandidateDTO, Candidate>().ReverseMap();
        CreateMap<GenderDTO, Gender>().ReverseMap();
        CreateMap<AddressDTO, Address>().ReverseMap();
        CreateMap<CandidateTechnologyRelDTO, CandidateTechnologyRel>().ReverseMap();
        CreateMap<CompanyTechnologyRelDTO, CompanyTechnologyRel>().ReverseMap();
        CreateMap<JobOpeningDTO, JobOpening>().ReverseMap();
        CreateMap<ResponsibilityDTO, Responsibility>().ReverseMap();
        CreateMap<Interview, InterviewDTO>().ReverseMap();
        CreateMap<JobInterviewWeight, JobInterviewWeightDTO>().ReverseMap();
        CreateMap<ReportCandidate, ReportCandidateDTO>().ReverseMap();
    }
}
