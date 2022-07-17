namespace GP_API.Repository
{
    using Dapper;
    using GP_API.Models;
    using GP_API.Repository.Interfaces;
    using Microsoft.Extensions.Options;
    using System.Data.SqlClient;

    public class UsersRepository : GenericRepository<User>, IUserRepository
    {
		private readonly IOptions<AppSettings> _appSettings;
        public UsersRepository(IOptions<AppSettings> appSettings)
			:base(appSettings)
        {
			_appSettings = appSettings;
        }

        public int Add(User user)
        {
			const string sql = @"INSERT INTO USERS (
												Name
												,Email
												,Password
												,Username
												,DOB
												,ProfilePicture
												,CoverPicture
												,About
												)
											VALUES (
												@name
												,@email
												,@password
												,@username
												,@dob
												,@profilePicture
												,@coverPicture
												,@about
												)
								SELECT SCOPE_IDENTITY();";
			using var connection = new SqlConnection(_appSettings.Value.ConnectionString);
            {
				return connection.QueryFirst<int>(sql, new { 
															user.Name, 
															user.Email, 
															user.Password, 
															user.Username, 
															user.DOB, 
															user.ProfilePicture, 
															user.CoverPicture, 
															user.About  });
            }

		}

        public User Get(int id)
        {
			const string query = @"SELECT Id
										,Name
										,Email
										,DOB
										,ProfilePicture
										,CoverPicture
										,About
										,Password
									FROM Users
									WHERE Id = @id";
			using var connection = new SqlConnection(_appSettings.Value.ConnectionString);
			{
				return connection.QueryFirst<User>(query, new { id });
			}
		}
    }
}
