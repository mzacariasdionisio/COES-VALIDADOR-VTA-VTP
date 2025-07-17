using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_DESPACHO_PRODGEN
    /// </summary>
    public interface IMeDespachoProdgenRepository
    {
        int GetMaxId();
        int Save(MeDespachoProdgenDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(MeDespachoProdgenDTO entity);
        void Delete(int tipoDato, DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran);
        MeDespachoProdgenDTO GetById(int dpgencodi);
        List<MeDespachoProdgenDTO> ListResumen(int tipoDato, DateTime fechaIni, DateTime fechaFin, string flagRER);
        List<MeDespachoProdgenDTO> GetByCriteria(int tipoDato, DateTime fechaIni, DateTime fechaFin, string flagIntegrante, string flagRER);
    }
}
