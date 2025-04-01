using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace D2LOffice.Models.D2L
{
    public class Availability
    {
        [JsonPropertyName("StartDate")]
        [JsonProperty("StartDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("EndDate")]
        [JsonProperty("EndDate")]
        public DateTime? EndDate { get; set; }
    }

    public class Assessment
    {
        [JsonPropertyName("ScoreDenominator")]
        [JsonProperty("ScoreDenominator")]
        public double? ScoreDenominator { get; set; }

        [JsonPropertyName("Rubrics")]
        [JsonProperty("Rubrics")]
        public required List<Rubric> Rubrics { get; set; }
    }

    public class Link
    {
        [JsonPropertyName("LinkId")]
        [JsonProperty("LinkId")]
        public int LinkId { get; set; }

        [JsonPropertyName("LinkName")]
        [JsonProperty("LinkName")]
        public required string LinkName { get; set; }

        [JsonPropertyName("Href")]
        [JsonProperty("Href")]
        public required string Href { get; set; }
    }

    public class DropboxCategory
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public string? LastModifiedDate { get; set; }
    }

    public class DropboxCategoryWithFolders : DropboxCategory
    {
        public required List<DropboxFolder> Folders { get; set; }
    }


    public class DropboxFolder
    {
        public DropboxFolder()
        {
            Attachments = new List<File>();
            LinkAttachments = new List<Link>();
        }
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public required string Name { get; set; }
        public required RichText CustomInstructions { get; set; }
        public required List<File> Attachments { get; set; } 
        public int TotalFiles { get; set; }
        public int UnreadFiles { get; set; }
        public int FlaggedFiles { get; set; }
        public int TotalUsers { get; set; }
        public int TotalUsersWithSubmissions { get; set; }
        public int TotalUsersWithFeedback { get; set; }
        public Availability? Availability { get; set; }
        public int? GroupTypeId { get; set; }
        public DateTime? DueDate { get; set; }
        public bool DisplayInCalendar { get; set; }
        public required Assessment Assessment { get; set; }
        public string? NotificationEmail { get; set; }
        public bool IsHidden { get; set; }
        public required List<Link> LinkAttachments { get; set; }
        public string? ActivityId { get; set; }
        public bool IsAnonymous { get; set; }
        public int DropboxType { get; set; }
        public int SubmissionType { get; set; }
        public int CompletionType { get; set; }
        public int? GradeItemId { get; set; }
        public bool? AllowOnlyUsersWithSpecialAccess { get; set; }
    }

    public class DropboxFolderUpdateData
    {
        public int? CategoryId { get; set; }
        public required string Name { get; set; }
        public required RichTextInput CustomInstructions { get; set; }
        public Availability? Availability { get; set; }
        public int? GroupTypeId { get; set; }
        public DateTime? DueDate { get; set; }
        public bool DisplayInCalendar { get; set; }
        public string? NotificationEmail { get; set; }
        public bool? IsHidden { get; set; }
        public Assessment? Assessment { get; set; }
        public bool? IsAnonymous { get; set; }
        public string? DropboxType { get; set; }
        public string? SubmissionType { get; set; }
        public string? CompletionType { get; set; }
        public int? GradeItemId { get; set; }
        public bool? AllowOnlyUsersWithSpecialAccess { get; set; }        
    }
}
