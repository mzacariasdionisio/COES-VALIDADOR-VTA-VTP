using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Mediciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.DemandaMaxima.Models
{
    public class DemandaMaximaModel
    {
        public int IdEmpresa { get; set; }
        public int IdFormato { get; set; }
        public int IdModulo { get; set; }
        public string Anho { get; set; }
        public string Mes { get; set; }
        public MeFormatoDTO Formato { get; set; }
        public List<SeguridadServicio.EmpresaDTO> ListaEmpresas { get; set; }       
        public List<SiTipoempresaDTO> ListaTipoempresa { get; set; }
        public List<MeEnvioDTO> ListaPeriodo { get; set; }
        public List<SiEmpresaDTO> ListaEmpresasCumplimiento { get; set; }
        public System.Data.IDataReader ListaReporteCumplimiento { get; set; }
        public List<MeMedicion96DTO> ListaReporteInformacion15min { get; set; }
        public List<MeMedicion48DTO> ListaReporteInformacion30min { get; set; }
        public List<DemandadiaDTO> ListaDemandadia { get; set; }
        public string tipoEmpresa { get; set; }

        public string registros { get; set; }

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        /// <summary>
        /// Lista de Periodos Sicli.
        /// </summary>
        public List<IioPeriodoSicliDTO> ListaPeriodoSicli { get; set; }

        public int MesesReporteCumplimiento { get; set; }
        public DateTime FechaInicioReporteCumplimiento { get; set; }

        public DateTime FechaFinReporteCumplimiento { get; set; }
    }
}