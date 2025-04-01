using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace D2LOffice.Models.D2L
{

    public class Type
    {
        [JsonPropertyName("Id")]
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Code")]
        [JsonProperty("Code")]
        public required string Code { get; set; }

        [JsonPropertyName("Name")]
        [JsonProperty("Name")]
        public required string Name { get; set; }
    }

    public class OrgUnit
    {
        [JsonPropertyName("Id")]
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Type")]
        [JsonProperty("Type")]
        public required Type Type { get; set; }

        [JsonPropertyName("Name")]
        [JsonProperty("Name")]
        public required string Name { get; set; }

        [JsonPropertyName("Code")]
        [JsonProperty("Code")]
        public required string Code { get; set; }

        [JsonPropertyName("HomeUrl")]
        [JsonProperty("HomeUrl")]
        public required string HomeUrl { get; set; }

        [JsonPropertyName("ImageUrl")]
        [JsonProperty("ImageUrl")]
        public required string ImageUrl { get; set; }
    }

    public class Access
    {
        [JsonPropertyName("IsActive")]
        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("StartDate")]
        [JsonProperty("StartDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("EndDate")]
        [JsonProperty("EndDate")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("CanAccess")]
        [JsonProperty("CanAccess")]
        public bool CanAccess { get; set; }

        [JsonPropertyName("ClasslistRoleName")]
        [JsonProperty("ClasslistRoleName")]
        public required string ClasslistRoleName { get; set; }

        [JsonPropertyName("LISRoles")]
        [JsonProperty("LISRoles")]
        public required List<string> LISRoles { get; set; }
    }

    public class Enrollment
    {
        [JsonPropertyName("OrgUnit")]
        [JsonProperty("OrgUnit")]
        public required OrgUnit OrgUnit { get; set; }

        [JsonPropertyName("Access")]
        [JsonProperty("Access")]
        public required Access Access { get; set; }

        [JsonPropertyName("PinDate")]
        [JsonProperty("PinDate")]
        public DateTime? PinDate { get; set; }
    }


}