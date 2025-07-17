using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models
{
    public class NrPeriodoResumenModel
    {
        public NrPeriodoResumenDTO NrPeriodoResumen { get; set; }
        public List<NrPeriodoDTO> ListaNrPeriodo { get; set; }
        public List<NrConceptoDTO> ListaNrConcepto { get; set; }
        public List<NrSubmoduloDTO> ListaNrSubmodulo { get; set; }
        public List<NrPeriodoResumenDTO> ListaNrPeriodoResumen { get; set; }        

        public int NrperrCodi { get; set; }
        public int NrperCodi { get; set; }
        public int NrcptCodi { get; set; }
        public int? NrperrNumObservacion { get; set; }
        public string NrperrObservacion { get; set; }
        public string NrperrEliminado { get; set; }
        public string NrperrUsuCreacion { get; set; }
        public string NrperrFecCreacion { get; set; }
        public string NrperrUsuModificacion { get; set; }
        public string NrperrFecModificacion { get; set; }
        public string NrperMes { get; set; }
        public string NrcptAbrev { get; set; }
        public int Accion { get; set; }

        public int NrsmodCodi { get; set; }
    }

    public class BusquedaNrPeriodoResumenModel
    {
        public List<NrPeriodoResumenDTO> ListaNrPeriodoResumen { get; set; }
        public List<NrPeriodoDTO> ListaNrPeriodo { get; set; }
        public List<NrConceptoDTO> ListaNrConcepto { get; set; }
        public List<NrSubmoduloDTO> ListaNrSubmodulo { get; set; }

        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int? SubModulo { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
    }
}
