using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class CatlistResponse
    {
        public int Code { get; set; }
        public Cat? All { get; set; }
        public List<Cat> Sub { get; set; } = new List<Cat>();
        public Dictionary<int, string> Categories { get; set; } = new Dictionary<int, string>();
    }

    public class SongUrlResponse
    {
        public int Code { get; set; }
        public List<SongUrl> Data { get; set; } = new();

        public class SongUrl
        {
            [JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonPropertyName("url")]
            public string Url { get; set; }

            [JsonPropertyName("br")]
            public int Br { get; set; }

            [JsonPropertyName("size")]
            public int Size { get; set; }

            [JsonPropertyName("md5")]
            public string Md5 { get; set; }

            [JsonPropertyName("code")]
            public int Code { get; set; }

            [JsonPropertyName("expi")]
            public int Expi { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("gain")]
            public double Gain { get; set; }

            [JsonPropertyName("fee")]
            public int Fee { get; set; }

            [JsonPropertyName("uf")]
            public object Uf { get; set; }

            [JsonPropertyName("payed")]
            public int Payed { get; set; }

            [JsonPropertyName("flag")]
            public int Flag { get; set; }

            [JsonPropertyName("canExtend")]
            public bool CanExtend { get; set; }

            [JsonPropertyName("freeTrialInfo")]
            public object FreeTrialInfo { get; set; }

            [JsonPropertyName("level")]
            public string Level { get; set; }

            [JsonPropertyName("encodeType")]
            public string EncodeType { get; set; }

            [JsonPropertyName("freeTrialPrivilege")]
            public FreeTrialPrivilege FreeTrialPrivilege { get; set; }

            [JsonPropertyName("freeTimeTrialPrivilege")]
            public FreeTimeTrialPrivilege FreeTimeTrialPrivilege { get; set; }

            [JsonPropertyName("urlSource")]
            public int UrlSource { get; set; }
        }

        public class FreeTimeTrialPrivilege
        {
            [JsonPropertyName("resConsumable")]
            public bool ResConsumable { get; set; }

            [JsonPropertyName("userConsumable")]
            public bool UserConsumable { get; set; }

            [JsonPropertyName("type")]
            public int Type { get; set; }

            [JsonPropertyName("remainTime")]
            public int RemainTime { get; set; }
        }

        public class FreeTrialPrivilege
        {
            [JsonPropertyName("resConsumable")]
            public bool ResConsumable { get; set; }

            [JsonPropertyName("userConsumable")]
            public bool UserConsumable { get; set; }

            [JsonPropertyName("listenType")]
            public object ListenType { get; set; }
        }
    }
}