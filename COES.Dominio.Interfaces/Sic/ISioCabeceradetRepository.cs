using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SIO_CABECERADET
    /// </summary>
    public interface ISioCabeceradetRepository
    {
        IDbConnection BeginConnection();
        IDbTransaction StartTransaction(IDbConnection conn);

        int Save(SioCabeceradetDTO entity);
        void Update(SioCabeceradetDTO entity);
        void Delete(int cabpricodi);
        SioCabeceradetDTO GetById(int cabpricodi);
        List<SioCabeceradetDTO> List();
        List<SioCabeceradetDTO> GetByCriteria(DateTime fechaProceso, int tpriecodi);
        List<SioCabeceradetDTO> GetByCriteriaPeriodo(DateTime fechaProceso);
        int Save(SioCabeceradetDTO entity, IDbConnection connection, IDbTransaction transaction);
        int? ObtenerUltNroVersion(DateTime entity, int tpriecodi);
        SioCabeceradetDTO ObtenerUltVersion(DateTime periodo, int tpriecodi);
    }
}
