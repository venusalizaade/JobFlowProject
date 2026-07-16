namespace JobFlowProject.Business.Exceptions.Authentication_Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException()
        : base("User not found.")
    {
    }
   
    
}