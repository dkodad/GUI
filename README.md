# GUI
ASP .NET RAZOR

# Inicializace projektu
## Požadavky

K vytvoření aplikace bude potřeba:

1. Visual studio - zdarma ke stažení zde:https://visualstudio.microsoft.com/cs/downloads/
2. ASP.NET
3. .NET 6.0 SDK

při instalaci nutné vybrat **Vývoj pro ASP.NET a web**

![Setup](Images/ASP.net_install.png)

## Vytvoření ASP.NET aplikace
Po otevření Visual studia následujte tyto kroky:

1. Vytvořit nový projekt
2. Webová aplikace ASP.NET Core
![Setup](Images/ASP.net_bluprint.png)

Po vytvoření bude složka vypadat nějak takto:
```bash
/Pages
   /Shared
   /Index.cshtml
   ...
/wwwroot
/Závislosti
/appsettings.json
Program.cs
```
## Instalace balíčků
Pro komunikaci s databází budeme využívat Entity Framework, který ale bude potřeba nejdříve nainstalovat. Následujte tedy tyto kroky:

1. V projektu klikněte pravým tlačítkem na závislosti (dependencies)
2. Spravovat balíčky NuGet
3. Dále vyhledejte balíčky **Microsoft.Entity.FrameworkCore.SqlServer** a
**Microsoft.Entity.FrameworkCore.Tools** a nainstalujte je.

## Vytvoření databáze

1. Vytvořit složku **Data** kde se boudou uchovávat DbContext
2. Vytvořit soubor **RazorDbContext.cs**
```csharp
using Microsoft.EntityFrameworkCore;

namespace Razor.Data
{
    public class RazorDbContext : DbContext
    {
        public RazorDbContext(DbContextOptions<RazorDbContext> options) : base(options)
        {

        }

    }
}
```
3. Vytvořit složku **Models** ve které vytvoříme další složku **Entities** kde vytvoříme soubor **Article.cs**

```csharp
using System.ComponentModel.DataAnnotations;

namespace Razor.Models.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

```
4. Do souboru **RazorDbContext.cs** přidat **DbSet** pro vytvoření tabulky v databázi pomocí Entiti frameworku

```csharp
using Microsoft.EntityFrameworkCore;
using Razor.Models.Entities;

namespace Razor.Data
{
    public class RazorDbContext : DbContext
    {
        public RazorDbContext(DbContextOptions<RazorDbContext> options) : base(options)
        {

        }

        public DbSet<Article> Articles { get; set; }
    }
}

```
5. Vytvoření connection stringu pro připojení databáze k programu.

   Prvně přidáme do **appsetting.json** novy connection string
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "RazorConnectionString": "server = ; database = ; trusted_connection = true"
  }
}

```
Do Connection strinu se uvádí 3 údaje: server, database a jestli je připojení důvěryhodné.

Název serveru lze zjistit **Zobrazit->SQL Server Object Explorer** pote najděte lokální server jeho jméno zkopírujte a dejte do connection stringu.

Údaj database slouží jen k pojmenování databáze.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "RazorConnectionString": "server = ; database = RazorDb; trusted_connection = true"
  }
}

```
6. Přidání connection stringu do programu

V program.cs přidejte **AddDbContext**

```csharp
using Microsoft.EntityFrameworkCore;
using Razor.Data;

builder.Services.AddDbContext<RazorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RazorConnectionString")));
```
7. Vytvoření Migrations pro vytvoření tabulek do databáze.

Otevřete NuGet konzoli **Nástroje->Správa balíčků NuGet->Konzole Správce balíčků**.

Vytvoření migration.

```bash
Add-Migration "Initial"
```
Následné použití této migration pro vytvoření tabulky.

```bash
Update-Database
```
## Vytvoření stránky Add

Ve složce **Pages** vytvoříme novou složku **Articles**. Ve složce si vytvoříme novou Razor stranků následovně: **Articles->Razor Page->Razor Page - Empty** a pojmenujeme ji **Add.cshtml**.

**Add.cshtml** bude vypadat následovně:

```csharp
@page
@model Razor.Pages.Articles.AddModel
@{
}

<form method="post">
<!--class="mb-3" is style from Bootstrap, margin-bottom 16px-->
<h1 class="mb-3">Add New Article</h1>

<div class="mb-3">
	<label class="form-label">Title</label>
	<input type="text" class="form-control" />
</div>

<div class="mb-3">
	<label for="description" class="form-label">Description</label>
    <!--Textarea is bigger input field for bigger chunk of text--> 
    <textarea id="description" class="form-control" rows="5"></textarea>
</div>

<div class="mb-3">
	<button type="submit" class="btn btn-primary">Save</button>
</div>
</form>
```
Poté se nám vytvoří stránka, na které bude formulář pro přidávání příspěvků.

Pro zobrazení stránky připište v prohlížeči za **localhost/xxxx/Articles/Add**. Např.: **localhost:7152/Articles/Add**.

Jakmile odešleme formulář, spustí se nám metoda, kterou si za chvilku vytvoříme. Pro přidání nové metody potřebujeme zobrazit kód třídy stránky **Add.cshtml**. Stisknete klávesu **F7**. Poté zde přídáme novou metodu:

