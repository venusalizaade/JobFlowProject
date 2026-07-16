namespace JobFlowProject.Business.Exceptions.Authentication_Exceptions;

public class UserRegistrationException : Exception
{
    public UserRegistrationException(string message)
        : base(message)
    {
    }
}