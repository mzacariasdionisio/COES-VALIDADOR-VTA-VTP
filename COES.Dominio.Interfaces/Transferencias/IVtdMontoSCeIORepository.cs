using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VtdFrapagmen
    /// </summary>
    public interface IVtdMontoSCeIORepository
    {
        //filtro por rango de fecha
        List<VtdMontoSCeIODTO> GetListFullByDateRange(int empcodi,DateTime fechaInicio, DateTime fechaFinal);
        List<VtdMontoSCeIODTO> GetListPageByDateRange(int empcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
    }
}
