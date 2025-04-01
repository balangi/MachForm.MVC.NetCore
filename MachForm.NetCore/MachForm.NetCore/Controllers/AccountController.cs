using MachForm.NetCore.Models.Account;
using MachForm.NetCore.Models.MainSettings;
using MachForm.NetCore.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MachForm.NetCore.Controllers;

// index.php
[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<UserDto> _userManager;
    private readonly SignInManager<UserDto> _signInManager;
    private readonly ILdapService _ldapService;
    private readonly IUserService _userService;
   // private readonly IOptions<MachFormSettings> _settings;
    private readonly IOptions<MainSettingsViewModel> _settings;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<UserDto> userManager,
        SignInManager<UserDto> signInManager,
        ILdapService ldapService,
        IUserService userService,
        IOptions<MainSettingsViewModel> settings,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _ldapService = ldapService;
        _userService = userService;
        _settings = settings;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string returnUrl = "")
    {
        // Check if tables exist, if not redirect to installer
        if (!await _userService.DatabaseTablesExist())
        {
            return RedirectToAction("Index", "Installer");
        }

        // Check if already logged in
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("ManageForms", "Home");
        }

        // Check IP restriction
        var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        if (_settings.Value.Security.EnableIpRestriction && !await _userService.IsIpWhitelisted(ipAddress))
        {
            TempData["ErrorMessage"] = $"Forbidden - Your IP address ({ipAddress}) is not allowed to access this page.";
            return View();
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "")
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check IP restriction again
        var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        if (_settings.Value.Security.EnableIpRestriction && !await _userService.IsIpWhitelisted(ipAddress))
        {
            ModelState.AddModelError("", $"Forbidden - Your IP address ({ipAddress}) is not allowed to access this page.");
            return View(model);
        }

        // Run cron jobs (simulated)
        await _userService.RunCronJobs();

        // Normalize username
        model.Username = model.Username.Trim().ToLower();

        // LDAP Authentication
        bool ldapAuthenticated = false;
        LdapUserInfo ldapUserInfo = null;
        string ldapError = null;

        if (_settings.Value.Ldap.Enabled)
        {
            (ldapAuthenticated, ldapError, ldapUserInfo) = await _ldapService.Authenticate(model.Username, model.Password);
        }

        // Local authentication (if LDAP not enabled or not exclusive)
        var localAuthResult = Microsoft.AspNetCore.Identity.SignInResult.Failed;
        UserDto user = null;

        if (!_settings.Value.Ldap.Enabled || !_settings.Value.Ldap.Exclusive || !ldapAuthenticated)
        {
            user = await _userManager.FindByNameAsync(model.Username) ??
                   await _userManager.FindByEmailAsync(model.Username);

            if (user != null)
            {
                if (user.Status == UserStatus.Suspended)
                {
                    ModelState.AddModelError("", "Your account has been suspended.");
                    return View(model);
                }

                localAuthResult = await _signInManager.PasswordSignInAsync(
                    user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);
            }
        }

        // Handle successful LDAP auth
        if (ldapAuthenticated)
        {
            user = await _userManager.FindByEmailAsync(ldapUserInfo.Email) ??
                   await _userManager.FindByNameAsync(ldapUserInfo.Username);

            if (user == null)
            {
                // Create new user from LDAP info
                user = new UserDto
                {
                    UserName = ldapUserInfo.Username,
                    Email = ldapUserInfo.Email,
                    FullName = ldapUserInfo.FullName,
                    Status = UserStatus.Active,
                    Privileges = new UserPrivileges
                    {
                        Administer = false,
                        NewForms = true,
                        NewThemes = true
                    }
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to create local user account");
                    return View(model);
                }

                // Create default folder
                await _userService.CreateDefaultFolder(user.Id);
            }

            // Sign in the LDAP user
            await _signInManager.SignInAsync(user, model.RememberMe);
            await _userService.UpdateLastLoginInfo(user.Id, ipAddress);

            return RedirectToLocal(returnUrl);
        }

        // Handle successful local auth
        if (localAuthResult.Succeeded)
        {
            await _userService.UpdateLastLoginInfo(user.Id, ipAddress);

            // Handle 2-step verification if enabled
            if (await _userService.TwoStepVerificationRequired(user.Id))
            {
                if (string.IsNullOrEmpty(user.TwoFactorSecret))
                {
                    return RedirectToAction("SetupTwoFactor", new { rememberMe = model.RememberMe });
                }
                else
                {
                    return RedirectToAction("VerifyTwoFactor", new { rememberMe = model.RememberMe });
                }
            }

            return RedirectToLocal(returnUrl);
        }

        // Handle errors
        if (ldapError != null)
        {
            ModelState.AddModelError("", ldapError);
        }
        else if (localAuthResult.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return RedirectToAction(nameof(Lockout));
        }
        else
        {
            ModelState.AddModelError("", "Invalid login attempt.");
        }

        return View(model);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("ManageForms", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Lockout()
    {
        // دریافت اطلاعات قفل شدن حساب کاربر فعلی
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user.");
        }

        var lockoutRecord = await _userService.GetByUserId(user.Id);
        
        return View(lockoutRecord);
    }

}
