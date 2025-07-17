using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models
{
    public class NrProcesoModel
    {
        public NrProcesoDTO NrProceso { get; set; }
        public List<NrPeriodoDTO> ListaNrPeriodo { get; set; }
        public List<PrGrupoDTO> ListaPrGrupo { get; set; }
        public List<NrConceptoDTO> ListaNrConcepto { get; set; }
        public int NrprcCodi { get; set; }
        public int NrperCodi { get; set; }
        public int GrupoCodi { get; set; }
        public int NrcptCodi { get; set; }
        public string NrprcFechaInicio { get; set; }
        public string NrprcFechaFin { get; set; }
        public decimal? NrprcHoraUnidad { get; set; }
        public decimal? NrprcHoraCentral { get; set; }
        public decimal? NrprcPotenciaLimite { get; set; }
        public decimal? NrprcPotenciaRestringida { get; set; }
        public decimal? NrprcPotenciaAdjudicada { get; set; }
        public decimal? NrprcPotenciaEfectiva { get; set; }
        public decimal? NrprcPotenciaPromMedidor { get; set; }
        public decimal? NrprcPrctjRestringEfect { get; set; }
        public decimal? NrprcVolumenCombustible { get; set; }
        public decimal? NrprcRendimientoUnidad { get; set; }
        public decimal? NrprcEde { get; set; }
        public int? NrprcPadre { get; set; }
        public string NrprcExceptuaCoes { get; set; }
        public string NrprcExceptuaOsinergmin { get; set; }
        public string NrprcTipoIngreso { get; set; }
        public string NrprcHoraFalla { get; set; }
        public decimal? NrprcSobrecosto { get; set; }
        public string NrprcObservacion { get; set; }
        public string NrprcNota { get; set; }
        public string NrprcNotaAutomatica { get; set; }
        public string NrprcFiltrado { get; set; }
        public decimal? NrprcRpf { get; set; }
        public decimal? NrprcTolerancia { get; set; }
        public string NrprcUsuCreacion { get; set; }
        public string NrprcFecCreacion { get; set; }
        public string NrprcUsuModificacion { get; set; }
        public string NrprcFecModificacion { get; set; }
        public string NrperMes { get; set; }
        public string GrupoNomb { get; set; }
        public string NrcptAbrev { get; set; }
        public int Accion { get; set; }
    }

    public class BusquedaNrProcesoModel
    {
        public List<NrProcesoDTO> ListaNrProceso { get; set; }
        public List<NrPeriodoDTO> ListaNrPeriodo { get; set; }
        public List<PrGrupoDTO> ListaPrGrupo { get; set; }
        public List<NrConceptoDTO> ListaNrConcepto { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int? Concepto { get; set; }
        public int? Grupo { get; set; }
        public string Estado { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
    }
}
