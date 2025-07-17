using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VtdMontoPorCapacidad
    /// </summary>
    public interface IVtdMontoPorCapacidadRepository
    {
        //filtro por rango de fecha
        VtdMontoPorCapacidadDTO GetListFullByDateRange(int emprcodi,DateTime fechaInicio, DateTime fechaFinal);
        List<VtdMontoPorCapacidadDTO> GetListPageByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
    }
}
