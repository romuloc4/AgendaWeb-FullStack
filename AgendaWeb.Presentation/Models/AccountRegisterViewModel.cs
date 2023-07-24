using System.ComponentModel.DataAnnotations;

namespace AgendaWeb.Presentation.Models
{
    public class AccountRegisterViewModel
    {
        [MinLength(6, ErrorMessage = "Por favor, Informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, Informe no mínimo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o Nome.")]
        public string? Nome { get; set; }

        [EmailAddress(ErrorMessage = "Por Favor, informe um endereço de email válido.")]
        [Required(ErrorMessage = "Por favor, informe o Email.")]
        public string? Email { get; set; }

        [MinLength(8, ErrorMessage = "Por favor, Informe no mínimo {1} caracteres.")]
        [MaxLength(20, ErrorMessage = "Por favor, Informe no mínimo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe a Senha.")]
        public string? Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não são iguais.")]
        [Required(ErrorMessage = "Por favor, Confirme a Senha.")]
        public string? SenhaConfirmacao { get; set; }
    }
}
