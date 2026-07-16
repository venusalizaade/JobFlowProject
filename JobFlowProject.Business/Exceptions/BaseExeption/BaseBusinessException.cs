namespace JobFlowProject.Business.Exceptions.BaseExeption;

public abstract class BaseBusinessException : Exception
{
    public string Code { get; }

    protected BaseBusinessException(string message, string code)
        : base(message)
    {
        Code = code;
    }
}