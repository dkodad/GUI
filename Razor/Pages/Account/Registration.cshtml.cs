using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Data;
using Razor.Models.Entities;
using Razor.Models.ViewModels;

namespace Razor.Pages.Account
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public UserViewModel Input { get; set; }

        private readonly RazorDbContext _context;
        private readonly PasswordHasher<string> _passwordHasher;

        public RegistrationModel(RazorDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<string>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = new User
            {
                Username = Input.Username,
                PasswordHash = _passwordHasher.HashPassword(null, Input.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login");
        }
    }
}
