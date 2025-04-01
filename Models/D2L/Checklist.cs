using System.Text.RegularExpressions;

namespace D2LOffice.Models.D2L
{
    public class ChecklistReadData
    {
        public long ChecklistId { get; set; }
        public required string Name { get; set; }
        public RichText Description { get; set; } = new RichText();
    }

    public class ChecklistCategoryReadData
    {
        public long CategoryId { get; set; }
        public required string Name { get; set; }
        public RichText Description { get; set; } = new RichText();
        public int SortOrder { get; set; } = 1; // 1 or greater
    }

    public class ChecklistItemReadData
    {
        public long ChecklistItemId { get; set; }
        public long CategoryId { get; set; }
        public long ChecklistId { get; set; }
        public required string Name { get; set; } = "";
        public RichText Description { get; set; } = new RichText();
        public int SortOrder { get; set; } = 1;
        public DateTime? DueDate { get; set; }

       
    }

    public class ChecklistUpdateData
    {
        public required string Name { get; set; } = "";
        public RichTextInput Description { get; set; } = new RichTextInput() { Content = "", Type = "Text" };
    }

    public class ChecklistCategoryUpdateData
    {
        public required string Name { get; set; } = "";
        public RichTextInput Description { get; set; } = new RichTextInput() { Content = "", Type = "Text" };
        public int SortOrder { get; set; } = 1;
    }

    public class ChecklistItemUpdateData
    {
        public long CategoryId { get; set; }
        public required string Name { get; set; } = "";
        public RichTextInput Description { get; set; } = new RichTextInput() { Content = "", Type = "Text" };
        public int SortOrder { get; set; } = 1;
        public DateTime? DueDate { get; set; }
    }
}