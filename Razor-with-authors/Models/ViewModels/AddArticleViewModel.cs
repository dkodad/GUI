namespace Razor.Models.ViewModels
{
    public class AddArticleViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        //Added Author
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
