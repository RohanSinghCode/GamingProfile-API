namespace GP_API.Repository.Interfaces
{
    using GP_API.Models;

    public interface IUserRepository
    {
        /// <summary>
        /// Adds the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>Added user</returns>
        int Add(User user);

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>User</returns>
        User Get(int id);
    }
}
