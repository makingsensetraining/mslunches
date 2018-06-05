using AutoMapper;
using MSLunches.Api.Models.Response;
using MSLunches.Data.Models;

namespace MSLunches.Api.Mapper
{
    public class MealTypeProfile : Profile
    {
        public MealTypeProfile()
        {
            CreateMap<MealType, MealTypeDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.IsSelectable, o => o.MapFrom(s => s.IsSelectable));
        }
    }
}
