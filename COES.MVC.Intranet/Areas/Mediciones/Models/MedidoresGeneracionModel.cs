using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Mediciones;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Mediciones.Models
{
    /// <summary>
    /// Maneja los datos del reporte de medidores
    /// </summary>
    public class MedidoresGeneracionModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<MeMedicion96DTO> ListaDatos { get; set; }
        public List<MeMedicion48DTO> ListaDatosGeneracionNoCoes { get; set; }

        public List<MeMedicion96DTO> EntidadTotal { get; set; }

        public List<MeMedicion48DTO> EntidadTotalGeneracionNoCoes { get; set; }
        public string TextoCabecera { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public string IndicadorPublico { get; set; }
    }

    /// <summary>
    /// Maneja los datos del reporte de medidores
    /// </summary>
    public class ReporteMedidoresModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaConsulta { get; set; }
        public List<MedicionReporteDTO> ListaCuadrosFE { get; set; }
        public List<MedicionReporteDTO> ListaCuadrosTG { get; set; }
        public MedicionReporteDTO DatosReporte { get; set; }
        public List<MedicionReporteDTO> ReporteFuenteEnergia { get; set; }

        public bool TieneAlertaValidacion { get; set; }
        public List<LogErrorHOPvsMedidores> ListaValidacion { get; set; }
    }
}