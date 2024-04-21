using Leanwork.Rh.Application.DTO.JobInterviewWeight;

namespace Leanwork.Rh.Application.Interface;

public interface IServiceJobInterviewWeight
{
    Task<int> Delete(int id);
    Task<int> Insert(JobInterviewWeightDTO jobInterviewWeightDTO);
}
