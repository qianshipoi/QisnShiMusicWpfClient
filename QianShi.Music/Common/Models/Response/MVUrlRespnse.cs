namespace QianShi.Music.Common.Models.Response
{
    public class MVUrlRespnse
    {
        public int Code { get; set; }

        public MVUrl? Data { get; set; }

        public class MVUrl
        {
            public long Id { get; set; }
            public string? Url { get; set; }
            public int R { get; set; }
            public long Size { get; set; }
            public string? Md5 { get; set; }
            public int Code { get; set; }
            public int Expi { get; set; }
            public int Fee { get; set; }
            public int MvFee { get; set; }
            public int St { get; set; }
            public string? PromotionVo { get; set; }
            public string? Msg { get; set; }
        }
    }
}
