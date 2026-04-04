using FinanceDashboard.Application.DTOs.Record;
using FinanceDashboard.Domain.Models;

namespace FinanceDashboard.Application.Interfaces.IServices
{
    public interface IFinancialRecordService
    {
        Task<List<FinancialRecordResponseDto>> GetAllAsync(string userId);

        Task<FinancialRecordResponseDto> CreateAsync(string userId, CreateFinancialRecordDto dto);
        Task<FinancialRecordResponseDto> UpdateAsync(string id, string userId, UpdateFinancialRecordDto dto);
        Task DeleteAsync(string id, string userId);
    }
}
