using FinanceDashboard.Application.DTOs;

namespace FinanceDashboard.Application.Interfaces.IServices
{
    public interface IFinancialRecordService
    {
        Task<List<FinancialRecordResponseDto>> GetAllAsync(string userId);

        Task<FinancialRecordResponseDto> CreateAsync(string userId, CreateFinancialRecordDto dto);

        Task DeleteAsync(string id, string userId);
    }
}
