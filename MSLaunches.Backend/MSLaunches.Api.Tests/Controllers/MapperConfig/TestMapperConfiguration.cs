using AutoMapper;
using MSLunches.Api.Mapper;

namespace MSLunches.Api.Tests.Controllers.MapperConfig
{
    public class TestMapperConfiguration : MapperConfiguration
    {
        public TestMapperConfiguration() : base(
            cfg =>
            {
                cfg.AddProfile<LunchProfile>();
                cfg.AddProfile<MealProfile>();
                cfg.AddProfile<UserLunchProfile>();
                cfg.AddProfile<MealTypeProfile>();
            })
        {
        }
    }
}
