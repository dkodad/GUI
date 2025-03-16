using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Data;
using Razor.Models.Entities;
using Razor.Models.ViewModels;
using System.Reflection;

namespace Razor.Pages.Articles
{
    public class AddModel : PageModel
    {
        private readonly RazorDbContext dbContext;

        public AddModel(RazorDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public AddArticleViewModel AddArticleRequest { get; set; }
        public void OnGet()
        {
        }
        //Because we use post method to send form data
        public void OnPost() 
        {
            AddArticleRequest.CreatedAt = DateTime.Now;
            //Convert ViewModel to EntitiesModel
            var articleEntitiesModel = new Article
            {
                Title = AddArticleRequest.Title,
                Description = AddArticleRequest.Description,
                CreatedAt = AddArticleRequest.CreatedAt,
            };
            dbContext.Articles.Add(articleEntitiesModel);
            dbContext.SaveChanges();

            ViewData["Message"] = "Article created succesfully.";
        }
    }
}
