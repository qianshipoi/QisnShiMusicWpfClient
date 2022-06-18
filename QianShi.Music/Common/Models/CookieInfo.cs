using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models
{
    public class CookieInfo
    {
        [JsonConstructor]
        public CookieInfo()
        { }

        public CookieInfo(Cookie cookie)
        {
            this.Name = cookie.Name;
            this.Value = cookie.Value;
            this.Path = cookie.Path;
            this.Domain = cookie.Domain;
        }

        public string Name { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string Path { get; set; } = default!;
        public string Domain { get; set; } = default!;

        public Cookie ToCookie()
        {
            return new Cookie(this.Name, this.Value, this.Path, this.Domain);
        }
    }
}