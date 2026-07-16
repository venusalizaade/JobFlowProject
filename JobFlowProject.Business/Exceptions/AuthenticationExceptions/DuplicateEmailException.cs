namespace JobFlowProject.Business.Exceptions.Authentication_Exceptions;

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException()
        : base("Email already exists.")
    {
    }
}