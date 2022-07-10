namespace GP_API.CustomExcecptions
{
    public class InvalidRequestDataException : Exception
    {
        public InvalidRequestDataException()
        {
        }

        public InvalidRequestDataException(string parameter)
            : base($"This parameter: {parameter} is invalid for this request!")
        {
        }
    }
}
