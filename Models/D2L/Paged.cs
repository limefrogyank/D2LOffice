using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace D2LOffice.Models.D2L
{

    public interface IPagedResultSet
    {
        PagingInfo PagingInfo { get; set; }
    }

    public class PagedResultSet<T> : IPagedResultSet
    {
        [JsonPropertyName("PagingInfo")]
        [JsonProperty("PagingInfo")]
        public required PagingInfo PagingInfo { get; set; }

        [JsonPropertyName("Items")]
        [JsonProperty("Items")]
        public required List<T> Items { get; set; }
    }

    public class PagingInfo
    {
        [JsonPropertyName("Bookmark")]
        [JsonProperty("Bookmark")]
        public required string Bookmark { get; set; }

        [JsonPropertyName("HasMoreItems")]
        [JsonProperty("HasMoreItems")]
        public bool HasMoreItems { get; set; }
    }

    public interface IObjectListPage
    {
        string Next { get; set; }
    }

    public class ObjectListPage<T> : IObjectListPage
    {
        [JsonPropertyName("Objects")]
        [JsonProperty("Objects")]
        public required List<T> Objects { get; set; }

        [JsonPropertyName("Next")]
        [JsonProperty("Next")]
        public required string Next { get; set; }
    }


}
