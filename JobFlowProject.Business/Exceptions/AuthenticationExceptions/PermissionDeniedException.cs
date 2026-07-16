using JobFlowProject.Business.Exceptions.BaseExeption;

namespace JobFlowProject.Business.Exceptions.AuthenticationExceptions;

public class PermissionDeniedException : BaseBusinessException
{
    public PermissionDeniedException()
        : base("Permission denied.", "PermissionDenied_403")
    {
    }

    public PermissionDeniedException(string message)
        : base(message, "PermissionDenied_403")
    {
    }
}