```csharp
//Because we use post method to send form data
public void OnPost() 
{
}
```
Nyní abychom mohli dostat data poslaná naším formulářem z naší Razor stránky **Add.cshtml**, potřebujeme si vytvořit nový **ViewModel**.

Ve složce **Models** vytvoříme novou podsložku s názvem **ViewModels** a v ní vytvoříme novou třídu s názvem **AddArticleViewModel.class**.

Ve třídě **AddArticleViewModel** vložíme tento kód:

```csharp
public class AddArticleViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
```
Tato třída bude zodpovídat za poslání dat z Razor page **Add.cshtml** do kódu za stránkou **Add.cshtml.cs**. Třída **AddArticleViewModel** obsahuje stejné vlastnosti jako **Models->Entities->Article.cs** kromě ID, protože si ID budeme generovat sami. Potřebujeme jen informace, které zobrazíme na našem formuláří **Add.cshtml**.

Nyní do **Add.cshtml.cs** přidáme novou vlastnost **AddArticleRequest** kód:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Models.ViewModels;

namespace Razor.Pages.Articles
{
    public class AddModel : PageModel
    {
        //BindProperty is here, 'cause we need to get information(Title, Description) from the form. This code will be binded with the Razor page
        [BindProperty]
        public AddArticleViewModel AddArticleRequest { get; set; }
        public void OnGet()
        {
        }
        //Because we use post method to send form data
        public void OnPost() 
        {
        }
    }
}
```
**[BindProperty]** použijeme, aby do sebe uložil data po poslání našeho formuláře. Nyní můžeme použít naší novou vlastnost **AddArticleRequest** na Razor stránce **Add.cshtml**.

Upravíme **Add.cshtml** takto:

```csharp
<form method="post">
<h1 class="mb-3">Add New Article</h1>

<div class="mb-3">
	<label class="form-label">Title</label>
    <!--We added the asp-for="AddArticleRequest.Title" so our property will store the value like [Title = <our title name>]--> 
		<input type="text" class="form-control" asp-for="AddArticleRequest.Title"/>
</div>

<div class="mb-3">
	<label class="form-label">Description</label>
		<!--Textarea is bigger input field for bigger chunk of text--> 
        <textarea id="description" class="form-control" asp-for="AddArticleRequest.Description" rows="5"></textarea>

</div>


<div class="mb-3">
	<button type="submit" class="btn btn-primary">Save</button>
</div>
</form>
```
Nepotřebujeme **ID** protože, **Entity.Framework.Core** si vygeneruje vlastní ID.

Nyní můžeme v metode **OnPost** číst z naší vlastnosti **AddArticleRequest**, která má v sobě uložené data z formuláře. Použijeme **Entity.Framework** k vytvoření nového článku. Potřebujeme použít náš vytvořený **dbContext**, protože funguje jako most mezi **Entity.Framework** a naší **databází**.
Upravíme **Add.cshtml.cs** takto:


```csharp
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
```
Pro zobrazení zprávy, že jsme úspěšně uložili příspěvek musíme upravit **Add.cshtml** takto:

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
		<label for="description" class="form-label">Description</label>
		<!--Textarea is bigger input field for bigger chunk of text-->
		<textarea id="description" class="form-control" asp-for="AddArticleRequest.Description" rows="5"></textarea>

</div>

<div class="mb-3">
	<button type="submit" class="btn btn-primary">Save</button>
</div>
</form>
```
## Přidání List stránky

Nyní si vytvoříme novou Razor stránku pro zobrazení příspěvků z databáze.
Přidáme nový soubor do složky **Articles**, která se nachází ve složce **Pages**. Vytvoříme novou Razor stránku následovně: **Articles->Razor Page->Razor Page - Empty** a pojmenujeme ji **List.cshtml**.

Zobrazíme si kód stránky pomocí klávesové zkratky **F7**. Kód stránky bude vypadat následovně:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Data;

namespace Razor.Pages.Articles
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
```
Naše články z databáze se nám uloží do listu vlastnosti **Articles**, teď je potřebujeme zobrazit na naší Razor page **List.cshtml**.
Teď uděláme strukturu stránky **List.cshtml**:

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
        <tr> <!--Row-->
          <th>Id</th> <!--Column-->
          <th>Title</th>
          <th>Description</th>
          <th>CreatedAt</th>
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
            </tr>
        }
    </tbody>
</table>
}
else{
    <p>No articles found!</p>
}
```
Po spuštění programu chceme zobrazit naší nově vytvořenou stránku **List**. Pro zobrazení stránky připište v prohlížeči za **localhost/xxxx/Articles/List**. Např.: **localhost:7152/Articles/List**. Na stránce se nám zobrazí naše vytvořené články.

## Upravit menu

Pro přidání stránek do menu potřebujeme upravit soubor **_Layout.cshtml**, který se nachází v **Pages->Shared**. Najdeme v **`<nav>`** tag **`<ul>`** a celý ho upravíme následovně:

```csharp
<ul class="navbar-nav flex-grow-1">
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-page="/Index">Home</a>
    </li>
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-page="/Articles/Add">Add Article</a>
    </li>
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-page="/Articles/List">List Articles</a>
    </li>
</ul>
```
Nyní uvidíme naše stránky v menu.