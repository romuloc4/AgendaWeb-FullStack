using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Data.Entities
{
    public class Usuario
    {
        #region Propiedades
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public DateTime DataInclusao { get; set; }
        #endregion

        #region Relacionamentos 

        public List<Evento>? Eventos { get; set; }
        #endregion
    }
}
