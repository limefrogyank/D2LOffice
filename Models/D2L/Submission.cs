using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace D2LOffice.Models.D2L
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Entity
    {
        [JsonPropertyName("DisplayName")]
        [JsonProperty("DisplayName")]
        required public string DisplayName { get; set; }

        [JsonPropertyName("EntityId")]
        [JsonProperty("EntityId")]
        required public int EntityId { get; set; }

        [JsonPropertyName("EntityType")]
        [JsonProperty("EntityType")]
        required public string EntityType { get; set; }

        [JsonPropertyName("Active")]
        [JsonProperty("Active")]
        required public bool Active { get; set; }
    }

    public class DropboxFeedback
    {

        [JsonPropertyName("Score")]
        [JsonProperty("Score")]
        public double? Score { get; set; }

        [JsonPropertyName("Feedback")]
        [JsonProperty("Feedback")]
        required public RichText Feedback { get; set; }

        [JsonPropertyName("RubricAssessments")]
        [JsonProperty("RubricAssessments")]
        required public List<RubricAssessment> RubricAssessments { get; set; }

        [JsonPropertyName("IsGraded")]
        [JsonProperty("IsGraded")]
        public bool IsGraded { get; set; }

        [JsonPropertyName("GradedSymbol")]
        [JsonProperty("GradedSymbol")]
        required public string GradedSymbol { get; set; }
    }

    public class DropboxFeedbackOut : DropboxFeedback
    {
        [JsonPropertyName("Files")]
        [JsonProperty("Files")]
        required public List<File> Files { get; set; }
    }

    public class SubmittedBy
    {
        [JsonPropertyName("Identifier")]
        [JsonProperty("Identifier")]
        required public string Identifier { get; set; }

        [JsonPropertyName("DisplayName")]
        [JsonProperty("DisplayName")]
        required public string DisplayName { get; set; }
    }

    public class File
    {
        [JsonPropertyName("IsRead")]
        [JsonProperty("IsRead")]
        public bool IsRead { get; set; }

        [JsonPropertyName("IsFlagged")]
        [JsonProperty("IsFlagged")]
        public bool IsFlagged { get; set; }

        [JsonPropertyName("IsDeleted")]
        [JsonProperty("IsDeleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("FileId")]
        [JsonProperty("FileId")]
        public int FileId { get; set; }

        [JsonPropertyName("FileName")]
        [JsonProperty("FileName")]
        required public string FileName { get; set; }

        [JsonPropertyName("Size")]
        [JsonProperty("Size")]
        public int Size { get; set; }
    }

    public class Submission
    {
        [JsonPropertyName("Id")]
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonPropertyName("SubmittedBy")]
        [JsonProperty("SubmittedBy")]
        required public SubmittedBy SubmittedBy { get; set; }

        [JsonPropertyName("SubmissionDate")]
        [JsonProperty("SubmissionDate")]
        public DateTime SubmissionDate { get; set; }

        [JsonPropertyName("Comment")]
        [JsonProperty("Comment")]
        required public RichText Comment { get; set; }

        [JsonPropertyName("Files")]
        [JsonProperty("Files")]
        required public List<File> Files { get; set; }
    }

    public class EntityDropbox
    {
        [JsonPropertyName("Entity")]
        [JsonProperty("Entity")]
        required public Entity Entity { get; set; }

        [JsonPropertyName("Status")]
        [JsonProperty("Status")]
        public int Status { get; set; }

        [JsonPropertyName("Feedback")]
        [JsonProperty("Feedback")]
        required public DropboxFeedbackOut Feedback { get; set; }

        [JsonPropertyName("Submissions")]
        [JsonProperty("Submissions")]
        required public List<Submission> Submissions { get; set; }

        [JsonPropertyName("CompletionDate")]
        [JsonProperty("CompletionDate")]
        public DateTime? CompletionDate { get; set; }
    }


}
