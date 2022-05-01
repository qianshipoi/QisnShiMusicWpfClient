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
        /// 800 二维码已过期 801 等待扫码 802 授权中 803 授权成功
        /// </summary>
        public int Code { get; set; }

        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 头像 仅当Code为802时有值
        /// </summary>
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// 昵称 仅当Code为802时有值
        /// </summary>
        public string? Nickname { get; set; }

        /// <summary>
        /// 凭据 仅当Code为803时有值
        /// </summary>
        public string? Cookie { get; set; }
    }

    public class LoginStatusResponse
    {
        public LoginStatusData Data { get; set; } = default!;
        public record LoginStatusData
        {
            public int Code { get; set; }
            public Account? Account { get; set; }
            public Profile? Profile { get; set; }
        }
        public record Account
        {
            public long Id { get; set; }
            public bool AnonimousUser { get; set; }
            public int Ban { get; set; }
            public sbyte BaoyueVersion { get; set; }
            public sbyte DonateVersion { get; set; }
            public long CreateTime { get; set; }
            public bool PaidFee { get; set; }
            public sbyte Status { get; set; }
            public sbyte TokenVersion { get; set; }
            public sbyte Type { get; set; }
            public string UserName { get; set; } = default!;
            public sbyte VipType { get; set; }
            public sbyte WhitelistAuthority { get; set; }
        }
        public record Profile
        {
            public sbyte AccountStatus { get; set; }
            public sbyte AccountType { get; set; }
            public bool Anchor { get; set; }
            public sbyte AuthStatus { get; set; }
            public bool Authenticated { get; set; }
            public int AuthenticationTypes { get; set; }
            public sbyte Authority { get; set; }
            public string AvatarUrl { get; set; } = default!;
            public string BackgroundUrl { get; set; } = default!;
            public long Birthday { get; set; }
            public int City { get; set; }
            public long CreateTime { get; set; }
            public bool DefaultAvatar { get; set; }
            public string? Description { get; set; }
            public string? DetailDescription { get; set; }
            public sbyte DjStatus { get; set; }
            public bool Followed { get; set; }
            public sbyte Gender { get; set; }
            public string LastLoginIP { get; set; } = default!;
            public long LastLoginTime { get; set; }
            public int LocationStatus { get; set; }
            public bool Mutual { get; set; }
            public string Nickname { get; set; } = default!;
            public int Province { get; set; }
            public string ShortUserName { get; set; } = default!;
            public long UserId { get; set; }
            public string UserName { get; set; } = default!;
            public int UserType { get; set; }
            public int VipType { get; set; }
            public long ViptypeVersion { get; set; }
        }
    }
}
