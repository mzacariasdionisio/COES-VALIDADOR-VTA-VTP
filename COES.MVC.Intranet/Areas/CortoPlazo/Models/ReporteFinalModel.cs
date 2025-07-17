using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.CortoPlazo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class ReporteFinalModel
    {
        public string FechaConsulta { get; set; }
        public List<BarraDTO> ListaBarrasTransf { get; set; }
        public List<BarraDTO> ListaBarrasDesconocida { get; set; }
        public List<CmConfigbarraDTO> ListaBarrasEMS { get; set; }
        public string FechaVigencia { get; set; }
        public string FechaExpiracion { get; set; }
        public CmBarraRelacionDTO Entidad { get; set; }
        public string TipoRegistro { get; set; }
        public string TipoRelacion { get; set; }
        public int BarraTransferencia { get; set; }
        public int BarraDesconocida { get; set; }
        public int BarraTransferencia2 { get; set; }
        public int BarraEMS { get; set; }
        public int BarraEMS2 { get; set; }
        public int CodigoBarra { get; set; }
        public int IdBarraEMS { get; set; }
        public List<CmBarraRelacionDTO> ListadoHistorico { get; set; }
        public CmPeriodoDTO EntidadPeriodo { get; set; }
        public int CodigoPeriodo { get; set; }
        public string PeriodoBase { get; set; }
        public string PeriodoMedia { get; set; }
        public string PeriodoPunta { get; set; }
        public string IdsBarrasEMSAdicional { get; set; }
        public List<CmPeriodoDTO> ListaHistoricoPeriodo { get; set; }

        public List<CmUmbralreporteDTO> ListaHistoricoUmbral { get; set; }

        public CmUmbralreporteDTO EntidadUmbral { get; set; }
        public int CodigoUmbral { get; set; }
        public decimal TotalMaximo { get; set; }
        public decimal TotalMinimo { get; set; }
        public decimal EnergiaMaximo { get; set; }
        public decimal EnergiaMinimo { get; set; }
        public decimal CongestionMaximo { get; set; }
        public decimal CongestionMinimo { get; set; }
        public decimal UmbralDiferencia { get; set; }

        public List<CmEquipobarraDTO> ListHistoricoEquipo { get; set; }
        public CmEquipobarraDTO EntidadEquipo { get; set; }
        public int CodigoEquipo { get; set; }
        public List<EqCongestionConfigDTO> ListaEquiposCongestion { get; set; } 

        public List<CmBarraRelacionDTO> ListaBarraAdicional { get; set; }
        public string IdsBarras { get; set; }
        public int Equipo { get; set; }

        public List<CmBarraRelacionDetDTO> ListaBarraEMSAdicionales { get; set; }

        #region Ticket_6245
        public string MostrarReporte { get; set; }
        #endregion
    }

    public class FactorPerdidaModel
    {
        public string[][] Datos { get; set; }
        public string FechaConsulta { get; set; }

    }

    public class VersionReporteModel
    { 
        public string FechaInicio { get; set; }
        public string Fecha { get; set; }
        public string FechaFin { get; set; }
        public List<CmReporteDTO> ListaReporte { get; set; }
        public List<ReporteBarraDesenergizada> ListaReporteBarra { get; set; }
    }
}