using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using D2LOffice.Models;
using D2LOffice.Models.D2L;
using DynamicData;
using Splat;


namespace D2LOffice.Service
{
    public class SyncService
    {
        private readonly long _orgUnitId;
        private readonly string _orgUnitCode;
        private readonly D2LService _d2lService;

        const string StartMarker = "*";

        public static string HTMLMarkerStart = "<div style=\"display: none;\">";
        public static string HTMLMarkerEnd = "</div>";

        public SyncService(long orgUnitId, string orgUnitCode)
        {
            _orgUnitId = orgUnitId;
            _orgUnitCode = orgUnitCode;
            var d2lService = Locator.Current.GetService<D2LService>();
            if (d2lService == null)
            {
                throw new InvalidOperationException("D2LService is null");
            }
            _d2lService = d2lService;
        }

        // Sync data
        public async Task SyncToD2LAsync(IList<PlanningItem> planningItems, IList<PlanningCategory> planningCategories, Action<SyncProgress> showProgress)
        {

            try
            {
                // Ensure categories and items have D2LOfficeId
                foreach (var item in planningItems)
                {
                    item.EnsureSetD2LOfficeId();
                }
                foreach (var cat in planningCategories)
                {
                    cat.EnsureSetD2LOfficeId();
                }                

                var dropboxFolders = await ProcessDropboxAssignmentItemsAsync(planningItems, planningCategories, showProgress);

                // get week numbers from planningItems
                var firstDate = planningItems.Min(x => x.Date);
                CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                Calendar calendar = cultureInfo.Calendar;
                CalendarWeekRule calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
                DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
                int weekNumberOfFirstDate = calendar.GetWeekOfYear(firstDate.DateTime, calendarWeekRule, firstDayOfWeek);
                var weekNumbers = planningItems.Select(x => calendar.GetWeekOfYear(x.Date.DateTime, calendarWeekRule, firstDayOfWeek) - weekNumberOfFirstDate + 1).Distinct().ToList();

                // status details
                var totalItems = planningItems.Count + (planningCategories.Count * weekNumbers.LastOrDefault(1));
                var completedItems = 0;

                // Get checklists
                var allchecklists = await _d2lService.GetAllChecklistsAsync(_orgUnitId);

                // Find weekly checklists   
                var weeklyChecklists = allchecklists.Where(x => x.Description.Html != null && x.Description.Html.Contains("D2LOfficePlanningId")).ToList();

                // Compare to weeks needed
                var tempBlankWeeks = weekNumbers.Select(w => new ChecklistReadData { Name = $"Week {w.ToString("00")}" });
                var existingWeeks = weeklyChecklists.Intersect(tempBlankWeeks, new CheckListComparer());
                var existingWeeksDelete = weeklyChecklists.Except(tempBlankWeeks, new CheckListComparer());
                var weeksToCreate = tempBlankWeeks.Except(weeklyChecklists, new CheckListComparer());

                // Delete weeks no longer needed
                foreach (var week in existingWeeksDelete)
                {
                    await _d2lService.DeleteChecklistAsync(_orgUnitId, week.ChecklistId);
                    weeklyChecklists.Remove(week);
                    showProgress(new SyncProgress { Message = $"Deleted {week.Name} checklist.", Completed = completedItems, Total=totalItems });
                }

                // Create new weeks
                foreach (var week in weeksToCreate)
                {
                    var newWeek = new ChecklistUpdateData
                    {
                        Name = week.Name,
                        Description = new RichTextInput { Content = $"{HTMLMarkerStart}D2LOfficePlanningId - Made by D2LOffice - DO NOT DELETE{HTMLMarkerEnd}", Type = "Html" }
                    };
                    var replace = await _d2lService.AddChecklistAsync(_orgUnitId, newWeek);
                    if (replace != null)
                        weeklyChecklists.Add(replace);
                    showProgress(new SyncProgress { Message = $"Added {week.Name} checklist.", Completed = completedItems, Total = totalItems });
                }

                // Create schedule
                await GenerateTopicsScheduleAsync(planningItems, showProgress);

                // Sync data to weeks
                foreach (var week in weeklyChecklists)
                {
                    try
                    {
                        var weekNumber = int.Parse(week.Name.Substring(5, 2));
                        // Get categories
                        var currentCategories = await _d2lService.GetAllCategoriesForChecklistAsync(_orgUnitId, week.ChecklistId);
                        // Get items
                        var currentItems = await _d2lService.GetAllItemsFromChecklistAsync(_orgUnitId, week.ChecklistId);

                        // Assume all categories and items are from D2LOffice so we can delete any and all

                        // Match up items with planningItems for that week
                        var sunday = FirstDateOfWeekISO8601(firstDate.Year, weekNumberOfFirstDate + weekNumber - 1);
                        var saturday = sunday.AddDays(6);
                        var planningItemsForWeek = planningItems.Where(x => x.Date.DateTime >= sunday && x.Date.DateTime <= saturday).Where(x => x.Type != "Topic" || x.AddToCalendar == true);

                        // check categories in planningItems and create/delete/update if needed

                        var tempCategories = planningItemsForWeek.Select(x => x.Category).Distinct().Select(x => new ChecklistCategoryReadData
                        {
                            Name = x!,
                            Description = new RichText { Html = planningCategories.FirstOrDefault(y => y.Name == x)?.Description }
                        }).ToList();

                        var existingCategoriesToDelete = currentCategories.Except(tempCategories, new CheckListCategoryComparer()).ToList();
                        var categoriesToCreate = tempCategories.Except(currentCategories, new CheckListCategoryComparer()).ToList();
                        var categoriesToUpdate = tempCategories.Intersect(currentCategories, new CheckListCategoryComparer()).ToList();

                        foreach (var category in existingCategoriesToDelete)
                        {
                            await _d2lService.DeleteCategoryOnChecklistAsync(_orgUnitId, week.ChecklistId, category.CategoryId);
                            currentCategories.Remove(category);
                            showProgress(new SyncProgress { Message = $"Deleted {category.Name} category.", Completed = completedItems, Total = totalItems });
                        }
                        foreach (var category in categoriesToCreate)
                        {
                            var newCategory = new ChecklistCategoryUpdateData
                            {
                                Name = category.Name,
                                Description = new RichTextInput { Content = category.Description.Html!, Type = "Html" },
                                SortOrder = planningCategories.IndexOf(planningCategories.FirstOrDefault(x => x.Name == category.Name, new PlanningCategory())) + 1
                            };
                            var newCategoryWithId = await _d2lService.AddCategoryToChecklistAsync(_orgUnitId, week.ChecklistId, newCategory);
                            if (newCategoryWithId != null)
                            {
                                currentCategories.Add(newCategoryWithId);
                            }
                            else
                                throw new InvalidOperationException("Failed to create category");
                            completedItems++;
                            showProgress(new SyncProgress { Message = $"Created {category.Name} category.", Completed = completedItems, Total = totalItems });
                        }
                        foreach (var category in categoriesToUpdate)
                        {
                            var updateCategory = new ChecklistCategoryUpdateData
                            {
                                Name = category.Name,
                                Description = new RichTextInput { Content = category.Description.Html!, Type = "Html" },
                                SortOrder = planningCategories.IndexOf(planningCategories.FirstOrDefault(x => x.Name == category.Name, new PlanningCategory())) + 1
                            };
                            var categoryId = currentCategories.FirstOrDefault(x => x.GetD2LOfficeId() == category.GetD2LOfficeId())?.CategoryId ?? 0;
                            await _d2lService.UpdateCategoryOnChecklistAsync(_orgUnitId, week.ChecklistId, categoryId, updateCategory);
                            completedItems++;
                            showProgress(new SyncProgress { Message = $"Updated {category.Name} category.", Completed = completedItems, Total = totalItems });
                            // shouldn't need to adjust allCategories 
                        }

                        // check items in planningItems and create/delete/update if needed
                        var tempPlanningItems = planningItemsForWeek.Select(x => new ChecklistItemReadData
                        {
                            Name = x.Title,
                            Description = new RichText
                            {
                                Html = (x.Type == "Reminder") ? x.Description : GenerateHTMLDescription(x, dropboxFolders)
                            },
                            DueDate = (x.Type == "Reminder" || (x.Type == "Topic" && x.AddToCalendar)) ? (x.AddToCalendar ? x.Date.UtcDateTime : null) : null, // only set due date for reminder.
                                                                                                                   // assignment and quiz has their own due date
                            CategoryId = currentCategories.FirstOrDefault(y => y.Name == x.Category)?.CategoryId ?? 0,
                            SortOrder = planningItemsForWeek.IndexOf(x) + 1
                        });
                        if (tempPlanningItems.Any(x => x.CategoryId == 0))
                        {
                            throw new InvalidOperationException("Category not found");
                        }

                        var existingItems = currentItems.Intersect(tempPlanningItems, new CheckListItemComparer()).ToList();
                        var existingItemsToDelete = currentItems.Except(tempPlanningItems, new CheckListItemComparer()).ToList();
                        var itemsToCreate = tempPlanningItems.Except(currentItems, new CheckListItemComparer()).ToList();
                        var itemsToUpdate = tempPlanningItems.Intersect(currentItems, new CheckListItemComparer()).ToList();

                        foreach (var item in existingItemsToDelete)
                        {
                            showProgress(new SyncProgress { Message = $"Deleting {item.Name} from week {weekNumber}" });
                            //Debug.WriteLine($"Deleting {item.Name} from week {weekNumber}");
                            var del = await _d2lService.DeleteItemFromChecklistAsync(_orgUnitId, week.ChecklistId, item.ChecklistItemId);
                            currentItems.Remove(item);
                            showProgress(new SyncProgress { Message = $"Week {weekNumber}: Deleted {item.Name} checklist item.", Completed = completedItems, Total = totalItems });
                        }
                        foreach (var item in itemsToCreate)
                        {
                            var newItem = new ChecklistItemUpdateData
                            {
                                CategoryId = item.CategoryId,
                                Name = item.Name,
                                Description = new RichTextInput { Content = item.Description.Html!, Type = "Html" },
                                DueDate = item.DueDate,
                                SortOrder = item.SortOrder
                            };
                            var newItemWithId = await _d2lService.AddItemToChecklistAsync(_orgUnitId, week.ChecklistId, newItem);
                            if (newItemWithId != null)
                                currentItems.Add(newItemWithId);
                            else
                                throw new InvalidOperationException("Failed to create item");
                            completedItems++;
                            showProgress(new SyncProgress { Message = $"Week {weekNumber}: Created {item.Name} checklist item.", Completed = completedItems, Total = totalItems });
                        }
                        foreach (var item in itemsToUpdate)
                        {
                            var updateItem = new ChecklistItemUpdateData
                            {
                                CategoryId = currentCategories.FirstOrDefault(y => y.Name == (planningItems.FirstOrDefault(x => x.Title == item.Name)?.Category ?? ""))?.CategoryId ?? 0,
                                Name = item.Name,
                                Description = new RichTextInput { Content = item.Description.Html!, Type = "Html" },
                                DueDate = item.DueDate,
                                SortOrder = item.SortOrder
                            };
                            var checkListItemId = currentItems.FirstOrDefault(x => x.GetD2LOfficeId() == item.GetD2LOfficeId())?.ChecklistItemId ?? 0;
                            await _d2lService.UpdateItemFromChecklistAsync(_orgUnitId, week.ChecklistId, checkListItemId, updateItem);
                            completedItems++;
                            showProgress(new SyncProgress { Message = $"Week {weekNumber}: Updated {item.Name} checklist item.", Completed = completedItems, Total = totalItems });
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private string GenerateHTMLDescription(PlanningItem planningItem, List<DropboxFolder> dropboxFolders)
        {
            switch (planningItem.Type)
            {
                case "Assignment":
                    var assignment = dropboxFolders.FirstOrDefault(x => x.GetD2LOfficeId() == planningItem.Id);
                    if (default)
                    {
                        throw new Exception("Can't find the assignment that goes with this planning item.");
                    }
                    if (assignment.ActivityId == null)
                    {
                        throw new Exception("ActivityId was null");
                    }
                    var uri = new Uri(assignment.ActivityId);
                    var rcode = uri.Segments.Last();
                    if (rcode == null)
                    {
                        throw new Exception("Path of ActivityId was empty");
                    }
                    return $"<p>Due {planningItem.Date.LocalDateTime.ToShortDateString()}</p><p><a href=\"/d2l/common/dialogs/quickLink/quickLink.d2l?ou={{orgUnitId}}&amp;type=dropbox&amp;rcode={rcode}\" target=\"_blank\" rel=\"noopener\">Link to Assignment</a></p>{planningItem.CreateD2LOfficeIdSnippet()}";
                case "Quiz":
                    return $"<p>Due {planningItem.Date.LocalDateTime.ToShortDateString()}</p><p><a href=\"{planningItem.QuizLink}\">{planningItem.Title} Link</a></p>{planningItem.CreateD2LOfficeIdSnippet()}";
                default:
                    return planningItem.Description;
            }
        }

        private async Task GenerateTopicsScheduleAsync(IList<PlanningItem> planningItems, Action<SyncProgress> showProgress)
        {
            // get week numbers from planningItems
            var firstDate = planningItems.Min(x => x.Date);
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            Calendar calendar = cultureInfo.Calendar;
            CalendarWeekRule calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            int weekNumberOfFirstDate = calendar.GetWeekOfYear(firstDate.DateTime, calendarWeekRule, firstDayOfWeek);
            var weekNumbers = planningItems.Select(x => calendar.GetWeekOfYear(x.Date.DateTime, calendarWeekRule, firstDayOfWeek) - weekNumberOfFirstDate + 1).Distinct().Order().ToList();

            if (weekNumbers.Count == 0)
                return;

            var first = weekNumbers.Min();
            var last = weekNumbers.Last();

            var daysOfWeek = planningItems.Select(x => calendar.GetDayOfWeek(x.Date.LocalDateTime)).Distinct().Order().ToList();

            StringBuilder builder = new StringBuilder();
            builder.Append("<!DOCTYPE html>\r\n<html><head><link rel=\"stylesheet\" href=\"https://s.brightspace.com/lib/fonts/0.6.1/fonts.css\">");
            builder.Append("<style>table, th, td {\r\n  border: 1px solid black;\r\n  border-collapse: collapse;\r\n}td.examColor{background-color:yellow;}</style>");
            builder.Append("</head><body style=\"color: rgb(32, 33, 34); font-family: Lato, sans-serif;\">");
            builder.Append("<table><caption>Lecture and Lab Schedule</caption><tbody><tr><th scope=\"col\">Week #</th><th scope=\"col\">Date</th>");
            foreach (var day in daysOfWeek)
            {
                builder.Append($"<th scope=\"col\">{day.ToString()}</th>");
            }
            builder.Append($"<th scope=\"col\">Lab</th>");
            builder.Append("</tr>");

            for (var i = first; i <= last; i++)
            {
                var dateStart = FirstDateOfWeekISO8601(firstDate.Year, weekNumberOfFirstDate + i - 1);
                var dateEnd = FirstDateOfWeekISO8601(firstDate.Year, weekNumberOfFirstDate + i - 1).AddDays(6);

                builder.Append("<tr>");
                builder.Append($"<td>{i}</td>");
                builder.Append($"<td>{dateStart.ToShortDateString()} - {dateEnd.ToShortDateString()}</td>");
                foreach (var day in daysOfWeek)
                {
                    var date = FirstDateOfWeekISO8601(firstDate.Year, weekNumberOfFirstDate + i - 1).AddDays((int)day);
                    var items = planningItems.Where(x => x.Type == "Topic").Where(x => x.Date.LocalDateTime.Date == date.Date && x.Category == "Lecture").ToList();
                    if (items.Any(x=>x.Title.Contains("Exam")))
                    {
                        builder.Append("<td class=\"examColor\">");
                    }
                    else
                    {
                        builder.Append("<td>");
                    }
                    items.ForEach(x =>
                    {
                        if (x.Title.Contains("Quiz"))
                            builder.Append("<b>");
                        builder.Append($"{x.Title}<br/>");
                        if (x.Title.Contains("Quiz"))
                            builder.Append("</b>");
                    });
                    builder.Append("</td>");

                }
                builder.Append("<td>");
                planningItems.Where(x => x.Type == "Topic").Where(x => x.Date.LocalDateTime.Date >= dateStart && x.Date.LocalDateTime.Date <= dateEnd && x.Category == "Lab")
                    .ToList().ForEach(x => builder.Append($"{x.Title}<br/>"));
                builder.Append("</td>");
                builder.Append("</tr>");
            }
            builder.Append("</tbody></table>");
            builder.Append("</body></html>");
            var html = builder.ToString();

            var rootModules = await _d2lService.GetRootModulesAsync(_orgUnitId.ToString());
            var courseInformationModule = rootModules.Cast<Module>().FirstOrDefault(x => x.Title == "Course Information", null);
            if (courseInformationModule == null)
            {
                courseInformationModule = await _d2lService.CreateModuleInRootAsync(_orgUnitId.ToString(), new ModuleData { Title = "Course Information" });
                if (courseInformationModule == null)
                {
                    throw new InvalidOperationException("Failed to create module");
                }
            }
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(html);
            writer.Flush();
            stream.Position = 0;

           

            var topic = courseInformationModule.Structure.Where(x => x.Type == ContentObjectType.Topic).Cast<Topic>().FirstOrDefault(x => x.Title == "Schedule");
            if (topic == null)
            {
                var orgId = $"{_orgUnitId}-{_orgUnitCode}";
                //var orgId = $"{_orgUnitId}-{string.Join('_', _orgUnitCode.Split('_').SkipLast(1))}";
                var filePath = $"/content/enforced/{orgId}/Schedule.html";
                topic = await _d2lService.CreateTopicInModuleWithFileAsync(
                        _orgUnitId.ToString(),
                        courseInformationModule.Id.ToString(),
                        new TopicData { Title = "Schedule", ShortTitle = "Schedule", Url = filePath, TopicType = TopicType.File },
                        "Schedule.html",
                        "text/html",
                        stream);
            }
            else
            {
                var uploadedFile = await _d2lService.UploadFileStreamAsync(
                    _orgUnitId.ToString(),
                    "",
                    "Schedule.html",
                    "text/html",
                    stream,
                    CancellationToken.None,
                    overwrite: true);
            }
        }

        private async Task<List<DropboxFolder>> ProcessDropboxAssignmentItemsAsync(IList<PlanningItem> planningItems, IList<PlanningCategory> planningCategories, Action<SyncProgress> showProgress)
        {
            var tempPlanningCategories = planningCategories.Select(x => new DropboxCategory
            {
                Name = x.Name
            });

            var currentCategories = await _d2lService.GetAllDropboxCategoriesAsync(_orgUnitId.ToString());
            if (currentCategories == null)
            {
                throw new InvalidOperationException("Categories retrieval was null...");
            }

            var existingCategories = currentCategories.Intersect(tempPlanningCategories, new DropboxCategoryComparer()).ToList();
            // Don't delete categories... can't ensure they were only created by D2LOffice
            // var existingItemsToDelete = currentCategories.Except(tempPlanningCategories, new DropboxCategoryComparer()).ToList();
            var categoriesToCreate = tempPlanningCategories.Except(currentCategories, new DropboxCategoryComparer()).ToList();

            foreach (var item in categoriesToCreate)
            {
                var newItemWithId = await _d2lService.CreateDropboxCategoryAsync(_orgUnitId.ToString(), item);
                if (newItemWithId != null)
                    currentCategories.Add(newItemWithId);
                else
                    throw new InvalidOperationException("Failed to create item");
            }

            var currentItems = await _d2lService.GetDropboxFoldersAsync(_orgUnitId.ToString());

            var tempPlanningItems = planningItems.Where(x => x.Type == "Assignment").Select(x => new DropboxFolder
            {
                Name = x.Title,
                CustomInstructions = new RichText { Html = x.Description },
                DueDate = x.Date.UtcDateTime,
                CategoryId = currentCategories.FirstOrDefault(y => y.Name == x.Category)?.Id ?? 0,
                IsHidden = false,
                LinkAttachments = new List<Link>(),
                Attachments = new List<Models.D2L.File>(),
                Assessment = new Assessment { Rubrics = new List<Rubric>(), ScoreDenominator = x.Points }
            });

            if (tempPlanningItems.Any(x => x.CategoryId == 0))
            {
                throw new InvalidOperationException("Category not found");
            }

            var existingItems = currentItems.Intersect(tempPlanningItems, new DropboxFolderComparer()).ToList();
            var existingItemsToDelete = currentItems.Except(tempPlanningItems, new DropboxFolderComparer()).ToList();
            var itemsToCreate = tempPlanningItems.Except(currentItems, new DropboxFolderComparer()).ToList();
            var itemsToUpdate = tempPlanningItems.Intersect(currentItems, new DropboxFolderComparer()).ToList();

            foreach (var item in existingItemsToDelete)
            {
                var del = await _d2lService.DeleteDropboxFolderAsync(_orgUnitId.ToString(), item.Id.ToString());
                currentItems.Remove(item);
                showProgress(new SyncProgress { Message = $"Assignment: Deleted {item.Name} assignment.", Completed = 0, Total = 0 });
            }
            foreach (var item in itemsToCreate)
            {
                var newItem = new DropboxFolderUpdateData
                {
                    CategoryId = item.CategoryId,
                    Name = item.Name,
                    CustomInstructions = new RichTextInput { Content = item.CustomInstructions.Html!, Type = "Html" },
                    DueDate = item.DueDate,
                    IsHidden = item.IsHidden,
                    Assessment = item.Assessment
                };
                var newItemWithId = await _d2lService.CreateDropboxFolderAsync(_orgUnitId.ToString(), newItem);
                if (newItemWithId != null)
                    currentItems.Add(newItemWithId);
                else
                    throw new InvalidOperationException("Failed to create item");
                showProgress(new SyncProgress { Message = $"Assignment: Created {item.Name} assignment.", Completed = 0, Total = 0 });
            }
            foreach (var item in itemsToUpdate)
            {
                var updateItem = new DropboxFolderUpdateData
                {
                    CategoryId = item.CategoryId,
                    Name = item.Name,
                    CustomInstructions = new RichTextInput { Content = item.CustomInstructions.Html!, Type = "Html" },
                    DueDate = item.DueDate,
                    IsHidden = item.IsHidden,
                    Assessment = item.Assessment
                };
                var dropboxId = currentItems.FirstOrDefault(x => x.GetD2LOfficeId() == item.GetD2LOfficeId())?.Id ?? 0;
                var dropboxFolderWithId = await _d2lService.UpdateDropboxFolderAsync(_orgUnitId.ToString(), dropboxId.ToString(), updateItem);
                currentItems.Replace(currentItems.FirstOrDefault(x => x.GetD2LOfficeId() == item.GetD2LOfficeId()), dropboxFolderWithId);
                showProgress(new SyncProgress { Message = $"Assignment: Updated {item.Name} assignment.", Completed = 0, Total = 0 });
            }
            return currentItems;
        }

        private Task CreateChecklistContentItemsAsync(string moduleId, ChecklistReadData checklist)
        {
            
            return Task.CompletedTask;
        }

        private DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 4 days from Thursday to get Sonday, which is the first weekday in ISO8601
            return result.AddDays(-4);
        }

    }

    public static class Extensions
    {
        public static string CreateD2LOfficeIdSnippet(this PlanningItem item)
        {
            return SyncService.HTMLMarkerStart + $"D2LOfficeId:{item.Id}" + SyncService.HTMLMarkerEnd;
        }
        public static string EnsureSetD2LOfficeId(this PlanningItem item)
        {
            if (item.Description == null)
            {
                return SyncService.HTMLMarkerStart + $"D2LOfficeId:{item.Id}" + SyncService.HTMLMarkerEnd;
            }
            var match = Regex.Match(item.Description, "D2LOfficeId:([\\w-]+)");
            if (match != null && match.Success)
            {
                var currentId = match.Groups[1].Value;
                if (currentId != item.Id)
                {
                    item.Description = Regex.Replace(item.Description, "D2LOfficeId:([\\w-]+)", $"D2LOfficeId:{item.Id}");
                    //item.Description.Html = item.Description.Html.Replace($"D2LOfficeId:{currentId}", $"D2LOfficeId:{guid}");
                }
            }
            else
            {
                item.Description += SyncService.HTMLMarkerStart + $"D2LOfficeId:{item.Id}" + SyncService.HTMLMarkerEnd;
            }
            return item.Description;
        }

        public static string? GetD2LOfficeId(this ChecklistItemReadData item)
        {
            if (item.Description.Html == null)
            {
                return null;
            }
            var match = Regex.Match(item.Description.Html, "D2LOfficeId:([\\w-]+)");
            if (match != null && match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }

        }

        public static string EnsureSetD2LOfficeId(this PlanningCategory item)
        {
            if (item.Description == null)
            {
                return SyncService.HTMLMarkerStart + $"D2LOfficeId:{item.Id}" + SyncService.HTMLMarkerEnd;
            }
            var match = Regex.Match(item.Description, "D2LOfficeId:([\\w-]+)");
            if (match != null && match.Success)
            {
                var currentId = match.Groups[1].Value;
                if (currentId != item.Id)
                {
                    item.Description = Regex.Replace(item.Description, "D2LOfficeId:([\\w-]+)", $"D2LOfficeId:{item.Id}");
                    //item.Description.Html = item.Description.Html.Replace($"D2LOfficeId:{currentId}", $"D2LOfficeId:{guid}");
                }
            }
            else
            {
                item.Description += SyncService.HTMLMarkerStart + $"D2LOfficeId:{item.Id}" + SyncService.HTMLMarkerEnd;

            }
            return item.Description;

        }

        public static string? GetD2LOfficeId(this ChecklistCategoryReadData item)
        {
            if (item.Description.Html == null)
            {
                return null;
            }
            var match = Regex.Match(item.Description.Html, "D2LOfficeId:([\\w-]+)");
            if (match != null && match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }

        }

        public static string? GetD2LOfficeId(this DropboxFolder item)
        {
            if (item.CustomInstructions.Html == null)
            {
                return null;
            }
            var match = Regex.Match(item.CustomInstructions.Html, "D2LOfficeId:([\\w-]+)");
            if (match != null && match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }

        }
    }

    public class CheckListComparer : IEqualityComparer<ChecklistReadData>
    {
        public bool Equals(ChecklistReadData? x, ChecklistReadData? y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(ChecklistReadData obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    public class CheckListCategoryComparer : IEqualityComparer<ChecklistCategoryReadData>
    {
        public bool Equals(ChecklistCategoryReadData? x, ChecklistCategoryReadData? y)
        {
            if (x == null || y == null)
                return false;
            if (x.Description == null || x.Description.Html == null)
                return false;
            if (y.Description == null || y.Description.Html == null)
                return false;
            var xid = x.GetD2LOfficeId();
            var yid = y.GetD2LOfficeId();
            if (xid == null || yid == null)
                return false;
            return xid! == yid!;
        }

        public int GetHashCode(ChecklistCategoryReadData obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    public class CheckListItemComparer : IEqualityComparer<ChecklistItemReadData>
    {
        public bool Equals(ChecklistItemReadData? x, ChecklistItemReadData? y)
        {
            if (x == null || y == null)
                return false;
            if (x.Description == null || x.Description.Html == null)
                return false;
            if (y.Description == null || y.Description.Html == null)
                return false;
            var xid = x.GetD2LOfficeId();
            var yid = y.GetD2LOfficeId();
            if (xid == null || yid == null)
                return false;
            return xid! == yid!;
        }

        public int GetHashCode(ChecklistItemReadData obj)
        {
            return obj.Name.GetHashCode();
        }
    }


    public class DropboxCategoryComparer : IEqualityComparer<DropboxCategory>
    {
        public bool Equals(DropboxCategory? x, DropboxCategory? y)
        {
            if (x == null || y == null)
                return false;

            return x.Name == y.Name;
        }

        public int GetHashCode(DropboxCategory obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    public class DropboxFolderComparer : IEqualityComparer<DropboxFolder>
    {
        public bool Equals(DropboxFolder? x, DropboxFolder? y)
        {
            if (x == null || y == null)
                return false;

            return x.Name == y.Name;
        }

        public int GetHashCode(DropboxFolder obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}