using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VtdMontoPorEnergia
    /// </summary>
    public interface IVtdMontoPorEnergiaRepository
    {
        //filtro por rango de fecha
        List<VtdMontoPorEnergiaDTO> GetListFullByDateRange(int emprcodi,DateTime fechaInicio, DateTime fechaFinal);
        List<VtdMontoPorEnergiaDTO> GetListPageByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
    }
}
