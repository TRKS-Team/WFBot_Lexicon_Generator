using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using WFBot_Lexicon_Generator;

namespace WFBot_Lexicon
{
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

            if (args.Contains("dict"))
            {
                lexicon.GenerateWFDict();
            }

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

        public void GenerateWFDict()
        {
            var hc = new HttpClient();
            var query = hc.GetStringAsync(
                    "https://warframe.huijiwiki.com/api.php?action=query&format=json&prop=revisions&titles=Data%3AUserDict.json&formatversion=2&rvprop=content&rvlimit=1")
                .Result.JsonDeserialize<UserDictQuery>();
            var content = query.Query.Pages.First().Revisions.First().Content.JsonDeserialize<Content>();
            var dict = hc
                .GetStringAsync("https://wfbot.kraber.top:8888/Resources/Richasy/WFA_Lexicon%40WFA5/WF_Dict.json")
                .Result.JsonDeserialize<List<Dict>>();
            var diff = dict.Where(d => !content.Text.Values.Select(t => t.ToLower()).Contains(d.Zh.ToLower()) && !content.Category.Values.Select(t => t.ToLower()).Contains(d.Zh.ToLower()));
            var result = new List<Dict>();
            result = result.Concat(diff).ToList();
            foreach (var key in content.Category.Keys)
            {
                result.Add(new Dict{ En = key, Zh = content.Category[key]});
            }

            foreach (var key in content.Text.Keys)
            {
                result.Add(new Dict{ En = key, Zh = content.Text[key]});
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[i].Id = i;
            }
            File.WriteAllText("WFBot_Dict.json", JsonConvert.SerializeObject(result));
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
            File.WriteAllText("WFBot_Sale.json", JsonConvert.SerializeObject(result));
        }
    }
}
