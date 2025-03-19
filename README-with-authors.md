## Přidání autorů článku

Přidáme novou vlastnost třídě **Article.cs** umístěné v **Models->Entities**:

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

Teď potřebujeme upravit naše schéma databáze pomocí **Migration**. Spustíme si **Nástroje->Správa balíčků NuGet->Konzole Správce balíčků** napíšeme následující příkaz:

```bash
Add-Migration "AddAuthorToArticle"
```
Následné použití této migration pro vytvoření tabulky.

```bash
Update-Database
```
Nyní musíme upravit naše **ViewModely AddArticleViewModel** a **EditArticleViewModel**:

**AddArticleViewModel.cs** a **EditArticleViewModel.cs**:

```csharp
   //Added Author
   public string Author { get; set; }
```

Následně budeme muset upravit naše stránky, abychom si mohli přidat autory článků a také je zobrazovat.

**Upravená stránka Add.cshtml**:

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
		<input type="text" class="form-control" asp-for="AddArticleRequest.Title"/>
</div>

<div class="mb-3">
		<label for="author" class="form-label">Author</label>
		<!--We will add author of the article-->
		<input type="text" class="form-control" asp-for="AddArticleRequest.Author" />

</div>

<div class="mb-3">
		<label for="description" class="form-label">Description</label>
		<!--Textarea is bigger input field for bigger chunk of text-->
		<textarea id="description" class="form-control" asp-for="AddArticleRequest.Description" rows="5"></textarea>

</div>

<div class="mb-3">
	<button type="submit" class="btn btn-primary">Save</button>
</div>
</form>
```

**Upravy kódu za stránkou Add.schtml.cs**:

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

**Upravená stránka List.cshtml**

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

Nyní na stránce **List.cshtml** uvidíme autora. Teď potřebujeme ještě mít možnost změnit jméno autora.

**Úpravy na stránce Edit.cshtml**

```csharp
 <!--Added Author to the form so we can change it-->
<div class="mb-3">
		<label class="form-label">Author</label>

		<input type="text" class="form-control" asp-for="EditArticleViewModel.Author" />
</div>
```

**Upravený kód stránky Edit.cshtml.cs**

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
Nyní můžeme vytvářet příspěvky s autorem a můžeme i během úpravy příspěvku autora změnit.