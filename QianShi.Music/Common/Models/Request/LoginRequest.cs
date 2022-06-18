namespace QianShi.Music.Common.Models.Request;

public class LoginRequest : BaseRequest
{
    /// <summary>
    /// 163 网易邮箱
    /// </summary>
    [Description("email")]
    public string Email { get; set; } = default!;

    /// <summary>
    /// 密码
    /// </summary>
    [Description("password")]
    public string? Password { get; set; }

    /// <summary>
    /// md5 加密后的密码,传入后 password 将失效
    /// </summary>
    [Description("md5_password")]
    public string? Md5Password { get; set; }
}