using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_DEMANDA_USUARUIO
    /// </summary>
    public interface IRcaDemandaUsuarioRepository
    {
        void Save(RcaDemandaUsuarioDTO entity);

        void Delete(string rcdeulperiodo);

        int GetMaxId();
        List<RcaDemandaUsuarioDTO> ListDemandaUsuarioReporte(string periodo, string codigoZona, string codigoPuntoMedicion,string empresa, string suministrador, int regIni, int regFin);

        List<MeEnvioDTO> ObtenerListaPeriodoReporte(string fecha);

        int ListDemandaUsuarioReporteCount(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador);
        List<EqEquipoDTO> ObtenerEquipos();

        List<RcaDemandaUsuarioDTO> ListDemandaUsuarioErroresPag(string periodo,  int regIni, int regFin);

        List<RcaDemandaUsuarioDTO> ListDemandaUsuarioErroresExcel(string periodo);
        List<RcaDemandaUsuarioDTO> ListDemandaUsuarioReporteExcel(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador);
    }
}
