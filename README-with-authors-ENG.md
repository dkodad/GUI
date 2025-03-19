## Add authors to articles

We will add new property to the class **Article.cs** located in **Models->Entities**:

```csharp
public class Article
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    //Added Author
    public string Author { get; set; }
}
```

Now we need to update the scheme of our database with **Migration**. We need to access **Tools->NuGet Package Manager->Package Manager Console**, then we will write this commands:

```bash
Add-Migration "AddAuthorToArticle"
```

This migration will update the scheme of the table **Articles**, it will add row **Authors**.

```bash
Update-Database
```
Now we must alter our **ViewModels AddArticleViewModel** and **EditArticleViewModel**:

**AddArticleViewModel.cs** and **EditArticleViewModel.cs**:

```csharp
   //Added Author
   public string Author { get; set; }
```

After that, we will need to alter our pages so that we can add authors of articles and display them.

**Altered page Add.cshtml**:

```csharp
@page
@model Razor.Pages.Articles.AddModel
@{
	//Get the string from code behind the Razor page, ViewData["message"]?.ToString(); means if not null, convert to string
	var message = ViewData["message"]?.ToString();
}

<form method="post">
	<!--class="mb-3" is style from Bootstrap, margin-bottom 16px-->
<h1 class="mb-3">Add New Article</h1>

	<!--If message if not empty-->
	@if (!string.IsNullOrEmpty(message)){
		<div class="alert alert-success" role="alert">
			@message
		</div>
	}

<div class="mb-3">
	<label class="form-label">Title</label>
		<!--We added the asp-for="AddArticleRequest.Title" so our property will store the value like [Title = <our title name>]-->
		<input type="text" class="form-control" asp-for="AddArticleRequest.Title" required/>
</div>

//Added author input
<div class="mb-3">
		<label for="author" class="form-label">Author</label>
		<!--We will add author of the article-->
		<input type="text" class="form-control" asp-for="AddArticleRequest.Author" required/>

</div>

<div class="mb-3">
		<label for="description" class="form-label">Description</label>
		<!--Textarea is bigger input field for bigger chunk of text-->
		<textarea id="description" class="form-control" asp-for="AddArticleRequest.Description" rows="5" required></textarea>

</div>

<div class="mb-3">
	<button type="submit" class="btn btn-primary">Save</button>
</div>
</form>
```

**Altered code behind the page Add.cshtml.cs**:

```csharp
var articleEntitiesModel = new Article
{
    //Map fields that we are getting from the ViewModel
    Title = AddArticleRequest.Title,
    Description = AddArticleRequest.Description,
    CreatedAt = AddArticleRequest.CreatedAt,
    //Added author
    Author = AddArticleRequest.Author,
};
```

**Altered page List.cshtml**

```csharp
@page
@model Razor.Pages.Articles.ListModel
@{
}

<h1 class="mb-3">List of Articles</h1>

<!--Create table only if some Article exist-->
@if (Model.Articles != null && Model.Articles.Any())
{

	<table>
    <thead>
      <tr><!--Row-->
        <th>Id</th> <!--Column-->
        <th>Title</th>
        <th>Description</th>
        <th>CreatedAt</th>
        <!--Added Author-->
        <th>Author</th>
      </tr>
    </thead>
    <tbody>
        <!--Get all articles-->
        @foreach(var article in Model.Articles){
            <tr>
                    <td>@article.Id</td>
                    <td>@article.Title</td>
                    <td>@article.Description</td>
                    <td>@article.CreatedAt</td>
                    <!--Added Author, if null, write "Unknown" otherwise write his name-->
                    <td>@(string.IsNullOrWhiteSpace(article.Author) ? "Unknown" : article.Author)</td>
                    <td>
						<a href="/Articles/Edit/@article.Id" class="btn btn-dark">Edit</a>
                    </td>
            </tr>
        }
    </tbody>
</table>
}
else{
    <p>No articles found!</p>
}
```

After that we will see the author on the page **List.cshtml**. Now we need the option of changing the author of the page.

**Changes on the page Edit.cshtml**

```csharp
 <!--Added Author to the form so we can change it-->
<div class="mb-3">
		<label class="form-label">Author</label>

		<input type="text" class="form-control" asp-for="EditArticleViewModel.Author" />
</div>
```

**Altered code behind the page Edit.cshtml.cs**

```csharp
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
            CreatedAt = article.CreatedAt,
            //Added edit author
            Author = article.Author,
        };
    }
}

public void OnPostEdit()
{

    if (EditArticleViewModel != null)
    {
        var theArticle = dbContext.Articles.Find(EditArticleViewModel.Id);
        if( theArticle != null)
        {


            theArticle.Title = EditArticleViewModel.Title;
            theArticle.Description = EditArticleViewModel.Description;
            //Added edit author
            theArticle.Author = EditArticleViewModel.Author;

            dbContext.SaveChanges();

            ViewData["Message"] = "Article updated successfully";

        }
    }
    // View model => Domain Model
    
}
```
Now we can finally create articles with authors and we can change author in editing the article.

## Added basic design

Altered page **Index.cshtml**, that is located in folder **Pages->Shared**:

```csharp
@page
@model IndexModel
@{
    ViewData["Title"] = "Home page";
}

<div class="text-center">
    <h1 class="display-4">Welcome to my blog</h1>
    <p>You can add, edit and delete articles. It's a simple blog for learning Razor pages with Entity.Framework</p>
    <img src="images/RazorLogo.jpg" alt="Razor" />
    <img src="images/EntityFrameworkLogo.png" alt="EntityFramework" />
</div>
```
Altered page **List.cshtml**:

```csharp
@page
@model Razor.Pages.Articles.ListModel
@{
}

<h1 class="mb-3">List of Articles</h1>

<!--Create table only if some Article exist-->
@if (Model.Articles.Any())
{
    <table class="table table-striped table-bordered mt-3">
        <thead class="thead-dark">
            <tr>
                <th>ID</th>
                <th>Title</th>
                <th>Description</th>
                <th>Created At</th>
                <th>Author</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var article in Model.Articles)
            {
                <tr>
                    <td>@article.Id</td>
                    <td>@article.Title</td>
                    <td>
                        <!--Scrollable if too long-->
                        <div style="max-height: 7.5em; overflow-y: auto; white-space: pre-wrap;">
                            @article.Description
                        </div>
                    </td>
                    <td>@article.CreatedAt.ToString("dd-MM-yyyy HH:mm")</td>
                    <td>@(string.IsNullOrWhiteSpace(article.Author) ? "Unknown" : article.Author)</td>
                    <td>
                        <a href="/Articles/Edit/@article.Id" class="btn btn-sm btn-primary">Edit</a>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}
else
{
    <div class="alert alert-warning text-center mt-3">
        <p>No articles found!</p>
    </div>
}
```

