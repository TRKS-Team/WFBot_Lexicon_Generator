using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace WFBot_Lexicon
{
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
    public static class JsonExtensions
    {

        public static T JsonDeserialize<T>(this object source)
        {
            return source.ToJsonString().JsonDeserialize<T>();
        }

        public static string ToJsonString<T>(this T source)
        {
            return JsonConvert.SerializeObject(source);
        }

        public static T JsonDeserialize<T>(this string source)
        {
            return JsonConvert.DeserializeObject<T>(source);
        }

        public static string ToJsonString<T>(this T source, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(source, settings);
        }

        public static T JsonDeserialize<T>(this string source, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(source, settings);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var lexicon = new WFBot_Lexicon();
            lexicon.GenerateWFSales();
        }
    }

    public class WFBot_Lexicon
    {
        public T DownloadJson<T>(string url, KeyValuePair<string, string> header)
        {
            var wc = new WebClient();
            wc.Headers.Add(header.Key, header.Value);
            return wc.DownloadString(url).JsonDeserialize<T>();
        }
        public void GenerateWFSales()
        {
            var itemszh = DownloadJson<WmItem>("https://api.warframe.market/v1/items",
                new KeyValuePair<string, string>("Language", "zh-hans")).Payload.Items;
            var itemsen = DownloadJson<WmItem>("https://api.warframe.market/v1/items",
                new KeyValuePair<string, string>("Language", "en")).Payload.Items;
            var result = new List<Sale>();
            var remove = new List<string>
            {
                "Neuroptics", "Fuselage", "Engines", "Avionics", "Set", "Blade", "Disc", "Blueprint", "Handle", "Head",
                "Motor", "Barrel", "Chain", "String", "Receiver", "Motus", "Chassis", "Systems", "Grip", "Stock",
                "Link", "Carapace", "Cerebrum", "Gauntlet", "Ornament", "Grineer", "Heatsink", "Hilt", "Aegis", "Guard",
                "Receivers", "Barrels", "Limbs", "Pouch", "Stars", "Limb", "Blades", "Boot", "Wings", "Harness",
                "Collar", "Kubrow", "Subcortex", "Pathocyst", "Casing", "Engine", "Weapon", "Thrusters", "Rivet",
                "Capsule"
            };
            for (int i = 0; i < itemszh.Count; i++)
            {
                var itemzh = itemszh[i];
                var itemen = itemsen.First(i => i.Id == itemzh.Id);
                var main = itemen.ItemName;
                foreach (var word in remove)
                {
                    main = main.Replace(word, "").Trim();
                }
                var sale = new Sale { Code = itemzh.UrlName, Component = "", En = itemen.ItemName, Id = i + 1, Main = main, MarketId = itemzh.Id, Thumb = itemzh.Thumb, Zh = itemzh.ItemName };
                result.Add(sale);
            }
            File.WriteAllText("WF_Sale.json", JsonConvert.SerializeObject(result));
        }
    }
}
