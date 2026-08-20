using System.Security.Claims;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Interfaces.JobPost;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JovFlowProject.JobMvc.Models.Job;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dto.Authentication;

namespace JovFlowProject.JobMvc.Controllers;

public class JobPostController : Controller
{
    private readonly IJobPostService _jobPostService;
    private readonly IWalletService _walletService;
    private readonly ISavedJobService _savedJobService;
    private readonly JobFlowDbContext _dbContext;

    public JobPostController(
        IJobPostService jobPostService,
        IWalletService walletService,
        ISavedJobService savedJobService,
        JobFlowDbContext dbContext)
    {
        _jobPostService = jobPostService;
        _walletService = walletService;
        _savedJobService = savedJobService;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var jobs = await _jobPostService.GetActiveAsync();
        return View(jobs);
    }

    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> MyJobs()
    {
        var requesterId = GetUserId();
        var jobs = await _jobPostService.GetCompanyJobPostsAsync(requesterId);

        return View(jobs);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var job = await _jobPostService.GetDetailsAsync(id);

        if (User.Identity?.IsAuthenticated ?? false)
        {
            var isSeeker = User.IsInRole("JobSeeker");
            ViewBag.IsJobSeeker = isSeeker;

            if (isSeeker)
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewBag.IsSaved = await _savedJobService.IsSavedAsync(userId, id);
            }
        }

        return View(job);
    }

    [HttpPost]
    [Authorize(Policy = "JobSeeker")]
    public async Task<IActionResult> ToggleSave(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (await _savedJobService.IsSavedAsync(userId, id))
            await _savedJobService.UnsaveAsync(userId, id);
        else
            await _savedJobService.SaveAsync(userId, id);

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> Create()
    {
        await PopulateLookupsAsync();
        return View();
    }

    [HttpPost]
    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> Create(CreateJobPostVm model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateLookupsAsync(model.ProvinceId, model.CategoryId, model.SkillId);
            return View(model);
        }

        var requesterId = GetUserId();
        var result = await _jobPostService.CreateAsync(requesterId, model.ToCommand());

        if (model.FeatureId.HasValue)
        {
            try
            {
                await _walletService.PurchaseFeatureAsync(requesterId, result.Id, model.FeatureId.Value);
                TempData["Success"] = "آگهی ثبت شد و فیچر خریداری شده در انتظار تایید ادمین است.";
            }
            catch (Exception ex)
            {
                TempData["Success"] = "آگهی شما با موفقیت ثبت شد.";
                TempData["Error"] = ex.Message;
            }
        }
        else
        {
            TempData["Success"] = "آگهی شما با موفقیت ثبت شد.";
        }

        return RedirectToAction(nameof(MyJobs));
    }

    [HttpPost]
    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> RequestFeature(Guid jobPostId, Guid featureId)
    {
        var requesterId = GetUserId();

        try
        {
            await _walletService.PurchaseFeatureAsync(requesterId, jobPostId, featureId);
            TempData["Success"] = "فیچر خریداری شد و در انتظار تایید ادمین است.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(MyJobs));
    }

    [HttpGet]
    public async Task<IActionResult> GetCities(Guid provinceId)
    {
        var cities = await _dbContext.Cities
            .Where(c => c.ProvinceId == provinceId)
            .OrderBy(c => c.Name)
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();

        return Json(cities);
    }

    private async Task PopulateLookupsAsync(
        Guid? selectedProvince = null,
        Guid? selectedCategory = null,
        Guid? selectedSkill = null)
    {
        ViewBag.Categories = new SelectList(
            await _dbContext.Categories.Where(x => !x.IsDeleted).OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = x.Id == selectedCategory
            }).ToListAsync(), "Value", "Text");

        ViewBag.Skills = new SelectList(
            await _dbContext.Skills.Where(x => !x.IsDeleted).OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = x.Id == selectedSkill
            }).ToListAsync(), "Value", "Text");

        ViewBag.Provinces = new SelectList(
            await _dbContext.provinces.OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = x.Id == selectedProvince
            }).ToListAsync(), "Value", "Text");

        ViewBag.Features = new SelectList(
            await _dbContext.Features.Where(x => !x.IsDeleted).OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " — " + x.Price.ToString("N0") + " تومان"
            }).ToListAsync(), "Value", "Text");

        if (selectedProvince.HasValue)
        {
            ViewBag.Cities = new SelectList(
                await _dbContext.Cities.Where(x => x.ProvinceId == selectedProvince.Value).OrderBy(x => x.Name).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync(), "Value", "Text");
        }
    }

    [HttpGet]
    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var requesterId = GetUserId();

        var entity = await _dbContext.JobPosts
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (entity is null || entity.Company?.AppUserId != requesterId)
            return NotFound();

        var model = new EditJobPostVm
        {
            Id = entity.Id,
            Title = entity.Title,
            AboutJob = entity.AboutJob,
            Salary = entity.Salary,
            EmploymentType = entity.EmploymentType,
            ProvinceId = entity.ProvinceId,
            CityId = entity.CityId,
            CategoryId = entity.CategoryId,
            SkillId = entity.SkillId
        };

        await PopulateLookupsAsync(model.ProvinceId, model.CategoryId, model.SkillId);

        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> Edit(EditJobPostVm model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateLookupsAsync(model.ProvinceId, model.CategoryId, model.SkillId);
            return View(model);
        }

        var requesterId = GetUserId();

        try
        {
            await _jobPostService.UpdateAsync(requesterId, model.Id, model.ToCommand());
            TempData["Success"] = "آگهی با موفقیت ویرایش شد.";
        }
        catch (JobFlowProject.Business.Exceptions.BaseExeption.BaseBusinessException)
        {
            TempData["Error"] = "این آگهی یافت نشد یا به شما تعلق ندارد.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(MyJobs));
    }

    [HttpPost]
    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var requesterId = GetUserId();

        try
        {
            await _jobPostService.DeactivateAsync(requesterId, id);
            TempData["Success"] = "آگهی با موفقیت غیرفعال شد.";
        }
        catch (JobFlowProject.Business.Exceptions.BaseExeption.BaseBusinessException)
        {
            TempData["Error"] = "این آگهی یافت نشد یا به شما تعلق ندارد.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(MyJobs));
    }

    [HttpGet]
    public async Task<IActionResult> Search(JobPostSearchRequestDto dto)
    {
        var jobs = await _jobPostService.SearchAsync(dto);
        return View("Index", jobs);
    }

    [HttpGet]
    public async Task<IActionResult> Filter(JobPostFilterRequestDto dto)
    {
        var jobs = await _jobPostService.FilterAsync(dto);
        return View("Index", jobs);
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }   }