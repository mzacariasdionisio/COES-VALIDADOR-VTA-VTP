using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla Vtdvalorizacion y vatdvalorizaciondetalle
    /// </summary>
    public interface IVtdMontoPorExcesoRepository
    {
        //filtro por rango de fecha
        List<VtdMontoPorExcesoDTO> GetListFullByDateRange(int emprcodi,DateTime fechaInicio, DateTime fechaFinal);
        List<VtdMontoPorExcesoDTO> GetListPageByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
    }
}
