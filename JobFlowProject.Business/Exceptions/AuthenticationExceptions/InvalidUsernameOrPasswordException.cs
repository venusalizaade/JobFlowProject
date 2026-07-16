namespace JobFlowProject.Business.Exceptions.Authentication_Exceptions;

public class InvalidUsernameOrPasswordException : Exception
{
    public InvalidUsernameOrPasswordException()
        : base("Invalid username or password.")
    {
    }
}