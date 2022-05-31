using Account = QianShi.Music.Common.Models.Response.LoginStatusResponse.Account;
using Profile = QianShi.Music.Common.Models.Response.LoginStatusResponse.Profile;

namespace QianShi.Music.Common.Models.Response;

public class LoginResponse
{
    public string? Msg { get; set; }
    public int Code { get; set; }
    public int LoginType { get; set; }
    public string? Token { get; set; }
    public Account Account { get; set; } = default!;
    public Profile Profile { get; set; } = default!;
    public string? Cookie { get; set; }
    public List<Binding>? Bindings { get; set; }

    public record Binding
    {
        public int UserId { get; set; }
        public string? Url { get; set; }
        public bool Expired { get; set; }
        public dynamic? BindingTime { get; set; }
        public string? TokenJsonStr { get; set; }
        public long ExpiresIn { get; set; }
        public int RefreshTime { get; set; }
        public dynamic? Id { get; set; }
        public int Type { get; set; }
    }
}