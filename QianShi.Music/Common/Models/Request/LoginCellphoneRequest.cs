using System.ComponentModel;

namespace QianShi.Music.Common.Models.Request;

public class LoginCellphoneRequest : BaseRequest
{
    /// <summary>
    /// 手机号码
    /// </summary>
    [Description("phone")]
    public string Phone { get; set; } = default!;
    /// <summary>
    /// 密码
    /// </summary>
    [Description("password")]
    public string? Password { get; set; } = default!;
    /// <summary>
    /// md5 加密后的密码,传入后 password 将失效
    /// </summary>
    [Description("md5_password")]
    public string? Md5Password { get; set; } = default!;
    /// <summary>
    ///  国家码，用于国外手机号登录，例如美国传入：1
    /// </summary>
    [Description("countrycode")]
    public string? Countrycode { get; set; }
    /// <summary>
    /// 验证码,使用 /captcha/sent接口传入手机号获取验证码,调用此接口传入验证码,可使用验证码登录,传入后 password 参数将失效
    /// </summary>
    [Description("captcha")]
    public string? Captcha { get; set; }
}