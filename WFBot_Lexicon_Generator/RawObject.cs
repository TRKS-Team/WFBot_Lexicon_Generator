using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WFBot_Lexicon_Generator
{
    public partial class Content
    {
        [JsonProperty("Text")]
        public Dictionary<string, string> Text { get; set; }

        [JsonProperty("Category")]
        public Dictionary<string, string> Category { get; set; }
    }
    public partial class UserDictQuery
    {
        [JsonProperty("continue")]
        public Continue Continue { get; set; }

        [JsonProperty("warnings")]
        public Warnings Warnings { get; set; }

        [JsonProperty("query")]
        public Query Query { get; set; }
    }

    public partial class Continue
    {
        [JsonProperty("rvcontinue")]
        public string Rvcontinue { get; set; }

        [JsonProperty("continue")]
        public string ContinueContinue { get; set; }
    }

    public partial class Query
    {
        [JsonProperty("pages")]
        public List<Page> Pages { get; set; }
    }

    public partial class Page
    {
        [JsonProperty("pageid")]
        public long Pageid { get; set; }

        [JsonProperty("ns")]
        public long Ns { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("revisions")]
        public List<Revision> Revisions { get; set; }
    }

    public partial class Revision
    {
        [JsonProperty("contentformat")]
        public string Contentformat { get; set; }

        [JsonProperty("contentmodel")]
        public string Contentmodel { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public partial class Warnings
    {
        [JsonProperty("main")]
        public Main Main { get; set; }

        [JsonProperty("revisions")]
        public Main Revisions { get; set; }
    }

    public partial class Main
    {
        [JsonProperty("warnings")]
        public string Warnings { get; set; }
    }
    public partial class Dict
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("zh")]
        public string Zh { get; set; }

        [JsonProperty("en")]
        public string En { get; set; }
    }
    public partial class Sale
    {
        private sealed class MarketIdEqualityComparer : IEqualityComparer<Sale>
        {
            public bool Equals(Sale x, Sale y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.MarketId == y.MarketId;
            }

            public int GetHashCode(Sale obj)
            {
                return (obj.MarketId != null ? obj.MarketId.GetHashCode() : 0);
            }
        }

        public static IEqualityComparer<Sale> MarketIdComparer { get; } = new MarketIdEqualityComparer();

        [JsonProperty("marketId")]
        public string MarketId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("main")]
        public string Main { get; set; }

        [JsonProperty("component")]
        public string Component { get; set; }

        [JsonProperty("zh")]
        public string Zh { get; set; }

        [JsonProperty("en")]
        public string En { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }
    public partial class WmItem
    {
        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }
    public partial class Payload
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("item_name")]
        public string ItemName { get; set; }

        [JsonProperty("url_name")]
        public string UrlName { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }
}
