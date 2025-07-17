using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_MENSAJE_REL_ARCHIVO
    /// </summary>
    public interface IInMensajeRelArchivoRepository
    {
        int GetMaxId();
        int Save(InMensajeRelArchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(InMensajeRelArchivoDTO entity);
        void Delete(int irmarcodi);
        InMensajeRelArchivoDTO GetById(int irmarcodi);
        List<InMensajeRelArchivoDTO> List();
        List<InMensajeRelArchivoDTO> GetByCriteria();
    }
}
