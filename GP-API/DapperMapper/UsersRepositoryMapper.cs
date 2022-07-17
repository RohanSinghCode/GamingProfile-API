using DapperExtensions.Mapper;
using GP_API.Models;

namespace GP_API.DapperMapper
{
    public class UsersRepositoryMapper : ClassMapper<User>
    {
        public UsersRepositoryMapper()
        {
            Table("Users");
            AutoMap();
        }
    }
}
