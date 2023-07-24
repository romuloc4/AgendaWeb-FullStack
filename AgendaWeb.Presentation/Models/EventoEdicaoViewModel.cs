using System.ComponentModel.DataAnnotations;

namespace AgendaWeb.Presentation.Models
{
    public class EventoEdicaoViewModel
    {
        //campo oculto
        public Guid Id { get; set; }

        [MinLength(6, ErrorMessage = "Por favor, informe no mínimo {1} caractees.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no ,máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, infome o nome do evento.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, infome a data do evento.")]
        public string? Data { get; set; }

        [Required(ErrorMessage = "Por Favor, infrme a hora do evento.")]
        public string? Hora { get; set; }

        [MaxLength(500, ErrorMessage = "Por favor, informe no ,máximo {1} caracteres.")]
        [Required(ErrorMessage = "por Favor, informa a descriçãodo evento.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Por Favor, informe a prioridade do evento.")]
        public string? Prioridade { get; set; }

        [Required(ErrorMessage = "Por Favor, informe se o campo esta ativo ou inativo.")]
        public int? Ativo { get; set; }
    }
}
