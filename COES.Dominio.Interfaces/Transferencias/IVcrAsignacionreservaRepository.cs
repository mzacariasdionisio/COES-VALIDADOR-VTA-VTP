using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_ASIGNACIONRESERVA
    /// </summary>
    public interface IVcrAsignacionreservaRepository
    {
        int Save(VcrAsignacionreservaDTO entity);
        void Update(VcrAsignacionreservaDTO entity);
        void Delete(int vcrecacodi);
        VcrAsignacionreservaDTO GetById(int vcrarcodi);
        List<VcrAsignacionreservaDTO> List();
        List<VcrAsignacionreservaDTO> GetByCriteria();
        List<VcrAsignacionreservaDTO> ListPorMesURS(int vcrecacodi, int grupocodi);
        List<VcrAsignacionreservaDTO> GetByCriteriaVcrAsignacionReservaOferta(int vcrecacodi, DateTime fecha);
        VcrAsignacionreservaDTO GetByIdEmpresa(int vcrecacodi, int emprcodi);
        //FUNCIONES DE TRABAJO PARA EveRsfhoraDTO
        List<VcrEveRsfhoraDTO> ExportarReservaAsignadaSEV(DateTime fechaInicio, DateTime fechaFin);
        List<EveRsfhoraDTO> ExportarReservaAsignadaSEV2020(DateTime fechaInicio, DateTime fechaFin);
        decimal GetBydMPA2020(int vcrecacodi, DateTime dDia);

        VcrAsignacionreservaDTO GetBydMPA(int vcrecacodi, DateTime dDia);
    }
}
