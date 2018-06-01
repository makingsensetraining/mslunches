using AutoMapper;
using MSLunches.Api.Models.Request;
using MSLunches.Api.Models.Response;
using MSLunches.Data.Models;

namespace MSLunches.Api.Mapper
{
    public class MealProfile : Profile
    {
        public MealProfile()
        {
            CreateMap<Meal, MealResponse>()
                .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedOn))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.TypeId, o => o.MapFrom(s => s.TypeId))
                .ForMember(d => d.UpdatedOn, o => o.MapFrom(s => s.UpdatedOn));

            CreateMap<MealRequest, Meal>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.TypeId, o => o.MapFrom(s => s.TypeId));
        }
    }
}
