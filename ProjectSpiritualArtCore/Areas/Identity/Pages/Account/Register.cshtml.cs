using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ProjectSpiritualArtCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly SpiritualDbContext _DbContext;

        public RegisterModel(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            SpiritualDbContext DbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _DbContext = DbContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Nombre")]
            public string Nombre { get; set; }

            [Required]
            [Display(Name = "Apellido")]
            public string Apellido { get; set; }

            [Required]
            [Display(Name = "País")]
            public string Pais { get; set; }

            [Required]
            [Display(Name = "Ciudad")]
            public string Ciudad { get; set; }

            [Required]
            [Display(Name = "Localidad")]
            public string Localidad { get; set; }

            [Required]
            [Display(Name = "Dirección")]
            public string Direccion { get; set; }

            [Required]
            [Display(Name = "Tipo de plan")]
            public int IdPlan { get; set; }
           
            public string ImagenPerfil { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Teléfono")]
            public string Telefono { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Clave")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme su clave")]
            [Compare("Password", ErrorMessage = "La contraseña no son iguales, intenta nuevamente.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
           
            ViewData["ListaPlanes"] = await _DbContext.Planes.ToListAsync();
            
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {

                ViewData["ListaPlanes"] = await _DbContext.Planes.ToListAsync();


                var user = new Users
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    PhoneNumber = Input.Telefono,
                    Nombre = Input.Nombre,
                    Apellido = Input.Apellido,
                    Pais = Input.Pais,
                    Ciudad = Input.Ciudad,
                    Localidad = Input.Localidad,
                    Direccion = Input.Direccion,
                    ImagenPerfil = Input.ImagenPerfil,
                    IdPlan = Input.IdPlan

                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {

                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Active su cuenta",
                            $"Por favor active su cuenta en <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Click aquí</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                   
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
