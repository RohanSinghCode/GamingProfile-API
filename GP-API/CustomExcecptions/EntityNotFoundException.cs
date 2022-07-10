namespace GP_API.CustomExcecptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(Type type, int id)
            : base($"{type} not found with id {id.ToString()}")
        {
        }
    }
}
