using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.PotenciaFirme.Models
{
    public class PotenciaFirmeModel
    {
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }

        public int? Version { get; set; }

        public bool TienePermiso { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }
        public bool TienePermisoGuardar { get; set; }

        public bool UsarLayoutModulo { get; set; }
        public bool TieneRegistroPrevio { get; set; }

        public List<GenericoDTO> ListaAnio { get; set; }
        public List<GenericoDTO> ListaMes { get; set; }
        public int AnioActual { get; set; }
        public int MesActual { get; set; }
        public int? NumVersion { get; set; }
        public int NRegistros { get; internal set; }

        public List<PfPotenciaGarantizadaDTO> ListaPotenciaGarantizada { get; set; }
        public List<PfPotenciaAdicionalDTO> ListaPotenciaAdicional { get; set; }
        public List<PfFactoresDTO> ListaFactorIndisponibilidad { get; set; }
        public List<PfFactoresDTO> ListaFactorPresencia { get; set; }
        public List<PfContratosDTO> ListaContratosCV { get; set; }
        public List<PfReporteTotalDTO> ListaPotenciaFirme { get; set; }
        public List<PfEscenarioDTO> ListaEscenario { get; set; }


        //listados
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqEquipoDTO> ListaCentrales { get; set; }
        public List<EqEquipoDTO> ListaUnidades { get; set; }

        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public List<GenericoDTO> ListaHoraIni { get; set; }
        public List<GenericoDTO> ListaHoraFin { get; set; }

        public int IdPeriodo { get; set; }
        public int IdReporte { get; set; }
        public int IdRecalculo { get; set; }
        public PfPeriodoDTO PfPeriodo { get; set; }
        public PfRecalculoDTO PfRecalculo { get; set; }
        public List<PfPeriodoDTO> ListaPeriodo { get; set; }
        public List<PfRecalculoDTO> ListaRecalculo { get; set; }
        public PfReporteDTO PfReporte { get; set; }
        public List<IndRecalculoDTO> ListaIndRecalculo { get; set; }
        public string FechaIniRER { get; set; }
        public string FechaFinRER { get; set; }

        public PfVersionDTO RevisionPG { get; set; }
        public PfVersionDTO RevisionPA { get; set; }
        public PfVersionDTO RevisionFI { get; set; }
        public PfVersionDTO RevisionFP { get; set; }
        public PfVersionDTO RevisionCCV { get; set; }

        public List<EmpresaGeneral> ListaEmpresasTotales { get; set; }
        public string TipoRecalculo { get; internal set; }
        public bool TieneReportePF { get; set; }
        public string PeriodoActual { get; set; }
    }

    public class EmpresaGeneral
    {
        public string Emprnomb { get; set; }
        public int? Emprcodi { get; set; }
    }

}