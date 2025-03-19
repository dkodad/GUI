using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor_GUI.Data;
using Razor_GUI.Models.ViewModels;

namespace Razor_GUI.Pages.Articles
{
    public class EditModel : PageModel
    {
        private readonly RazorDbContext dbContext;
        [BindProperty]
        public EditArticleViewModel EditArticleViewModel { get; set; }

        public EditModel(RazorDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(Guid id)
        {
            var article = dbContext.Articles.Find(id);

            if (article != null)
            {
                // Domain Model => View Model
                EditArticleViewModel = new EditArticleViewModel()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Description = article.Description,
                    CreatedAt = article.CreatedAt

                };
            }
        }

        public void OnPostEdit()
        {

            if (EditArticleViewModel != null)
            {
                var theArticle = dbContext.Articles.Find(EditArticleViewModel.Id);
                if (theArticle != null)
                {

                    if (theArticle.Title == EditArticleViewModel.Title && theArticle.Description == EditArticleViewModel.Description)
                    {
                        ViewData["Message"] = $"Values are the same"; 
                        Redirect("/Articles/Edit");
                        theArticle.Title = EditArticleViewModel.Title;
                    }
                    else
                    {
                        theArticle.Description = EditArticleViewModel.Description;
                        dbContext.SaveChanges();

                        ViewData["Message"] = $"Article {EditArticleViewModel.Title} updated successfully";

                        RedirectToPage("/Articles/List"); 
                    }


                }
            }
            // View model => Domain Model

        }

        public IActionResult OnPostDelete()
        {
            var theArticle = dbContext.Articles.Find(EditArticleViewModel.Id);

            if (theArticle != null)
            {
                dbContext.Articles.Remove(theArticle);
                dbContext.SaveChanges();

                ViewData["Message"] = $"Article {EditArticleViewModel.Title} was deleted";

                return RedirectToPage("/Articles/List");
            }
            return Page();
        }
    }
}
