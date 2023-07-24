using System.ComponentModel.DataAnnotations;

namespace AgendaWeb.Presentation.Models
{
    public class EventoRelatorioViewModel
    { 
        [Required(ErrorMessage = "Por favor, Informe a data de início.")]
        public string? DataMin { get; set; }

        [Required(ErrorMessage = "Por favor, Informe a data de término. ")]
        public string? DataMax { get; set; }

        [Required(ErrorMessage = "Por favor, marque Ativo ou Inativo. ")]
        public int? Ativo { get; set; }

        [Required(ErrorMessage = "Por favor, selecioe o fomato desejado ")]
        public int? Formato { get; set; }
    }
}
