using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Commands;

namespace JovFlowProject.JobMvc.Models.Accounts;

public class LoginVm
{
    [Required(ErrorMessage = "نام کاربری الزامی است.")]
    [Display(Name = "نام کاربری")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "رمز عبور الزامی است.")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; } = null!;

    public string? ReturnUrl { get; set; }
    
    public LoginCommand ToCommand()
    {
        return new LoginCommand(Username, Password);
    }
}