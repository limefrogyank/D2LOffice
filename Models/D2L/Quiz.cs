using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace D2LOffice.Models.D2L
{
    public class RichText
    {
        // [JsonPropertyName("Text")]
        // [JsonProperty("Text")]
        public string? Text { get; set; }

        // [JsonPropertyName("Html")]
        // [JsonProperty("Html")]
        public string? Html { get; set; }
    }

    public class Instructions
    {
        [JsonPropertyName("Text")]
        [JsonProperty("Text")]
        public required RichText Text { get; set; }

        [JsonPropertyName("IsDisplayed")]
        [JsonProperty("IsDisplayed")]
        public bool IsDisplayed { get; set; }
    }

    public class Description
    {
        [JsonPropertyName("Text")]
        [JsonProperty("Text")]
        public required RichText Text { get; set; }

        [JsonPropertyName("IsDisplayed")]
        [JsonProperty("IsDisplayed")]
        public bool IsDisplayed { get; set; }
    }

    public class Header
    {
        [JsonPropertyName("Text")]
        [JsonProperty("Text")]
        public required RichText Text { get; set; }

        [JsonPropertyName("IsDisplayed")]
        [JsonProperty("IsDisplayed")]
        public bool IsDisplayed { get; set; }
    }

    public class Footer
    {
        [JsonPropertyName("Text")]
        [JsonProperty("Text")]
        public required RichText Text { get; set; }

        [JsonPropertyName("IsDisplayed")]
        [JsonProperty("IsDisplayed")]
        public bool IsDisplayed { get; set; }
    }

    public class SubmissionTimeLimit
    {
        [JsonPropertyName("IsEnforced")]
        [JsonProperty("IsEnforced")]
        public bool IsEnforced { get; set; }

        [JsonPropertyName("ShowClock")]
        [JsonProperty("ShowClock")]
        public bool ShowClock { get; set; }

        [JsonPropertyName("TimeLimitValue")]
        [JsonProperty("TimeLimitValue")]
        public int TimeLimitValue { get; set; }
    }

    public class LateSubmissionInfo
    {
        [JsonPropertyName("LateSubmissionOption")]
        [JsonProperty("LateSubmissionOption")]
        public int LateSubmissionOption { get; set; }

        [JsonPropertyName("LateLimitMinutes")]
        [JsonProperty("LateLimitMinutes")]
        public int? LateLimitMinutes { get; set; }
    }

    public class AttemptsAllowed
    {
        [JsonPropertyName("IsUnlimited")]
        [JsonProperty("IsUnlimited")]
        public bool IsUnlimited { get; set; }

        [JsonPropertyName("NumberOfAttemptsAllowed")]
        [JsonProperty("NumberOfAttemptsAllowed")]
        public int? NumberOfAttemptsAllowed { get; set; }
    }

    public class Quiz
    {
        [JsonPropertyName("QuizId")]
        [JsonProperty("QuizId")]
        public int QuizId { get; set; }

        [JsonPropertyName("Name")]
        [JsonProperty("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("AutoExportToGrades")]
        [JsonProperty("AutoExportToGrades")]
        public bool AutoExportToGrades { get; set; }

        [JsonPropertyName("IsActive")]
        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("GradeItemId")]
        [JsonProperty("GradeItemId")]
        public int? GradeItemId { get; set; }

        [JsonPropertyName("IsAutoSetGraded")]
        [JsonProperty("IsAutoSetGraded")]
        public bool IsAutoSetGraded { get; set; }

        [JsonPropertyName("Instructions")]
        [JsonProperty("Instructions")]
        public Instructions? Instructions { get; set; }

        [JsonPropertyName("Description")]
        [JsonProperty("Description")]
        public Description? Description { get; set; }

        [JsonPropertyName("Header")]
        [JsonProperty("Header")]
        public Header? Header { get; set; }

        [JsonPropertyName("Footer")]
        [JsonProperty("Footer")]
        public Footer? Footer { get; set; }

        [JsonPropertyName("StartDate")]
        [JsonProperty("StartDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("EndDate")]
        [JsonProperty("EndDate")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("DueDate")]
        [JsonProperty("DueDate")]
        public DateTime? DueDate { get; set; }

        [JsonPropertyName("DisplayInCalendar")]
        [JsonProperty("DisplayInCalendar")]
        public bool DisplayInCalendar { get; set; }

        [JsonPropertyName("SortOrder")]
        [JsonProperty("SortOrder")]
        public int SortOrder { get; set; }

        [JsonPropertyName("SubmissionTimeLimit")]
        [JsonProperty("SubmissionTimeLimit")]
        public SubmissionTimeLimit? SubmissionTimeLimit { get; set; }

        [JsonPropertyName("SubmissionGracePeriod")]
        [JsonProperty("SubmissionGracePeriod")]
        public int SubmissionGracePeriod { get; set; }

        [JsonPropertyName("LateSubmissionInfo")]
        [JsonProperty("LateSubmissionInfo")]
        public LateSubmissionInfo? LateSubmissionInfo { get; set; }

        [JsonPropertyName("AttemptsAllowed")]
        [JsonProperty("AttemptsAllowed")]
        [JsonInclude()]
        public AttemptsAllowed? AttemptsAllowed { get; set; }

        [JsonPropertyName("Password")]
        [JsonProperty("Password")]
        public object? Password { get; set; }

        [JsonPropertyName("AllowHints")]
        [JsonProperty("AllowHints")]
        public bool AllowHints { get; set; }

        [JsonPropertyName("DisableRightClick")]
        [JsonProperty("DisableRightClick")]
        public bool DisableRightClick { get; set; }

        [JsonPropertyName("DisablePagerAndAlerts")]
        [JsonProperty("DisablePagerAndAlerts")]
        public bool DisablePagerAndAlerts { get; set; }

        [JsonPropertyName("RestrictIPAddressRange")]
        [JsonProperty("RestrictIPAddressRange")]
        public List<object>? RestrictIPAddressRange { get; set; }

        [JsonPropertyName("NotificationEmail")]
        [JsonProperty("NotificationEmail")]
        public object? NotificationEmail { get; set; }

        [JsonPropertyName("CalcTypeId")]
        [JsonProperty("CalcTypeId")]
        public int CalcTypeId { get; set; }

        [JsonPropertyName("CategoryId")]
        [JsonProperty("CategoryId")]
        public object? CategoryId { get; set; }

        [JsonPropertyName("PreventMovingBackwards")]
        [JsonProperty("PreventMovingBackwards")]
        public bool PreventMovingBackwards { get; set; }

        [JsonPropertyName("Shuffle")]
        [JsonProperty("Shuffle")]
        public bool Shuffle { get; set; }

        [JsonPropertyName("ActivityId")]
        [JsonProperty("ActivityId")]
        public string? ActivityId { get; set; }

        [JsonPropertyName("AllowOnlyUsersWithSpecialAccess")]
        [JsonProperty("AllowOnlyUsersWithSpecialAccess")]
        public bool AllowOnlyUsersWithSpecialAccess { get; set; }

        [JsonPropertyName("IsRetakeIncorrectOnly")]
        [JsonProperty("IsRetakeIncorrectOnly")]
        public bool IsRetakeIncorrectOnly { get; set; }
    }

}
