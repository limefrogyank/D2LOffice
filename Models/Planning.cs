using System.Globalization;
using System.Reactive.Linq;

namespace D2LOffice.Models
{
    public class ChecklistPlanningComboItem
    {
        public PlanningItem? PlanningItem { get; set; }
        public D2L.ChecklistItemReadData? ChecklistItem { get; set; }
    }
    public class ChecklistPlanningComboCategory
    {
        public PlanningCategory? PlanningCategory { get; set; }
        public D2L.ChecklistCategoryReadData? ChecklistCategory { get; set; }
    }

    public class PlanningItem : ICloneable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        public bool AddToCalendar { get; set; } = true;
        public string? Category { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Type { get; set; } = "Reminder";
        public string? QuizLink { get; set; }
        public int? Points { get; set; }

        public static List<string> PlanningItemTypes = new List<string> { "Reminder", "Assignment", "Quiz", "Topic" };
        // Reminder is a standard checklist item
        // Assignment is a checklist item with an associated assignment dropbox
        // Quiz is a checklist item with an associated quiz
        // Topic is a ONLY a checklist item if "AddToCalendar" is true.  It's meant for showing in a schedule/syllabus.

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class PlanningCategory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int SortOrder { get; set; } = 0; 
    }

}