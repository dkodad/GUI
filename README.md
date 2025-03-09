# GUI
ASP .NET RAZOR

# Inicializace projektu
## Požadavky

K vytvoření aplikace bude potřeba:

1. Visual studio - zdarma ke stažení zde:https://visualstudio.microsoft.com/cs/downloads/
2. ASP.NET

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