using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VtdMontoPorPeaje
    /// </summary>
    public interface IVtdMontoPorPeajeRepository
    {
        //filtro por rango de fecha
        VtdMontoPorPeajeDTO GetListFullByDateRange(int emprcodi,DateTime fechaInicio, DateTime fechaFinal);
        List<VtdMontoPorPeajeDTO> GetListPageByDateRange(DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
    }
}
