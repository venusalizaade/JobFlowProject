namespace JobFlowProject.Business.Dto.User;

public class AppUserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Username { get; set; }
    public string NationalId { get; set; }
    public string? Email { get; set; }
}