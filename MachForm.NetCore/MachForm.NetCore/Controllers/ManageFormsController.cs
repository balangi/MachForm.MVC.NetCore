using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MachForm.NetCore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using MachForm.NetCore.Services;
using MachForm.NetCore.Services.FoldersConditions;
using MachForm.NetCore.Models.Forms;
using MachForm.NetCore.Models.FormSorts;

namespace MachForm.NetCore.Controllers;


    [Authorize]
    public class FormManagerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public FormManagerController(ApplicationDbContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? id, int? folder, string sortby, bool? hl)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userPrivileges = await GetUserPrivileges(userId);

            var selectedFormId = id ?? 0;
            var highlightSelectedFormId = hl ?? false;

            // Handle folder selection
            if (folder.HasValue)
            {
                await UpdateSelectedFolder(userId, folder.Value);
            }

            // Get selected folder
            var selectedFolder = await _dbContext.Folders
                .FirstOrDefaultAsync(f => f.UserId == userId && Convert.ToBoolean(f.FolderSelected));

            var selectedFolderId = selectedFolder?.FolderId ?? 1;
            var selectedFolderName = selectedFolder?.FolderName ?? "All Forms";
            var selectedFolderRuleAllAny = selectedFolder?.RuleAllAny ?? "all";

            // Get form sort preference
            var formSortByComplete = await GetFormSortPreference(userId);

            // Get folder conditions
            var folderConditions = await _dbContext.FoldersConditions
                .Where(fc => fc.UserId == userId && fc.FolderId == selectedFolderId)
                .ToListAsync();

            // Build query for forms
            var formQuery = BuildFormQuery(userId, folderConditions, selectedFolderRuleAllAny, formSortByComplete);

            // Get forms with permissions
            var formList = await formQuery.ToListAsync();
            var formListArray = await ProcessFormsWithPermissions(formList, userId, userPrivileges);

            // Pagination
            var rowsPerPage = _configuration.GetValue<int>("FormManager:MaxRows", 20);
            var totalRows = formListArray.Count;
            var totalPages = (int)Math.Ceiling(totalRows / (double)rowsPerPage);

            // Get available tags
            var allTagNames = await _dbContext.Forms
                .Where(f => !string.IsNullOrEmpty(f.FormTags))
                .Select(f => f.FormTags)
                .ToListAsync();

            var uniqueTags = allTagNames
                .SelectMany(t => t.Split(','))
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            // Get themes
            var themeList = await GetThemeList(userId, userPrivileges);
            var builtInThemes = await _dbContext.FormThemes
                .Where(t => Convert.ToBoolean( t.ThemeBuiltIn) && Convert.ToBoolean(t.Status))
                .OrderBy(t => t.ThemeName)
                .ToDictionaryAsync(t => t.ThemeId, t => t.ThemeName);

            // Prepare view model
            var viewModel = new FormManagerViewModel
            {
                FormList = formListArray,
                SelectedFormId = selectedFormId,
                HighlightSelectedFormId = highlightSelectedFormId,
                SelectedFolderId = selectedFolderId,
                SelectedFolderName = selectedFolderName,
                TotalActiveForms = totalRows,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                AllTagNames = uniqueTags,
                ThemeList = themeList,
                BuiltInThemes = builtInThemes,
                FormSortByComplete = formSortByComplete,
                SortByTitle = GetSortByTitle(formSortByComplete),
                IsFolderPinned = Convert.ToBoolean( await _dbContext.Users
                    .Where(u => u.Id == userId.ToString())
                    .Select(u => u.FoldersPinned)
                    .FirstOrDefaultAsync()),
                Folders = await _dbContext.Folders
                    .Where(f => f.UserId == userId)
                    .OrderBy(f => f.FolderPosition)
                    .ToListAsync(),
                UserPrivileges = userPrivileges
            };

            return View(viewModel);
        }

        private async Task<Dictionary<string, bool>> GetUserPrivileges(int userId)
        {
            return await _dbContext.Permissions
                .Where(up => up.UserId == userId)
                .ToDictionaryAsync(
                    up => $"priv_{up.PermissionType.ToLower()}",
                    up => up.HasPermission
                );
        }

        private async Task UpdateSelectedFolder(int userId, int folderId)
        {
            // Clear any active folder
            await _dbContext.Folders
                .Where(f => f.UserId == userId)
                .ForEachAsync(f => f.FolderSelected = Convert.ToInt16(false));

            // Set new selected folder
            var folder = await _dbContext.Folders
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FolderId == folderId);

            if (folder != null)
            {
                folder.FolderSelected = Convert.ToInt16( true);
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task<string> GetFormSortPreference(int userId)
        {
            var defaultSort = "date_created-desc";

            if (HttpContext.Request.Query.ContainsKey("sortby"))
            {
                var newSort = HttpContext.Request.Query["sortby"].ToString().ToLower();

                // Save new sort preference
                var existingSort = await _dbContext.FormSorts
                    .FirstOrDefaultAsync(fs => fs.UserId == userId);

                if (existingSort != null)
                {
                    existingSort.SortBy = newSort;
                }
                else
                {
                    _dbContext.FormSorts.Add(new FormSortDto
                    {
                        UserId = userId,
                        SortBy = newSort
                    });
                }

                await _dbContext.SaveChangesAsync();
                return newSort;
            }

            // Load saved sort preference
            var savedSort = await _dbContext.FormSorts
                .Where(fs => fs.UserId == userId)
                .Select(fs => fs.SortBy)
                .FirstOrDefaultAsync();

            return savedSort ?? defaultSort;
        }

        private IQueryable<FormDto> BuildFormQuery(int userId, List<FoldersConditionDto> folderConditions, string ruleAllAny, string formSortByComplete)
        {
            var query = _dbContext.Forms
                .Include(f => f.FormStats)
                .Where(f => f.FormActive || !f.FormActive); // Include both active and inactive forms

            // Apply folder conditions
            if (folderConditions.Any())
            {
                var conditions = new List<string>();
                var parameters = new Dictionary<string, object>();

                foreach (var condition in folderConditions)
                {
                    var whereClause = BuildWhereClause(condition, parameters);
                    if (!string.IsNullOrEmpty(whereClause))
                    {
                        conditions.Add(whereClause);
                    }
                }

                var combinedCondition = ruleAllAny == "all" ?
                    string.Join(" AND ", conditions) :
                    string.Join(" OR ", conditions);

                query = query.Where(f => EF.Functions.Like(combinedCondition, "1=1")); // This needs proper dynamic LINQ
            }

            // Apply sorting
            var sortParts = formSortByComplete.Split('-');
            var sortBy = sortParts[0];
            var sortOrder = sortParts.Length > 1 ? sortParts[1] : "asc";

            switch (sortBy)
            {
                case "form_title":
                    query = sortOrder == "desc" ?
                        query.OrderByDescending(f => f.FormName) :
                        query.OrderBy(f => f.FormName);
                    break;
                case "form_tags":
                    query = sortOrder == "desc" ?
                        query.OrderByDescending(f => f.FormTags) :
                        query.OrderBy(f => f.FormTags);
                    break;
                case "today_entries":
                    query = sortOrder == "desc" ?
                        query.OrderByDescending(f => f.FormStats.TodayEntries) :
                        query.OrderBy(f => f.FormStats.TodayEntries);
                    break;
                case "total_entries":
                    query = sortOrder == "desc" ?
                        query.OrderByDescending(f => f.FormStats.TotalEntries) :
                        query.OrderBy(f => f.FormStats.TotalEntries);
                    break;
                default: // date_created
                    query = sortOrder == "desc" ?
                        query.OrderByDescending(f => f.FormId) :
                        query.OrderBy(f => f.FormId);
                    break;
            }

            return query;
        }

        private string BuildWhereClause(FoldersConditionDto condition, Dictionary<string, object> parameters)
        {
            // This is a simplified version - in a real app you'd use Dynamic LINQ or build expressions
            switch (condition.ElementName)
            {
                case "title":
                    return BuildStringCondition("FormName", condition.RuleCondition, condition.RuleKeyword);
                case "tag":
                    return BuildStringCondition("FormTags", condition.RuleCondition, condition.RuleKeyword);
                // Add other cases...
                default:
                    return "";
            }
        }

        private string BuildStringCondition(string field, string ruleCondition, string ruleKeyword)
        {
            switch (ruleCondition)
            {
                case "is":
                    return $"{field} = '{ruleKeyword}'";
                case "is_not":
                    return $"{field} <> '{ruleKeyword}'";
                case "begins_with":
                    return $"{field} LIKE '{ruleKeyword}%'";
                case "ends_with":
                    return $"{field} LIKE '%{ruleKeyword}'";
                case "contains":
                    return $"{field} LIKE '%{ruleKeyword}%'";
                case "not_contain":
                    return $"{field} NOT LIKE '%{ruleKeyword}%'";
                default:
                    return "";
            }
        }

        private async Task<List<FormViewModel>> ProcessFormsWithPermissions(List<FormDto> forms, int userId, Dictionary<string, bool> userPrivileges)
        {
            var userPermissions = await _dbContext.Permissions
                .Where(up => up.UserId == userId)
                .ToDictionaryAsync(up => up.FormId, up => up);

            var result = new List<FormViewModel>();

            foreach (var form in forms)
            {
                // Check permissions
                if (!userPrivileges["priv_administer"] &&
                    (!userPermissions.ContainsKey(form.FormId) ||
                     ! Convert.ToBoolean( userPermissions[form.FormId].EditForm)))
                {
                    continue;
                }

                var formVm = new FormViewModel
                {
                    FormId = form.FormId,
                    FormName = string.IsNullOrEmpty(form.FormName) ?
                        $"-Untitled Form- (#{form.FormId})" :
                        form.FormName.Truncate(90),
                    FormActive = Convert.ToBoolean(form.FormActive),
                    FormDisabledMessage = form.FormDisabledMessage,
                    FormThemeId = form.FormThemeId,
                    FormApprovalEnable = Convert.ToBoolean(form.FormApprovalEnable),
                    TodayEntries = form.FormStats?.TodayEntries ?? 0,
                    LatestEntry = form.FormStats?.LastEntryDate?.ToRelativeDateString() ?? "Never",
                    TotalEntries = form.FormStats?.TotalEntries ?? 0,
                    FormCreatedBy = form.FormCreatedBy.ToString(),
                    FormTags = string.IsNullOrEmpty(form.FormTags) ?
                        new List<string>() :
                        form.FormTags.Split(',').Select(t => t.Trim()).ToList()
                };

                result.Add(formVm);
            }

            return result;
        }

        private async Task<Dictionary<int, string>> GetThemeList(int userId, Dictionary<string, bool> userPrivileges)
        {
            if (userPrivileges["priv_administer"])
            {
                return await _dbContext.FormThemes
                    .Where(t => !Convert.ToBoolean(t.ThemeBuiltIn) && Convert.ToBoolean(t.Status))
                    .OrderBy(t => t.ThemeName)
                    .ToDictionaryAsync(t => t.ThemeId, t => t.ThemeName);
            }

            return await _dbContext.FormThemes
                .Where(t => !Convert.ToBoolean(t.ThemeBuiltIn) && Convert.ToBoolean(t.Status) &&
                           (t.UserId == userId || !Convert.ToBoolean(t.ThemeIsPrivate)))
                .OrderBy(t => t.ThemeName)
                .ToDictionaryAsync(t => t.ThemeId, t => t.ThemeName);
        }

        private string GetSortByTitle(string formSortByComplete)
        {
            var sortParts = formSortByComplete.Split('-');
            var sortBy = sortParts[0];

            switch (sortBy)
            {
                case "form_title": return "Form Title";
                case "form_tags": return "Form Tags";
                case "today_entries": return "Today's Entries";
                case "total_entries": return "Total Entries";
                default: return "Date Created";
            }
        }
    }

    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string ToRelativeDateString(this DateTime date)
        {
            var timeSpan = DateTime.Now - date;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return $"{timeSpan.Seconds} seconds ago";

            if (timeSpan <= TimeSpan.FromMinutes(60))
                return $"{timeSpan.Minutes} minutes ago";

            if (timeSpan <= TimeSpan.FromHours(24))
                return $"{timeSpan.Hours} hours ago";

            if (timeSpan <= TimeSpan.FromDays(30))
                return $"{timeSpan.Days} days ago";

            if (timeSpan <= TimeSpan.FromDays(365))
                return $"{timeSpan.Days / 30} months ago";

            return $"{timeSpan.Days / 365} years ago";
        }
    }
