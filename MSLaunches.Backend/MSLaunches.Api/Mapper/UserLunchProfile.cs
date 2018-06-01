using AutoMapper;
using MSLunches.Api.Models.Request;
using MSLunches.Api.Models.Response;
using MSLunches.Data.Models;

namespace MSLunches.Api.Mapper
{
    public class UserLunchProfile : Profile
    {
        public UserLunchProfile()
        {
            CreateMap<UserLunch, UserLunchResponse>()
                .ForMember(s => s.Approved, o => o.MapFrom(s => s.Approved))
                .ForMember(s => s.CreatedOn, o => o.MapFrom(s => s.CreatedOn))
                .ForMember(s => s.Id, o => o.MapFrom(s => s.Id))
                .ForMember(s => s.LunchId, o => o.MapFrom(s => s.LunchId))
                .ForMember(s => s.UpdatedOn, o => o.MapFrom(s => s.UpdatedOn))
                .ForMember(s => s.UserId, o => o.MapFrom(s => s.UserId));

            CreateMap<UserLunchRequest, UserLunch>()
                .ForMember(s => s.Approved, o => o.MapFrom(s => s.Approved))
                .ForMember(s => s.LunchId, o => o.MapFrom(s => s.LunchId))
                .ForMember(s => s.UserId, o => o.MapFrom(s => s.UserId));
        }
    }
}
