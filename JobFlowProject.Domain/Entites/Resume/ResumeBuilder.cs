using JobFlowProject.Domain.Entites.User;

namespace JobFlowProject.Domain.Entites.Resume;

public class ResumeBuilder : BaseEntity
{
    
    public int Age { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Education { get; set; }
    public string? Experience { get; set; }
    public string? About { get; set; }
   
    public Guid ProfileId { get; set; }
    public AppUser AppUser { get; set; }
  
    public override void Validation()
    {
       
    
    }
}