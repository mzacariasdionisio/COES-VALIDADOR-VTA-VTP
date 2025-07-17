using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_RESULTADOS_GAMS
    /// </summary>
    public interface IPfrResultadosGamsRepository
    {        
        void Update(PfrResultadosGamsDTO entity);
        void Delete(int pfrrgcodi);
        PfrResultadosGamsDTO GetById(int pfrrgcodi);
        List<PfrResultadosGamsDTO> List();
        List<PfrResultadosGamsDTO> GetByCriteria();
        int Save(PfrResultadosGamsDTO entity, IDbConnection conn, DbTransaction tran);
        List<PfrResultadosGamsDTO> ListByTipoYEscenario(int pfresccodi, int pfrrgtipo);
    }
}
