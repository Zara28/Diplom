namespace OfficeTime.GenerationModels
{
    public class Posts
    {
        public string NameComppany { get; set; }
        public DateTime DateStart {  get; set; }
        public DateTime DateEnd { get; set; }

        public List<PostsRow> PostsRow { get; set; }
    }

    public class PostsRow
    {
        public string NamePost { get; set; }
        public int Count { get; set; }
        public int Cost { get; set; }

    }
}
