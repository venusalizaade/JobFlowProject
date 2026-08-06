using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Commands;

namespace JovFlowProject.JobMvc.Models.Accounts;

public class LoginVm
{
    [Required(ErrorMessage = "NationalId is Required")]
    [Display(Name = "UserName")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;
    
    [Required]
    [StringLength(10)]
    public string NationalId { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
    
    public LoginCommand ToCommand()
    {
        return new LoginCommand(Username, Password);
    }
}