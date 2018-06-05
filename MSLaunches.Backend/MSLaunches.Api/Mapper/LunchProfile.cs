using AutoMapper;
using MSLunches.Api.Models.Request;
using MSLunches.Api.Models.Response;
using MSLunches.Data.Models;
using System;

namespace MSLunches.Api.Mapper
{
    public class LunchProfile : Profile
    {
        public LunchProfile()
        {
            CreateMap<Lunch, LunchDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Meal, o => o.MapFrom(s => s.Meal))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date))
                .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedOn))
                .ForMember(d => d.UpdatedOn, o => o.MapFrom(s => s.UpdatedOn));

            CreateMap<InputLunchDto, Lunch>()
                .ForMember(d => d.MealId, o => o.MapFrom(s => s.MealId))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date));

            CreateMap<InputBatchLunchDto, Lunch>()
                .ForMember(d => d.MealId, o => o.MapFrom(s => s.MealId))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id ?? Guid.Empty));
        }
    }
}