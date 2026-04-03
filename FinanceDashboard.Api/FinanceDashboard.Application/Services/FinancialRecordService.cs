using FinanceDashboard.Application.DTOs;
using FinanceDashboard.Application.Interfaces.IServices;
using FinanceDashboard.Application.Interfaces.Repository;
using FinanceDashboard.Domain.Models;
using FinanceDashboard.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceDashboard.Application.Services
{
    public class FinancialRecordService : IFinancialRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FinancialRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FinancialRecordResponseDto>> GetAllAsync(string userId)
        {
            var records = await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            return records.Select(r => new FinancialRecordResponseDto
            {
                Id = r.Id,
                Amount = r.Amount,
                Type = r.Type.ToString(),
                Category = r.Category,
                Date = r.Date,
                Notes = r.Notes
            }).ToList();
        }

        public async Task<FinancialRecordResponseDto> CreateAsync(string userId, CreateFinancialRecordDto dto)
        {
            var record = new FinancialRecord
            {
                Amount = dto.Amount,
                Type = Enum.Parse<RecordType>(dto.Type),
                Category = dto.Category,
                Date = dto.Date,
                Notes = dto.Notes,
                UserId = userId
            };

            await _unitOfWork.FinancialRecordRepository.AddAsync(record);
            await _unitOfWork.SaveChangesAsync();

            return new FinancialRecordResponseDto
            {
                Id = record.Id,
                Amount = record.Amount,
                Type = record.Type.ToString(),
                Category = record.Category,
                Date = record.Date,
                Notes = record.Notes
            };
        }

        public async Task DeleteAsync(string id, string userId)
        {
            var record = await _unitOfWork.FinancialRecordRepository
                .Query()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (record == null)
                throw new Exception("Record not found");

            _unitOfWork.FinancialRecordRepository.Delete(record);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
