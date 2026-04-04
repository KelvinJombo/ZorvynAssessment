using AutoMapper;
using FinanceDashboard.Application.DTOs.Record;
using FinanceDashboard.Application.Interfaces.IServices;
using FinanceDashboard.Application.Interfaces.Repository;
using FinanceDashboard.Commons.Utilities;
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

        public async Task<Response<List<FinancialRecordResponseDto>>> GetAllAsync(string userId)
        {
            var records = await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Date)
                .Select(r => new FinancialRecordResponseDto
                {
                    Id = r.Id,
                    Amount = r.Amount,
                    Type = r.Type.ToString(),
                    Category = r.Category,
                    Date = r.Date,
                    Notes = r.Notes
                })
                .ToListAsync();

            return Response<List<FinancialRecordResponseDto>>.Success(
                records,
                ResponseMessages.Success
            );
        }

        public async Task<Response<FinancialRecordResponseDto>> CreateAsync(string userId, CreateFinancialRecordDto dto)
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

            var result = new FinancialRecordResponseDto
            {
                Id = record.Id,
                Amount = record.Amount,
                Type = record.Type.ToString(),
                Category = record.Category,
                Date = record.Date,
                Notes = record.Notes
            };

            return Response<FinancialRecordResponseDto>.Success(
                result,
                ResponseMessages.RecordCreated,
                StatusCodes.Created
            );
        }

        public async Task<Response<FinancialRecordResponseDto>> UpdateAsync(string id, string userId, UpdateFinancialRecordDto dto)
        {
            var record = await _unitOfWork.FinancialRecordRepository.GetByIdAsync(id);

            if (record == null)
                return Response<FinancialRecordResponseDto>.Failure(
                    ResponseMessages.RecordNotFound,
                    StatusCodes.NotFound
                );

            if (record.UserId != userId)
                return Response<FinancialRecordResponseDto>.Failure(
                    ResponseMessages.Unauthorized,
                    StatusCodes.Unauthorized
                );

            record.Amount = dto.Amount;
            record.Type = dto.Type;
            record.Category = dto.Category;
            record.Date = dto.Date;
            record.Notes = dto.Notes;

            _unitOfWork.FinancialRecordRepository.Update(record);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<FinancialRecordResponseDto>(record);

            return Response<FinancialRecordResponseDto>.Success(
                result,
                ResponseMessages.RecordUpdated
            );
        }


        public async Task<Response<string>> DeleteAsync(string id, string userId)
        {
            var record = await _unitOfWork.FinancialRecordRepository.GetByIdAsync(id);

            if (record == null)
                return Response<string>.Failure(
                    ResponseMessages.RecordNotFound,
                    StatusCodes.NotFound
                );

            if (record.UserId != userId)
                return Response<string>.Failure(
                    ResponseMessages.Unauthorized,
                    StatusCodes.Unauthorized
                );

            _unitOfWork.FinancialRecordRepository.Delete(record);
            await _unitOfWork.SaveChangesAsync();

            return Response<string>.Success(
                "Deleted",
                ResponseMessages.RecordDeleted
            );
        }
    }
}
