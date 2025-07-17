using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla NR_PERIODO
    /// </summary>
    public interface INrPeriodoRepository
    {
        int Save(NrPeriodoDTO entity);
        void Update(NrPeriodoDTO entity);
        void Delete(int nrpercodi);
        NrPeriodoDTO GetById(int nrpercodi);
        List<NrPeriodoDTO> List();
        List<NrPeriodoDTO> GetByCriteria();
        int SaveNrPeriodoId(NrPeriodoDTO entity);
        List<NrPeriodoDTO> BuscarOperaciones(string estado, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int PageSize);
        int ObtenerNroFilas(string estado, DateTime fechaInicio, DateTime fechaFinal);
    }
}
