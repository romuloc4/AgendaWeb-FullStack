using System.ComponentModel.DataAnnotations;

namespace AgendaWeb.Presentation.Models
{
    public class AccountLoginViewModel
    {
        [Required(ErrorMessage = "Por favor, informe o Email.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Por Favor, informe a senha")]
        public string? Senha { get; set; }
    }
}
