using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace D2LOffice.Models.D2L
{
    public class InstructionsUpdate
    {
        [JsonPropertyName("Text")]
        [JsonProperty("Text")]
        public required RichTextInput Text { get; set; }

        [JsonPropertyName("IsDisplayed")]
        [JsonProperty("IsDisplayed")]
        public bool IsDisplayed { get; set; }
    }

    public class DescriptionUpdate
    {
        [JsonPropertyName("Text")]
        [JsonProperty("Text")]
        public required RichTextInput Text { get; set; }

        [JsonPropertyName("IsDisplayed")]
        [JsonProperty("IsDisplayed")]
        public bool IsDisplayed { get; set; }
    }

    public class HeaderUpdate
    {
        [JsonPropertyName("Text")]
        [JsonProperty("Text")]
        public required RichTextInput Text { get; set; }

        [JsonPropertyName("IsDisplayed")]
        [JsonProperty("IsDisplayed")]
        public bool IsDisplayed { get; set; }
    }

    public class FooterUpdate
    {
        [JsonPropertyName("Text")]
        [JsonProperty("Text")]
        public required RichTextInput Text { get; set; }

        [JsonPropertyName("IsDisplayed")]
        [JsonProperty("IsDisplayed")]
        public bool IsDisplayed { get; set; }
    }

    public class QuizUpdate : Quiz
    {
        [JsonPropertyName("NumberOfAttemptsAllowed")]
        [JsonProperty("NumberOfAttemptsAllowed")]
        public int? NumberOfAttemptsAllowed { get; set; }

        [JsonPropertyName("Instructions")]
        [JsonProperty("Instructions")]
        public new InstructionsUpdate Instructions { get; set; }

        [JsonPropertyName("Description")]
        [JsonProperty("Description")]
        public new DescriptionUpdate Description { get; set; }

        [JsonPropertyName("Header")]
        [JsonProperty("Header")]
        public new HeaderUpdate Header { get; set; }

        [JsonPropertyName("Footer")]
        [JsonProperty("Footer")]
        public new FooterUpdate Footer { get; set; }

        public QuizUpdate(Quiz quiz)
        {
            var sourceProps = typeof(Quiz).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(QuizUpdate).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name && x.PropertyType.Name == sourceProp.PropertyType.Name))
                {
                    var p = destProps.FirstOrDefault(x => x.Name == sourceProp.Name && x.PropertyType.Name == sourceProp.PropertyType.Name);
                    if (p != null && p.CanWrite)
                    { // check if the property can be set or no.
                        p.SetValue(this, sourceProp.GetValue(quiz, null), null);
                    }
                }

            }

            Description = new DescriptionUpdate
            {
                IsDisplayed = quiz.Description != null ? quiz.Description.IsDisplayed : false,
                Text = new RichTextInput
                {
                    Content =  quiz.Description != null ? quiz.Description.Text.Text : "",
                    Type = "Text"
                }
            };

            Instructions = new InstructionsUpdate
            {
                IsDisplayed = quiz.Instructions != null ? quiz.Instructions.IsDisplayed : false,
                Text = new RichTextInput
                {
                    Content = quiz.Instructions != null ? quiz.Instructions.Text.Text : "",
                    Type = "Text"
                }
            };

            Header = new HeaderUpdate
            {
                IsDisplayed = quiz.Header.IsDisplayed,
                Text = new RichTextInput
                {
                    Content = quiz.Header.Text.Text,
                    Type = "Text"
                }
            };

            Footer = new FooterUpdate
            {
                IsDisplayed = quiz.Footer.IsDisplayed,
                Text = new RichTextInput
                {
                    Content = quiz.Footer.Text.Text,
                    Type = "Text"
                }
            };

            NumberOfAttemptsAllowed = quiz.AttemptsAllowed.NumberOfAttemptsAllowed;
        }
    }
}
