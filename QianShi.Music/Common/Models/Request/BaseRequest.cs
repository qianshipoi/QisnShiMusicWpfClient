using System.ComponentModel;

namespace QianShi.Music.Common.Models.Request;

public class BaseRequest
{
    [Description("time")]
    public long? Time { get; set; }
}