using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla NR_PERIODO_RESUMEN
    /// </summary>
    public interface INrPeriodoResumenRepository
    {
        int Save(NrPeriodoResumenDTO entity);
        void Update(NrPeriodoResumenDTO entity);
        void Delete(int nrperrcodi);        
        NrPeriodoResumenDTO GetById(int nrperrcodi);
        NrPeriodoResumenDTO GetById(int nrpercodi, int nrcptcodi);
        List<NrPeriodoResumenDTO> List();
        List<NrPeriodoResumenDTO> List(int idNrsmodCodi, int idNrperCodi);
        List<NrPeriodoResumenDTO> GetByCriteria();
        int SaveNrPeriodoResumenId(NrPeriodoResumenDTO entity);
        List<NrPeriodoResumenDTO> BuscarOperaciones(int nrsmodCodi, string estado, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int PageSize);
        int ObtenerNroFilas(int nrsmodCodi, string estado, DateTime fechaInicio, DateTime fechaFinal);
    }
}
