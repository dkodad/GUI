namespace Razor.Models.ViewModels
{
    public class EditArticleViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        //Added Author
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
