namespace GP_API.Repository.Interfaces
{
    using DapperExtensions.Predicate;

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets the by predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>TEntity</returns>
        public IEnumerable<TEntity> GetByPredicate(IPredicateGroup predicate);

        /// <summary>
        /// Gets the by predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>TEntity</returns>
        public IEnumerable<TEntity> GetByPredicate(IFieldPredicate predicate);
    }
}
