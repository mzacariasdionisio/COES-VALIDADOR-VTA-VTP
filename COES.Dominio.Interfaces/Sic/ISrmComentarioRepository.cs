using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SRM_COMENTARIO
    /// </summary>
    public interface ISrmComentarioRepository
    {
        int Save(SrmComentarioDTO entity);
        void Update(SrmComentarioDTO entity);
        void Delete(int srmcomcodi);
        SrmComentarioDTO GetById(int srmcomcodi);
        List<SrmComentarioDTO> List();
        List<SrmComentarioDTO> GetByCriteria();
        int SaveSrmComentarioId(SrmComentarioDTO entity);
        List<SrmComentarioDTO> BuscarOperaciones(int srmreccodi, string activo, int nroPage, int pageSize);
        int ObtenerNroFilas(int srmreccodi, string activo);
    }
}
