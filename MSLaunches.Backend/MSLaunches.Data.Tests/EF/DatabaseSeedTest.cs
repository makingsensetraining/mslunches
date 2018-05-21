using Microsoft.EntityFrameworkCore;
using MSLunches.Data.EF;
using System.Threading.Tasks;
using Xunit;

namespace MSLunches.Data.Tests.EF
{
    public class DatabaseMSLunchesTest
    {
        [Fact]
        public async Task Initialize_ShouldCreateAUser()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WebApiCoreLunchesContext>();
            optionsBuilder.UseInMemoryDatabase("GetByIdAsync_ShouldReturnUser");
            using (var dbContext = new WebApiCoreLunchesContext(optionsBuilder.Options))
            {
                DatabaseMSLunches.Initialize(dbContext);

                Assert.Equal(1, await dbContext.Users.CountAsync());
            }
        }
    }
}
