namespace JobFlowProject.Business.Exceptions.Authentication_Exceptions;

public class RoleAssignmentException : Exception
{
    public RoleAssignmentException(string message)
        : base(message)
    {
    }
}