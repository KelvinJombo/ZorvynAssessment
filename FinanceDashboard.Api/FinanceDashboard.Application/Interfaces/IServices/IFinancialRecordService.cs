using FinanceDashboard.Application.DTOs.Record;
using FinanceDashboard.Commons.Utilities;

namespace FinanceDashboard.Application.Interfaces.IServices
{    
    public interface IFinancialRecordService
    {
        Task<Response<FinancialRecordResponseDto>> CreateAsync(string userId, CreateFinancialRecordDto dto);
        Task<Response<List<FinancialRecordResponseDto>>> GetAllAsync(string userId);
        Task<Response<string>> DeleteAsync(string id, string userId);
        Task<Response<FinancialRecordResponseDto>> UpdateAsync(string id, string userId, UpdateFinancialRecordDto dto);
    }
}
