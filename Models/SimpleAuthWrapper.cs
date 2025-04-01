using D2LOffice.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace D2LOffice.Models
{
    public class SimpleAuthWrapper
    {
        [JsonProperty("*:*:*")]
        [JsonPropertyName("*:*:*")]
        public required AuthToken Wrapper { get; set; }

    }


    public class TestAuth
    {
        public static string JSON = @"{
        ""*:*:*"": {
        ""expires_at"": 1616045691,
        ""access_token"": ""eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IjIzYmNjNDBlLWUzMjItNDdiZS1iOTU2LTQ4N2QyZjhjNTIxYiJ9.eyJpc3MiOiJodHRwczovL2FwaS5icmlnaHRzcGFjZS5jb20vYXV0aCIsImF1ZCI6Imh0dHBzOi8vYXBpLmJyaWdodHNwYWNlLmNvbS9hdXRoL3Rva2VuIiwiZXhwIjoxNjE2MDQ1NjkxLCJuYmYiOjE2MTYwNDIwOTEsInN1YiI6Ijg1MDg5NCIsInRlbmFudGlkIjoiZTA2M2ZhYjEtMDc5YS00NDYxLWI0ZDktMTRiMGI0ZTQ5OWE5IiwiYXpwIjoibG1zOmUwNjNmYWIxLTA3OWEtNDQ2MS1iNGQ5LTE0YjBiNGU0OTlhOSIsInNjb3BlIjoiKjoqOioiLCJqdGkiOiI3NWY1Y2NkYy0xMGEzLTQ4ZTItYWI3ZC01NjkzODg3Njc1NmIifQ.XNpbExGCuyb2hQ14aDO4IduA8Jb97F1TINxhSU_b5msg62l6QLaNT8ZvJBZK_dxJ9RNQJhkZTEqrqSKtJWcmmNro5LtKddnvLhCGN9dCFvLlUI_GBW2vXdtnI2t12VhP-kjD3udAcfLbCGGl3f_6JJD0eAMVa8eLu5kCrBrQWDJQVkOmFljU70raLDkVzRDW0_-HE8ENGBq4a30wM0s2uMVPfvpuDpbbcVCET21lNaPndOlvxhoL7kJiMsEVkDm9ci0q2a3kbYDxEY5ng8KSx1b3eqGeeq2VBzIOwd9m7epY2wMJzbSgCvI38x5u50kw4EJOPsDRL6SFdqkAyPm6sA""
    }
}
";
    }
}
