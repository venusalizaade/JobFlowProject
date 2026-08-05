using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.AuthenticationExceptions;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JovFlowProject.JobMvc.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JovFlowProject.JobMvc.Controllers;

public class AccountController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAuthenticationService authenticationService,
        SignInManager<AppUser> signInManager,
        ILogger<AccountController> logger)
    {
        _authenticationService = authenticationService;
        _signInManager = signInManager;
        _logger = logger;
    }

    // GET
    [HttpGet]
    public IActionResult RegisterJobSeeker()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterJobSeeker(RegisterJobSeekerViewModel model)
    {
        try
        {
            await _authenticationService.JobSeekerRegisterAsync(model.ToCommand());
            return RedirectToAction(nameof(Login));
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "JobSeeker registration failed. NationalId:" +
                                   " {NationalId}", model.NationalId);
            TempData["Error"] = e.Message;
            return RedirectToAction(nameof(RegisterJobSeeker));
        }
    }

    [HttpGet]
    public IActionResult RegisterEmployer()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterEmployer(RegisterEmployerViewModel model)
    {
        try
        {
            await _authenticationService.EmployerRegisterAsync(model.ToCommand());
            return RedirectToAction(nameof(Login));
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Employer registration failed. NationalId: {NationalId}", model.NationalId);
            TempData["Error"] = e.Message;
            return RedirectToAction(nameof(RegisterEmployer));
        }
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, [FromQuery] string returnUrl)
    {
        try
        {
          
            await _authenticationService.LoginAsync(model.ToCommand());

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Login failed. Username: {UserName}", model.Username);
            TempData["Error"] = e.Message;
            return RedirectToAction(nameof(Login));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}

   
