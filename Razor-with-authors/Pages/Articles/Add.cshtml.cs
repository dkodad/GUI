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
        //dbContext is handled by the application in Program.cs. 
        //We get instance of dbContext which we will use in the OnPost() method.
        private readonly RazorDbContext dbContext;

        public AddModel(RazorDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //BindProperty is here, 'cause we need to get information(Title, Description) from the form. This code will be binded with the Razor page
        [BindProperty]
        public AddArticleViewModel AddArticleRequest { get; set; }
        public void OnGet()
        {
        }
        //Because we use post method to send form data
        public void OnPost()
        {
            AddArticleRequest.CreatedAt = DateTime.Now;
            //Convert ViewModel to EntitiesModel, 'cause dbContext cares only about Entities model
            var articleEntitiesModel = new Article
            {
                //Map fields that we are getting from the ViewModel
                Title = AddArticleRequest.Title,
                Description = AddArticleRequest.Description,
                CreatedAt = AddArticleRequest.CreatedAt,
                //Added author
                Author = AddArticleRequest.Author,
            };
            //Use EntitiesModel to pass it to the Entity.Framework to the database
            dbContext.Articles.Add(articleEntitiesModel);
            //Make changes in the database
            dbContext.SaveChanges();

            //Notification that we successfully created an article 
            ViewData["Message"] = "Article created succesfully.";
        }
    }
}
