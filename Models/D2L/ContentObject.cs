#nullable enable
using DynamicData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace D2LOffice.Models.D2L
{
    public class ContentObjectJsonConverter : JsonConverter<ContentObject>
    {
        public override ContentObject? Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader typeReader = reader;

            if (typeReader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            while (typeReader.Read())
            {
                if (typeReader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }
                string? propertyName = typeReader.GetString();

                if (propertyName != null && propertyName.Equals(nameof(ContentObject.Type)))
                {
                    break;
                }

                typeReader.Skip();

            }
            if (!typeReader.Read() || typeReader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }
            ContentObject? baseClass = default;
            ContentObjectType typeDiscriminator = (ContentObjectType)typeReader.GetInt32();
            switch (typeDiscriminator)
            {
                case ContentObjectType.Module:
                    //if (!reader.Read() || reader.GetInt32() != 0)
                    //{
                    //    throw new JsonException();
                    //}
                    //if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                    //{
                    //    throw new JsonException();
                    //}
                    baseClass = (Module?)JsonSerializer.Deserialize(ref reader, typeof(Module));
                    break;
                case ContentObjectType.Topic:
                    //if (!reader.Read() || reader.GetInt32() != 1)
                    //{
                    //    throw new JsonException();
                    //}
                    //if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                    //{
                    //    throw new JsonException();
                    //}
                    baseClass = (Topic?)JsonSerializer.Deserialize(ref reader, typeof(Topic));
                    break;
                default:
                    throw new NotSupportedException();
            }

            return baseClass;
        }

        public override void Write(Utf8JsonWriter writer, ContentObject value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value is Module module)
            {
                writer.WriteNumber("TypeDiscriminator", (int)ContentObjectType.Module);
                writer.WritePropertyName("TypeValue");
                JsonSerializer.Serialize(writer, module);
            }
            else if (value is Topic topic)
            {
                writer.WriteNumber("TypeDiscriminator", (int)ContentObjectType.Topic);
                writer.WritePropertyName("TypeValue");
                JsonSerializer.Serialize(writer, topic);
            }
            else
            {
                throw new NotSupportedException();
            }

            writer.WriteEndObject();
        }
    }

    //private class ListContentObject<TKey, TValue> :
    //        JsonConverter<Dictionary<TKey, TValue>> where TKey : struct, Enum
    //{
    //}

    [JsonConverter(typeof(ContentObjectJsonConverter))]
    public abstract class ContentObject
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        abstract public ContentObjectType Type { get;  }
        public int Id { get; set; }

    }

    public enum ContentObjectType
    {
        Module = 0,
        Topic = 1
    }

    public enum TopicType
    {
        File = 1,
        Link = 3,
        SCORM_2004 = 5,
        SCORM_2004_ROOT = 6,
        SCORM_1_2 = 7,
        SCORM_1_2_ROOT = 8
    }

    public enum ActivityType
    {
        UnknownActivity = -1,
        Module = 0,
        File = 1,
        Link = 2,
        Dropbox = 3,
        Quiz = 4,
        DiscussionForum = 5,
        DiscussionTopic = 6,
        LTI = 7,
        Chat = 8,
        Schedule = 9,
        Checklist = 10,
        SelfAssessment = 11,
        Survey = 12,
        OnlineRoom = 13,
        Scorm_1_3 = 20,
        Scorm_1_3_Root = 21,
        Scorm_1_2 = 22,
        Scorm_1_2_Root = 23
    }


    public class Module : ContentObject
    {
        public override ContentObjectType Type => ContentObjectType.Module;
        public List<ContentObject>? Structure { get; set; }
        public string? ModuleStartDate { get; set; }
        public string? ModuleEndDate { get; set; }
        public string? ModuleDueDate { get; set; }
        public bool IsHidden { get; set; }
        public bool IsLocked { get; set; }
        required public string Title { get; set; }
        required public string ShortTitle { get; set; }
        public string? Color { get; set; }
        public RichText? Description { get; set; }
        public int? ParentModuleId { get; set; }
        public int? Duration { get; set; }        
        public DateTime LastModifiedDate { get; set; }
    }

    public class Topic : ContentObject
    {
        public override ContentObjectType Type => ContentObjectType.Topic;
        public TopicType TopicType { get; set; }
        public string? Url { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? DueDate { get; set; }
        public bool IsHidden { get; set; }
        public bool IsLocked { get; set; }
        public bool IsBroken { get; set; }
        public bool? OpenAsExternalResource { get; set; }

        required public string Title { get; set; }
        required public string ShortTitle { get; set; }
        public RichText? Description { get; set; }
        public int ParentModuleId { get; set; }
        public string? ActivityId { get; set; }
        public int? Duration { get; set; }
        public bool IsExempt { get; set; }
        public int? ToolId { get; set; }
        public int? ToolItemId { get; set; }
        public ActivityType? ActivityType { get; set; }
        public int? GradeItemId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public List<int>? AssociateGradeItemIds { get; set; }
    }


    public class ContentObjectDataJsonConverter : JsonConverter<ContentObjectData>
    {
        public override ContentObjectData? Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader typeReader = reader;

            if (typeReader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            while (typeReader.Read())
            {
                if (typeReader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }
                string? propertyName = typeReader.GetString();

                if (propertyName != null && propertyName.Equals(nameof(ContentObjectData.Type)))
                {
                    break;
                }

                typeReader.Skip();

            }
            if (!typeReader.Read() || typeReader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }
            ContentObjectData? baseClass = default;
            ContentObjectType typeDiscriminator = (ContentObjectType)typeReader.GetInt32();
            switch (typeDiscriminator)
            {
                case ContentObjectType.Module:
                    baseClass = (ModuleData?)JsonSerializer.Deserialize(ref reader, typeof(ModuleData));
                    break;
                case ContentObjectType.Topic:
                    baseClass = (TopicData?)JsonSerializer.Deserialize(ref reader, typeof(TopicData));
                    break;
                default:
                    throw new NotSupportedException();
            }

            return baseClass;
        }

        public override void Write(Utf8JsonWriter writer, ContentObjectData value, JsonSerializerOptions options)
        {
            //writer.WriteStartObject();

            if (value is ModuleData module)
            {
                //writer.WriteNumber("TypeDiscriminator", (int)ContentObjectType.Module);
                //writer.WritePropertyName("TypeValue");
                JsonSerializer.Serialize(writer, module, options);
            }
            else if (value is TopicData topic)
            {
                //writer.WriteNumber("TypeDiscriminator", (int)ContentObjectType.Topic);
                //writer.WritePropertyName("TypeValue");
                JsonSerializer.Serialize(writer, topic, options);
            }
            else
            {
                throw new NotSupportedException();
            }

            //writer.WriteEndObject();
        }
    }



    [JsonConverter(typeof(ContentObjectDataJsonConverter))]
    public abstract class ContentObjectData
    {
        public RichTextInput? Description { get; set; }
        public string? ShortTitle { get; set; }
        required public string Title { get; set; }
        abstract public ContentObjectType Type { get; }
    }

    public class ModuleData : ContentObjectData
    {
        public override ContentObjectType Type => ContentObjectType.Module;
        public string? ModuleStartDate { get; set; }
        public string? ModuleEndDate { get; set; }
        public string? ModuleDueDate { get; set; }
        public bool IsHidden { get; set; }
        public bool IsLocked { get; set; }
        public int? Duration { get; set; }
    }
    public class TopicData : ContentObjectData
    {
        public override ContentObjectType Type => ContentObjectType.Topic;
        public TopicType TopicType { get; set; }
        public string? Url { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? DueDate { get; set; }
        public bool IsHidden { get; set; }
        public bool IsLocked { get; set; }
        public bool? OpenAsExternalResource { get; set; }
        public bool? MajorUpdate { get; set; }
        public string? MajorUpdateText { get; set; }
        public bool? ResetCompletionTracking { get; set; }
        public int? Duration { get; set; }
    }
}
