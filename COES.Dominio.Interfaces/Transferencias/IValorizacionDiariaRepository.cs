using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTD_VALORIZACION - VTD_VALORIZACIONDETALLE
    /// </summary>
    public interface IValorizacionDiariaRepository
    {
        List<ValorizacionDiariaDTO> GetListFullByFilter(int emprcodi,DateTime fechaInicio, DateTime fechaFinal);
        List<ValorizacionDiariaDTO> GetListPageByFilter(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
        ValorizacionDiariaDTO GetMontoCalculadoPorMesPorParticipante(int emprcodi, DateTime fechaInicio,DateTime fechaFinal);

    }
}
