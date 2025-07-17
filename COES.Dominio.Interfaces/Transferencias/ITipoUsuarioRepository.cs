using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_TIPO_USUARIO
    /// </summary>
    public interface ITipoUsuarioRepository : IRepositoryBase
    {
        int Save(TipoUsuarioDTO entity);
        void Update(TipoUsuarioDTO entity);
        void Delete(System.Int32 id);
        TipoUsuarioDTO GetById(System.Int32 id);
        List<TipoUsuarioDTO> List();
        List<TipoUsuarioDTO> GetByCriteria(string nombre);
    }
}

