using JobFlowProject.Business.Exceptions.BaseExeption;

namespace JobFlowProject.Business.Exceptions.Authentication_Exceptions;

public class UserRegistrationException : BaseBusinessException
{
    public UserRegistrationException(string message)
        : base(message, "UserRegistration_400")
    {
    }
}