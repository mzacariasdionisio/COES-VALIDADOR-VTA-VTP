using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_TOPOLOGIA
    /// </summary>
    public interface ICpTopologiaRepository
    {
        int Save(CpTopologiaDTO entity);
        void Update(CpTopologiaDTO entity);
        void Delete(int topcodi);
        CpTopologiaDTO GetById(int topcodi);
        List<CpTopologiaDTO> List();
        List<CpTopologiaDTO> GetByCriteria(DateTime topfechaIni, DateTime topfechaFin, short tophorizonte);
        CpTopologiaDTO GetByIdRsf(int topcodi);
        List<CpTopologiaDTO> ListNombre(string nombre);
        CpTopologiaDTO GetByFechaTopfinal(DateTime topfecha, string toptipo);
        List<CpTopologiaDTO> GetTopReprogramadas(int topcodi);
        List<CpTopologiaDTO> ObtenerEscenariosPorDia(DateTime fechaProceso);
        List<CpTopologiaDTO> ObtenerEscenariosPorDiaConsulta(DateTime fechaProceso, int tipo);
        List<CpTopologiaDTO> ObtenerTipoRestriccion();
        CpTopologiaDTO ObtenerUltimoEscenarioReprogramado(DateTime topfecha);
        List<CpTopologiaDTO> ListEscenarioReprograma(DateTime fecha);
        #region Ticket IMME
        List<CpTopologiaDTO> ListaTopFinalDiario(DateTime fini, DateTime ffin);
        #endregion
    }
}
