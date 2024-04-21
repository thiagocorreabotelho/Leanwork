using Leanwork.Rh.Application.DTO.Interview;

namespace Leanwork.Rh.Application.Interface;

public interface IServiceInterview
{
    Task<int> Delete(int id);
    Task<int> Insert(InterviewDTO interviewDTO);
}
