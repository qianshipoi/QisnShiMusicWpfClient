namespace QianShi.Music.Common.Models.Response
{
    public class UserRecordResponse
    {
        public int Code { get; set; }

        public List<PlayRecord> WeekData { get; set; } = new();
        public List<PlayRecord> AllData { get; set; } = new();
    }

    public class PlayRecord
    {
        public int PlayCount { get; set; }
        public sbyte Score { get; set; }
        public Song Song { get; set; } = default!;
    }
}