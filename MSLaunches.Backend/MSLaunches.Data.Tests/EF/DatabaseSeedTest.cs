using Microsoft.EntityFrameworkCore;
using MSLaunches.Data.EF;
using System.Threading.Tasks;
using Xunit;

namespace MSLaunches.Data.Tests.EF
{
    public class DatabaseMSLaunchesTest
    {
        [Fact]
        public async Task Initialize_ShouldCreateAUser()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WebApiCoreLaunchesContext>();
            optionsBuilder.UseInMemoryDatabase("GetByIdAsync_ShouldReturnUser");
            using (var dbContext = new WebApiCoreLaunchesContext(optionsBuilder.Options))
            {
                DatabaseMSLaunches.Initialize(dbContext);

                Assert.Equal(1, await dbContext.Users.CountAsync());
            }
        }
    }
}
