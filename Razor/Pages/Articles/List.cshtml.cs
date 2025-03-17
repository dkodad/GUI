using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Data;

namespace Razor.Pages.Articles
{
    public class ListModel : PageModel
    {
        private readonly RazorDbContext dbContext;
        public List<Models.Entities.Article> Articles { get; set; }
        public ListModel(RazorDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            Articles = dbContext.Articles.ToList();
        }
    }
}
