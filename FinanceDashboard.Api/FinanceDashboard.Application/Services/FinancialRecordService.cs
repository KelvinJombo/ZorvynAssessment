using AutoMapper;
using FinanceDashboard.Application.DTOs.Record;
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
        private readonly IMapper _mapper;

        public FinancialRecordService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<FinancialRecordResponseDto> UpdateAsync(string id, string userId, UpdateFinancialRecordDto dto)
        {
            var record = await _unitOfWork.FinancialRecordRepository.GetByIdAsync(id);

            if (record == null)
                throw new Exception("Record not found");

            //Ensure user owns the record or its Admin User
            if (record.UserId != userId && record.User.Role != UserRole.Admin)
                throw new UnauthorizedAccessException("You are not allowed to update this record");

            //Update fields
            record.Amount = dto.Amount;
            record.Type = dto.Type;
            record.Category = dto.Category;
            record.Date = dto.Date;
            record.Notes = dto.Notes;

            _unitOfWork.FinancialRecordRepository.Update(record);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FinancialRecordResponseDto>(record);
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
