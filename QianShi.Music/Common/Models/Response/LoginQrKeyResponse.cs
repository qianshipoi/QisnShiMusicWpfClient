namespace QianShi.Music.Common.Models.Response
{
    public class LoginQrKeyResponse
    {
        public int Code { get; set; }
        public Result Data { get; set; } = null!;
        public record Result
        {
            public int Code { get; set; }
            public string Unikey { get; set; } = null!;
        }
    }

    public class LoginQrCreateResponse
    {
        public int Code { get; set; }

        public Result Data { get; set; } = null!;

        public record Result
        {
            public string Qrurl { get; set; } = string.Empty;

            public string Qrimg { get; set; } = string.Empty;
        }
    }

    public class LoginQrCheckResponse
    {
        /// <summary>
        /// 800 二维码已过期 801 等待扫码 803 授权成功
        /// </summary>
        public int Code { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? Cookie { get; set; }
    }
}
