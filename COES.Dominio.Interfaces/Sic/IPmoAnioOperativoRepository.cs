using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_ANIO_OPERATIVO
    /// </summary>
    public interface IPmoAnioOperativoRepository
    {
        int Save(PmoAnioOperativoDTO entity);
        void Update(PmoAnioOperativoDTO entity);
        void Delete(int pmanopcodi);
        PmoAnioOperativoDTO GetById(int pmanopcodi);
        List<PmoAnioOperativoDTO> List();
        List<PmoAnioOperativoDTO> GetByCriteria(string anio);
        int Save(PmoAnioOperativoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateBajaAnioOperativo(PmoAnioOperativoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateAprobar(PmoAnioOperativoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateEstadoProcesado(int Pmmescodi, IDbConnection connection, IDbTransaction transaction);
    }
}
