using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;
namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RI_DETALLE_REVISION
    /// </summary>
    public interface IRiDetalleRevisionRepository
    {
        int Save(RiDetalleRevisionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RiDetalleRevisionDTO entity);
        void UpdateEstado(RiDetalleRevisionDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int dervcodi);
        RiDetalleRevisionDTO GetById(int dervcodi);
        List<RiDetalleRevisionDTO> List();
        List<RiDetalleRevisionDTO> GetByCriteria();
        List<RiDetalleRevisionDTO> ListByRevicodi(int revicodi);
    }
}
