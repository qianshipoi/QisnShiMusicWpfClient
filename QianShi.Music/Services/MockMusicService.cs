using QianShi.Music.Common.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace QianShi.Music.Services
{
    public class MockMusicService : IMusicService
    {
        public Task<List<PlayList>> GetApplyMusic()
        {
            return Task.FromResult(new List<PlayList>
            {
                new PlayList
                {
                    Id = 1,
                    Title = "Happy Hits",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/GvYQoflE99eoeGi9jG4Bsw==/109951165375336156.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 2,
                    Title = "中嘻合壁",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/5CJeYN35LnzRDsv5Lcs0-Q==/109951165374966765.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 3,
                    Title = "Heartbreak Pop",
                    Description = "by apply music",
                    Cover = "https://p1.music.126.net/cPaBXr1wZSg86ddl47AK7Q==/109951165375130918.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 4,
                    Title = "Fesival Bangers",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/FDtX55P2NjccDna-LBj9PA==/109951165375065973.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 5,
                    Title = "Bedtime Beats",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/hC0q2dGbOWHVfg4nkhIXPg==/109951165374881177.jpg?param=512y512"
                }
            });
        }

        public Task<List<Album>> GetNewAlbum()
        {
            return Task.FromResult(new List<Album>
            {
                new Album
                {
                    Id = 1,
                    Name = "Neck & Wrist",
                    Cover = "https://p2.music.126.net/FjkXmTp7otMT7nz3iGwDSw==/109951167252015982.jpg?param=512y512",
                    Singer = new Singer
                    {
                        Id=1,
                        Name = "Pusha T"
                    }
                },
                new Album
                {
                    Id =2,
                    Name = "Ain't No Grave",
                    Cover=  "https://p2.music.126.net/uqltKrodch7bPaZWDhANdQ==/109951167240379994.jpg?param=512y512",
                    Singer = new Singer
                    {
                        Id = 2,
                        Name ="Anna Calvi"
                    }
                },
                 new Album
                {
                    Id = 3,
                    Name = "Neck & Wrist",
                    Cover = "https://p2.music.126.net/FjkXmTp7otMT7nz3iGwDSw==/109951167252015982.jpg?param=512y512",
                    Singer = new Singer
                    {
                        Id=1,
                        Name = "Pusha T"
                    }
                },
                new Album
                {
                    Id =4,
                    Name = "Ain't No Grave",
                    Cover=  "https://p2.music.126.net/uqltKrodch7bPaZWDhANdQ==/109951167240379994.jpg?param=512y512",
                    Singer = new Singer
                    {
                        Id = 2,
                        Name ="Anna Calvi"
                    }
                },
                 new Album
                {
                    Id = 5,
                    Name = "Neck & Wrist",
                    Cover = "https://p2.music.126.net/FjkXmTp7otMT7nz3iGwDSw==/109951167252015982.jpg?param=512y512",
                    Singer = new Singer
                    {
                        Id=1,
                        Name = "Pusha T"
                    }
                },
                new Album
                {
                    Id =6,
                    Name = "Ain't No Grave",
                    Cover=  "https://p2.music.126.net/uqltKrodch7bPaZWDhANdQ==/109951167240379994.jpg?param=512y512",
                    Singer = new Singer
                    {
                        Id = 2,
                        Name ="Anna Calvi"
                    }
                },
                 new Album
                {
                    Id = 7,
                    Name = "Neck & Wrist",
                    Cover = "https://p2.music.126.net/FjkXmTp7otMT7nz3iGwDSw==/109951167252015982.jpg?param=512y512",
                    Singer = new Singer
                    {
                        Id=1,
                        Name = "Pusha T"
                    }
                },
                new Album
                {
                    Id =8,
                    Name = "Ain't No Grave",
                    Cover=  "https://p2.music.126.net/uqltKrodch7bPaZWDhANdQ==/109951167240379994.jpg?param=512y512",
                    Singer = new Singer
                    {
                        Id = 2,
                        Name ="Anna Calvi"
                    }
                },
                 new Album
                {
                    Id = 9,
                    Name = "Neck & Wrist",
                    Cover = "https://p2.music.126.net/FjkXmTp7otMT7nz3iGwDSw==/109951167252015982.jpg?param=512y512",
                    Singer = new Singer
                    {
                        Id=1,
                        Name = "Pusha T"
                    }
                },
                new Album
                {
                    Id =10,
                    Name = "Ain't No Grave",
                    Cover=  "https://p2.music.126.net/uqltKrodch7bPaZWDhANdQ==/109951167240379994.jpg?param=512y512",
                    Singer = new Singer
                    {
                        Id = 2,
                        Name ="Anna Calvi"
                    }
                }
            });
        }

        public Task<List<Ranking>> GetRanking()
        {
            return Task.FromResult(new List<Ranking>
            {
                new Ranking
                {
                     Id = 1,
                     Cover = "http://p1.music.126.net/pcYHpMkdC69VVvWiynNklA==/109951166952713766.jpg?param=512y512",
                     Name ="飙升榜",
                     Description ="刚刚更新"
                },
                new Ranking
                {
                     Id = 2,
                     Cover = "http://p1.music.126.net/fhAqiflLy3eU-ldmBQByrg==/109951165613082765.jpg?param=512y512",
                     Name ="美国Billboard榜",
                     Description ="每周一更新"
                },
                new Ranking
                {
                     Id = 3,
                     Cover = "http://p1.music.126.net/rwRsVIJHQ68gglhA6TNEYA==/109951165611413732.jpg?param=512y512",
                     Name ="UK排行榜周榜",
                     Description ="刚刚更新"
                },
                new Ranking
                {
                     Id = 4,
                     Cover = "http://p1.music.126.net/oT-RHuPBJiD7WMoU7WG5Rw==/109951166093489621.jpg?param=512y512",
                     Name ="Beatport全球电子舞曲榜",
                     Description ="每周三更新"
                },
                new Ranking
                {
                     Id = 5,
                     Cover = "http://p1.music.126.net/aXUPgImt8hhf4cMUZEjP4g==/109951165611417794.jpg?param=512y512",
                     Name ="日本Oricon榜",
                     Description ="每周三更新"
                }
            });
        }

        public Task<List<PlayList>> GetRecommendMusic()
        {
            return Task.FromResult(new List<PlayList> {
                new PlayList
                {
                    Id = 1,
                    Title = "Happy Hits",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/GvYQoflE99eoeGi9jG4Bsw==/109951165375336156.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 2,
                    Title = "中嘻合壁",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/5CJeYN35LnzRDsv5Lcs0-Q==/109951165374966765.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 3,
                    Title = "Heartbreak Pop",
                    Description = "by apply music",
                    Cover = "https://p1.music.126.net/cPaBXr1wZSg86ddl47AK7Q==/109951165375130918.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 4,
                    Title = "Fesival Bangers",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/FDtX55P2NjccDna-LBj9PA==/109951165375065973.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 5,
                    Title = "Bedtime Beats",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/hC0q2dGbOWHVfg4nkhIXPg==/109951165374881177.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 6,
                    Title = "Happy Hits",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/GvYQoflE99eoeGi9jG4Bsw==/109951165375336156.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 7,
                    Title = "中嘻合壁",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/5CJeYN35LnzRDsv5Lcs0-Q==/109951165374966765.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 8,
                    Title = "Heartbreak Pop",
                    Description = "by apply music",
                    Cover = "https://p1.music.126.net/cPaBXr1wZSg86ddl47AK7Q==/109951165375130918.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 9,
                    Title = "Fesival Bangers",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/FDtX55P2NjccDna-LBj9PA==/109951165375065973.jpg?param=512y512"
                },
                new PlayList
                {
                    Id = 10,
                    Title = "Bedtime Beats",
                    Description = "by apply music",
                    Cover = "https://p2.music.126.net/hC0q2dGbOWHVfg4nkhIXPg==/109951165374881177.jpg?param=512y512"
                }
            });
        }

        public Task<List<Singer>> GetRecommmendSinger()
        {
            return Task.FromResult(new List<Singer>
            {
                new Singer
                {
                    Id= 1,
                    Name =  "Sia",
                    Cover = "https://p1.music.126.net/YKfe7kAB_WGWD6HJi7OJdw==/109951165434727308.jpg?param=512y512"
                },
                new Singer
                {
                    Id =2,
                    Name = "Rihanna",
                    Cover = "https://p1.music.126.net/f21NQmJ7Zc_HiIp48RUJqA==/18561955301751999.jpg?param=512y512"
                },
                new Singer
                {
                    Id =3,
                    Name = "OneRepublic",
                    Cover = "https://p1.music.126.net/l6IbTi-EUJTJgO-s74cBSg==/109951167129591257.jpg?param=512y512"
                },
                new Singer
                {
                    Id =4,
                    Name = "Demxntia",
                    Cover = "https://p1.music.126.net/Sn8Tsxqygi-LAzDqMAQ-ug==/109951166888428197.jpg?param=512y512"
                },
                new Singer
                {
                    Id =5,
                    Name = "Hillsong Young & Free",
                    Cover = "https://p1.music.126.net/EkoN4nRJpxBVsAtx0XhH2w==/109951165796372800.jpg?param=512y512"
                },
                new Singer
                {
                    Id =6,
                    Name = "Conor Maynard",
                    Cover = "https://p1.music.126.net/73OgxcADlIZj2LtWKVFyMw==/109951164202556693.jpg?param=512y512"
                }
            });
        }
    }
}
