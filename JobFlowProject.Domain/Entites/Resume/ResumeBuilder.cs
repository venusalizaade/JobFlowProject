using JobFlowProject.Domain.Entites.User;

namespace JobFlowProject.Domain.Entites.Resume;

public class ResumeBuilder : BaseEntity
{
    private ResumeBuilder() { }

    public ResumeBuilder(int age,string firstname, string lastName, string email, string phoneNumber, Guid appUserId)
    {
        Age = age;
        AppUser.FirstName = firstname;
        AppUser.LastName= lastName;
        AppUser.Email = email;
        AppUser.PhoneNumber = phoneNumber;
        AppUser.Id = appUserId;
       
    }
    
    public int Age { get; private set; }
    public string? Address { get; private set; }
    public string? City { get; private set; }
    public string? Education { get; private set; }
    public string? Experience { get; private set; }
    public string? About { get; private set; }
   
    public Guid AppUserId { get; private set; }
    public AppUser AppUser { get; private set; }


    public override void Validate()
    {
        throw new NotImplementedException();
    }
}