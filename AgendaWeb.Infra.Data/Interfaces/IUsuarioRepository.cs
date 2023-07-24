using AgendaWeb.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Data.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        /// <summary>
        /// Método para retorna os dados de um usuário baseado no email
        /// </summary>
        /// <param name="email">Email do uuário</param>
        /// <returns>Objeto usuário ou null se nao for encontrado</returns>
        Usuario? GetByEmail(string email);


        Usuario? GetByEmailESenha(string email, string senha);

    }
}
