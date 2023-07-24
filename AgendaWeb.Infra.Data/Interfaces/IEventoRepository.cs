using AgendaWeb.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Data.Interfaces
{
    public interface IEventoRepository : IBaseRepository<Evento>
    {
        List<Evento> GetByDatas(DateTime? dataMin, DateTime? dataMax, int? ativo, Guid idUsuario);

    }
}
