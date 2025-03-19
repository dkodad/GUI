using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_GUI.Data;

namespace Razor_GUI.Pages.Articles
{

    public class ListModel : PageModel
    {
        //Like in Add.cshtml.cs we need to create this property so we can use it in the OnGet() method
        private readonly RazorDbContext dbContext;

        public List<Models.Entities.Article> Articles { get; set; }

        public ListModel(RazorDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            //Get all articles from the table Articles
            Articles = dbContext.Articles.ToList();
        }
    }
}
