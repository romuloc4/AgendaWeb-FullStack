using AgendaWeb.Infra.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgendaWeb.Presentation.Models
{
    public class EventoConsultaViewModel
    {
        [Required(ErrorMessage = "Por favor, Informe a data de início.")]
        public string? DataMin { get; set; }

        [Required(ErrorMessage = "Por favor, Informe a data de término. ")]
        public string? DataMax { get; set; }

        [Required(ErrorMessage = "Por favor, marque Ativo ou Inativo. ")]
        public int? Ativo { get; set; }

        //lista de eventos que será utilizada para exibir
        //na página o resultado das consultas feita no banco
        public List<Evento>? Eventos { get; set; }
    }
}

