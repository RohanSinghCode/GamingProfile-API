namespace GP_API.Repository
{
    using DapperExtensions;
    using DapperExtensions.Predicate;
    using GP_API.Models;
    using GP_API.Repository.Interfaces;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly AppSettings _appSettings;
        public GenericRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public IEnumerable<TEntity> GetByPredicate(IPredicateGroup predicate)
        {
            using (var connection = new SqlConnection(_appSettings.ConnectionString))
            {
                return connection.GetList<TEntity>(predicate).ToList();
            }
        }

        public IEnumerable<TEntity> GetByPredicate(IFieldPredicate predicate)
        {
            using (var connection = new SqlConnection(_appSettings.ConnectionString))
            {
                return connection.GetList<TEntity>(predicate).ToList();
            }
        }
    }
}
