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
<h1 class="mb-3">Add New Article</h1>

<div class="mb-3">
	<label class="form-label">Title</label>
	<input type="text" class="form-control" />
</div>

<div class="mb-3">
	<label class="form-label">Description</label>
	<input type="text" class="form-control" />
</div>

<div class="mb-3">
	<button type="submit" class="btn btn-primary">Save</button>
</div>
</form>
```
Poté se nám vytvoří stránka, na které bude formulář pro přidávání příspěvků. 

Pro zobrazení stránky připište v prohlížeči za localhost/xxxx/Articles/Add. Např.: localhost:7152/Articles/Add

Chceme zobrazit kód třídy stránky **Add.cshtml**. Stisknete klávesu F7. Poté zde přídáme novou metodu:

```csharp
//Because we use post method to send form data
public void OnPost() 
{
}
```

Ve složce **Models** vytvoříme novou podložku s názvem **ViewModels** a v ní vytvoříme novou třídu s názvem **AddArticleViewModel.class**.

Ve třídě **AddArticleViewModel** vložíme tento kód:

```csharp
public class AddArticleViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

Nyní do **Add.cshtml.cs** vložíme tento kód:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Models.ViewModels;

namespace Razor.Pages.Articles
{
    public class AddModel : PageModel
    {
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

Upravíme **Add.cshtml** takto:

```csharp
<form method="post">
<h1 class="mb-3">Add New Article</h1>

<div class="mb-3">
	<label class="form-label">Title</label>
		<input type="text" class="form-control" asp-for="AddArticleRequest.Title"/>
</div>

<div class="mb-3">
	<label class="form-label">Description</label>
		<input type="text" class="form-control" asp-for="AddArticleRequest.Description" />
</div>


<div class="mb-3">
	<button type="submit" class="btn btn-primary">Save</button>
</div>
</form>
```

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
```
Pro zobrazení zprávy, že jsme úspěšně uložili příspěvek musíme upravit **Add.cshtml** takto:

```csharp
@page
@model Razor.Pages.Articles.AddModel
@{
	var message = ViewData["message"]?.ToString();
}

<form method="post">
<h1 class="mb-3">Add New Article</h1>

	@if (!string.IsNullOrEmpty(message)){
		<div class="alert alert-success" role="alert">
			@message
		</div>
	}

<div class="mb-3">
	<label class="form-label">Title</label>
		<input type="text" class="form-control" asp-for="AddArticleRequest.Title"/>
</div>

<div class="mb-3">
	<label class="form-label">Description</label>
		<input type="text" class="form-control" asp-for="AddArticleRequest.Description" />
</div>

<div class="mb-3">
	<button type="submit" class="btn btn-primary">Save</button>
</div>
</form>
```
Přidáme nový soubor do složky **Articles**, která se nachází ve složce **Pages**. Vytvoříme novou Razor stránku následovně: **Articles->Razor Page->Razor Page - Empty** a pojmenujeme ji **List.cshtml**.

Zobrazíme si kód stránky pomocí klávesové zkratky F7. Kód stránky bude vypadat následovně:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Data;

namespace Razor.Pages.Articles
{
    public class ListModel : PageModel
    {
        private readonly RazorDbContext dbContext;
        public List<Models.Entities.Article> Articles { get; set; };
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
```
Teď uděláme strukturu stránky **List.cshtml**:

```csharp
@page
@model Razor.Pages.Articles.ListModel
@{
}

<h1 class="mb-3">List of Articles</h1>

@if (Model.Articles != null && Model.Articles.Any())
{

	<table>
    <thead>
        <tr>
        <th>Id</th>
        <th>Title</th>
        <th>Description</th>
        <th>CreatedAt</th>
    </tr>
    </thead>
    <tbody>
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
Po spuštění programu chceme zobrazit naší nově vytvořenou stránku **List**. Pro zobrazení stránky připište v prohlížeči za localhost/xxxx/Articles/List. Např.: localhost:7152/Articles/List

Pro přidání stránek do menu potřebujeme upravit soubor **_Layout.cshtml**, který se nachází v **Pages->Shared**. Najdeme v <nav> tag <ul> a celý ho upravíme následovně:

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