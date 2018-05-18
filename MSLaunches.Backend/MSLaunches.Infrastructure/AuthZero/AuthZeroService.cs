using Auth0.Core;
using Auth0.Core.Exceptions;
using Auth0.ManagementApi.Models;
using MSLaunches.Infrastructure.Result;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSLaunches.Infrastructure.AuthZero
{
    public class AuthZeroService : IAuthZeroService
    {
        /// <summary> Auht0 connection used to store credentials </summary>
        private const string UserPasswordConnection = "Username-Password-Authentication";

        private readonly IAuthZeroClient _authZeroClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthZeroService"/> class.
        /// </summary>
        /// <param name="authZeroClient"> Configured Auth0 client </param>
        public AuthZeroService(IAuthZeroClient authZeroClient)
        {
            _authZeroClient = authZeroClient;
        }

        /// <inheritdoc/>
        public async Task<Result<User, ErrorResult>> CreateUserAsync(string email, string password, string[] roles)
        {
            var getClientResult = await _authZeroClient.GetManagementApiClient();
            if (!getClientResult.IsSuccessResult)
                return new Result<User, ErrorResult>(InfrastructureError.Auth0ManagementApiAuthenticationFailed);

            var managementApiClient = getClientResult.SuccessValue;
            var userCreateParameters = new UserCreateRequest
            {
                Email = email,
                Password = password,
                Connection = UserPasswordConnection,
                AppMetadata = new
                {
                    Authorization = new
                    {
                        Groups = new object[] { },
                        Roles = roles,
                        Permissions = new object[] { }
                    }
                }
            };

            try
            {
                var user = await managementApiClient.Users.CreateAsync(userCreateParameters);
                return new Result<User, ErrorResult>(user);
            }
            catch (ApiException e)
            {
                return new Result<User, ErrorResult>(new ErrorResult(e.ApiError.ErrorCode, e.ApiError.Message));
            }
        }

        public async Task<Result<IList<User>, ErrorResult>> GetUsers()
        {
            var getClientResult = await _authZeroClient.GetManagementApiClient();
            if (!getClientResult.IsSuccessResult)
                return new Result<IList<User>, ErrorResult>(InfrastructureError.Auth0ManagementApiAuthenticationFailed);

            var managementApiClient = getClientResult.SuccessValue;

            try
            {
                var users = await managementApiClient.Users.GetAllAsync();
                return new Result<IList<User>, ErrorResult>(users.ToList());
            }
            catch(ApiException e)
            {
                return new Result<IList<User>, ErrorResult>(new ErrorResult(e.ApiError.ErrorCode, e.ApiError.Message));
            }

        }
    }
}
