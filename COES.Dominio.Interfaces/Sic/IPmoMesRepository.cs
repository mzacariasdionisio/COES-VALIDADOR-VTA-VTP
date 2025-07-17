using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_MES
    /// </summary>
    public interface IPmoMesRepository
    {
        int Save(PmoMesDTO entity);
        void Update(PmoMesDTO entity);
        void Delete(int pmmescodi);
        PmoMesDTO GetById(int pmmescodi);
        List<PmoMesDTO> List();
        List<PmoMesDTO> GetByCriteria(int pmanopcodi);
        List<PmoMesDTO> GetByCriteriaXAnio(string anios);
        PmoMesDTO GetByCriteriaXMes(DateTime fchaIni);
        int Save(PmoMesDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateBajaPeriodoSemanaMes(PmoMesDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateAprobar(PmoMesDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateEstadoProcesado(int aniomesIni, int aniomesFin, IDbConnection connection, IDbTransaction transaction);
    }
}
