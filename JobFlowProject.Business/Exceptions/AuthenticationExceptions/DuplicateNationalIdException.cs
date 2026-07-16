namespace JobFlowProject.Business.Exceptions.Authentication_Exceptions;

public class DuplicateNationalIdException : Exception
{
    public DuplicateNationalIdException()
        : base("NationalId already exists.")
    {
    }
}