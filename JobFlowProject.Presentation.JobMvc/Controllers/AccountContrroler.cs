using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JovFlowProject.JobMvc.Models.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JovFlowProject.JobMvc.Controllers;

public class AccountController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly JobFlowDbContext _dbContext;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAuthenticationService authenticationService,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        JobFlowDbContext dbContext,
        ILogger<AccountController> logger)
    {
        _authenticationService = authenticationService;
        _signInManager = signInManager;
        _userManager = userManager;
        _dbContext = dbContext;
        _logger = logger;
    }

    // ---------- Job Seeker Registration ----------

    [HttpGet]
    public IActionResult RegisterJobSeeker()
    {
        return View(new RegisterJobSeekerVm());
    }

    [HttpGet]
    public async Task<IActionResult> GetCities(Guid provinceId)
    {
        var cities = await _dbContext.Cities
            .AsNoTracking()
            .Where(c => c.ProvinceId == provinceId)
            .OrderBy(c => c.Name)
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();

        return Json(cities);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterJobSeeker(RegisterJobSeekerVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await _authenticationService.JobSeekerRegisterAsync(model.ToCommand());

            var user = await _userManager.FindByIdAsync(result.JobSeekerId.ToString());
            if (user is not null)
                await _signInManager.SignInAsync(user, isPersistent: false);

            TempData["Success"] = "ثبت‌نام شما با موفقیت انجام شد. خوش آمدید!";
            return RedirectToAction("Profile", "JobSeeker");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "JobSeeker registration failed. NationalId: {NationalId}", model.NationalId);
            ModelState.AddModelError(string.Empty, e.Message);
            return View(model);
        }
    }

    // ---------- Employer Registration ----------

    [HttpGet]
    public async Task<IActionResult> RegisterEmployer()
    {
        await PopulateLocationListsAsync();
        return View(new RegisterEmployerVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterEmployer(RegisterEmployerVm model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateLocationListsAsync(model.ProvinceId, model.CityId);
            return View(model);
        }

        try
        {
            await _authenticationService.EmployerRegisterAsync(model.ToCommand());

            TempData["Success"] =
                "ثبت‌نام شما با موفقیت انجام شد. پس از تأیید حساب توسط مدیر، می‌توانید وارد پنل کارفرما شوید.";
            return RedirectToAction(nameof(Login));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Employer registration failed. NationalId: {NationalId}", model.NationalId);
            ModelState.AddModelError(string.Empty, e.Message);
            await PopulateLocationListsAsync(model.ProvinceId, model.CityId);
            return View(model);
        }
    }

    // ---------- Login ----------

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = model.ReturnUrl;
            return View(model);
        }

        try
        {
            await _authenticationService.LoginAsync(model.ToCommand());

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return await RedirectByRoleAsync(model.Username);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Login failed. Username: {Username}", model.Username);
            ModelState.AddModelError(string.Empty, e.Message);
            ViewData["ReturnUrl"] = model.ReturnUrl;
            return View(model);
        }
    }

    private async Task<IActionResult> RedirectByRoleAsync(string nationalId)
    {
        var user = await _userManager.FindByNameAsync(nationalId);
        if (user is null)
            return RedirectToAction("Index", "Home");

        if (await _userManager.IsInRoleAsync(user, RoleConstants.AdminRoleName))
            return RedirectToAction("Dashboard", "Admin");

        if (await _userManager.IsInRoleAsync(user, RoleConstants.EmployerRoleName))
            return RedirectToAction("Dashboard", "Employer");

        if (await _userManager.IsInRoleAsync(user, RoleConstants.JobSeekerRoleName))
            return RedirectToAction("Profile", "JobSeeker");

        return RedirectToAction("Index", "Home");
    }

    // ---------- Logout / Access Denied ----------

    [Authorize]
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

    private async Task PopulateLocationListsAsync(Guid? selectedProvinceId = null, Guid? selectedCityId = null)
    {
        ViewBag.Provinces = new SelectList(
            await _dbContext.provinces.AsNoTracking().OrderBy(p => p.Name).ToListAsync(),
            "Id",
            "Name",
            selectedProvinceId);

        if (selectedProvinceId.HasValue)
        {
            ViewBag.Cities = new SelectList(
                await _dbContext.Cities.AsNoTracking()
                    .Where(c => c.ProvinceId == selectedProvinceId.Value)
                    .OrderBy(c => c.Name)
                    .ToListAsync(),
                "Id",
                "Name",
                selectedCityId);
        }
        else
        {
            ViewBag.Cities = new SelectList(
                await _dbContext.Cities.AsNoTracking().OrderBy(c => c.Name).ToListAsync(),
                "Id",
                "Name",
                selectedCityId);
        }
    }
}
