using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BibliotecaDevlights.Business.DTOs.Auth
{
    public class RegisterDto : IValidatableObject
    {
        [Required(ErrorMessage = "Username es requerido")]
        [StringLength(100, MinimumLength = 3,
       ErrorMessage = "Username debe tener entre 3 y 100 caracteres")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password es requerido")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Password mínimo 8 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)",
            ErrorMessage = "Password debe tener mayúscula, minúscula y número")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmar password es requerido")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
            {
                yield return new ValidationResult(
                    "Las contraseñas no coinciden",
                    new[] { nameof(ConfirmPassword) });
            }
        }
    }
}
