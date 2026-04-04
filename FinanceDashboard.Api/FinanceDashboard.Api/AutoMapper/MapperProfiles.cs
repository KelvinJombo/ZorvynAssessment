using AutoMapper;
using FinanceDashboard.Api.Extensions;
using FinanceDashboard.Application.DTOs.Record;
using FinanceDashboard.Domain.Models;
using FinanceDashboard.Domain.Models.Enums;

namespace FinanceDashboard.Api.AutoMapper
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            // Map FinancialRecord to FinancialRecordResponseDto
            CreateMap<FinancialRecord, FinancialRecordResponseDto>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
             .ForMember(dest => dest.Type, opt => opt.ConvertUsing(new EnumToStringConverter<RecordType>(), src => src.Type))
             .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
             .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
             .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
             .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes ?? string.Empty))
             .ReverseMap()
             .ForMember(dest => dest.Type, opt => opt.ConvertUsing(new StringToEnumConverter<RecordType>(), src => src.Type));
            

            // Map CreateFinancialRecordDto to FinancialRecord
            CreateMap<CreateFinancialRecordDto, FinancialRecord>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes ?? string.Empty));

            // Map UpdateFinancialRecordDto to FinancialRecord (for updates)
            CreateMap<UpdateFinancialRecordDto, FinancialRecord>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes ?? string.Empty))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
