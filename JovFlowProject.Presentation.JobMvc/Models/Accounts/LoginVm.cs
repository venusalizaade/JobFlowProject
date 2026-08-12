using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Commands;

namespace JovFlowProject.JobMvc.Models.Accounts;

public class LoginVm
{
    [Required(ErrorMessage = "Username is Required")]
    [Display(Name = "UserName")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    public string? ReturnUrl { get; set; }
    
    public LoginCommand ToCommand()
    {
        return new LoginCommand(Username, Password);
    }
}