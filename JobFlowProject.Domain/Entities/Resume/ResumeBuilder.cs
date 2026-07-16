using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Entities.User;

namespace JobFlowProject.Domain.Entites.Resume;

public class ResumeBuilder : BaseEntity
{
    private ResumeBuilder() { }

    public ResumeBuilder( Guid appUserId)
    {
       
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