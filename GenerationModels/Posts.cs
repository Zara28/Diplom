namespace OfficeTime.GenerationModels
{
    public class Posts
    {
        public string NameCompany { get; set; }
        public DateTime DateStart {  get; set; }
        public DateTime DateEnd { get; set; }

        public PostsRow[] PostsRow { get; set; }

        public int SumCost { get; set; }
        public int SumCount { get; set; }
    }

    public class PostsRow
    {
        public string NamePost { get; set; }
        public int Count { get; set; }
        public int Cost { get; set; }

    }
}
