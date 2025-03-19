
# GUI
ASP .NET RAZOR

# Project Initialization
## Requirements

To create the application, you will need:

1. Visual Studio - free to download here: https://visualstudio.microsoft.com/downloads/
2. ASP.NET
3. .NET 6.0 SDK

During installation, make sure to select **ASP.NET and Web Development**

![Setup](Images/ASP.net_install.png)

## Creating an ASP.NET Application
After opening Visual Studio, follow these steps:

1. Create a new project
2. Choose ASP.NET Core Web Application
![Setup](Images/ASP.net_bluprint.png)

After creating the project, the folder structure should look like this:
```bash
/Pages
   /Shared
   /Index.cshtml
   ...
/wwwroot
/Dependencies
/appsettings.json
Program.cs
```

## Installing Packages
To communicate with the database, we will use Entity Framework, which needs to be installed first. Follow these steps:

1. Right-click on **Dependencies** in the project
2. Manage NuGet Packages
3. Search for and install the following packages: **Microsoft.EntityFrameworkCore.SqlServer** and **Microsoft.EntityFrameworkCore.Tools**

## Creating the Database

1. Create a folder **Data** where the DbContext will be stored
2. Create the file **RazorDbContext.cs**
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
3. Create a folder **Models**, and within it, create another folder **Entities**, then create the file **Article.cs**
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
4. In **RazorDbContext.cs**, add the **DbSet** to create a table in the database using Entity Framework
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
5. Create the connection string for connecting the database to the program.

   First, add a new connection string to **appsettings.json**
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
The connection string requires three details: server, database, and whether the connection is trusted.

To find the server name, go to **View -> SQL Server Object Explorer**, find your local server, copy its name, and put it in the connection string.

The database value is just the name of the database.

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
6. Add the connection string to the program.

In **Program.cs**, add **AddDbContext**:
```csharp
using Microsoft.EntityFrameworkCore;
using Razor.Data;

builder.Services.AddDbContext<RazorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RazorConnectionString")));
```
7. Create Migrations to generate tables in the database.

Open the NuGet Console via **Tools -> NuGet Package Manager -> Package Manager Console**.

Create the migration:
```bash
Add-Migration "Initial"
```
Then use this migration to create the table:
```bash
Update-Database
```

## Creating the Add Page

In the **Pages** folder, create a new folder **Articles**. Inside that folder, create a new Razor Page as follows: **Articles -> Razor Page -> Razor Page - Empty**, and name it **Add.cshtml**.

**Add.cshtml** will look like this:
```csharp
@page
@model Razor.Pages.Articles.AddModel
@{
}

<form method="post">
<!-- class="mb-3" is a Bootstrap style, margin-bottom 16px -->
<h1 class="mb-3">Add New Article</h1>

<div class="mb-3">
    <label class="form-label">Title</label>
    <input type="text" class="form-control" required/>
</div>

<div class="mb-3">
    <label for="description" class="form-label">Description</label>
    <!-- Textarea is a larger input field for larger chunks of text -->
    <textarea id="description" class="form-control" rows="5" required></textarea>
</div>

<div class="mb-3">
    <button type="submit" class="btn btn-primary">Save</button>
</div>
</form>
```
Once the page is created, a form will appear for adding posts.

To view the page, append **/Articles/Add** to your **localhost/xxxx** URL. For example: **localhost:7152/Articles/Add**.

After submitting the form, a method will be triggered, which we will create shortly. To add the new method, open the code for the page **Add.cshtml.cs** by pressing **F7**. Then, add the new method:
```csharp
// Because we use the post method to send form data
public void OnPost()
{
}
```

Next, in order to retrieve the data sent by our form from the Razor page **Add.cshtml**, we need to create a new **ViewModel**.

In the **Models** folder, create a new subfolder called **ViewModels**, and within it create a new class called **AddArticleViewModel.cs**.

In the **AddArticleViewModel** class, add the following code:
```csharp
public class AddArticleViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
```
This class will handle sending data from the Razor page **Add.cshtml** to the backend code in **Add.cshtml.cs**. The **AddArticleViewModel** class contains the same properties as **Models->Entities->Article.cs**, except for the ID, as we will generate the ID ourselves. We only need the information displayed on the form in **Add.cshtml**.

Now, add a new property **AddArticleRequest** to **Add.cshtml.cs**:
```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Models.ViewModels;

namespace Razor.Pages.Articles
{
    public class AddModel : PageModel
    {
        // BindProperty is here because we need to get information (Title, Description) from the form. This code will be bound with the Razor page.
        [BindProperty]
        public AddArticleViewModel AddArticleRequest { get; set; }

        public void OnGet()
        {
        }

        // Because we use the post method to send form data
        public void OnPost()
        {
        }
    }
}
```
**[BindProperty]** is used so that the form data will be stored in the property after submission. Now we can use our new property **AddArticleRequest** in the Razor page **Add.cshtml**.

Modify **Add.cshtml** like this:
```csharp
<form method="post">
<h1 class="mb-3">Add New Article</h1>

<div class="mb-3">
    <label class="form-label">Title</label>
    <!-- We added the asp-for="AddArticleRequest.Title" so our property will store the value like [Title = <our title name>] -->
    <input type="text" class="form-control" asp-for="AddArticleRequest.Title" required/>
</div>

<div class="mb-3">
    <label class="form-label">Description</label>
    <!-- Textarea is a larger input field for larger chunks of text -->
    <textarea id="description" class="form-control" asp-for="AddArticleRequest.Description" rows="5" required></textarea>
</div>

<div class="mb-3">
    <button type="submit" class="btn btn-primary">Save</button>
</div>
</form>
```
We don't need the **ID** because **Entity Framework** will generate the ID automatically.
