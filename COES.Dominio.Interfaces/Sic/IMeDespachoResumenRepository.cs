using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_DESPACHO_RESUMEN
    /// </summary>
    public interface IMeDespachoResumenRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        int Save(MeDespachoResumenDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(MeDespachoResumenDTO entity);
        void Delete(int tipoDato, DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran);
        MeDespachoResumenDTO GetById(int dregencodi);
        List<MeDespachoResumenDTO> List();
        List<MeDespachoResumenDTO> GetByCriteria(int tipoDato, DateTime fechaIni, DateTime fechaFin);
    }
}
