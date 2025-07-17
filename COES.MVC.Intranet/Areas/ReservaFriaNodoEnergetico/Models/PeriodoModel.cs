using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models
{
    public class NrPeriodoModel
    {
        public NrPeriodoDTO NrPeriodo { get; set; }
        public int NrperCodi { get; set; }
        public string NrperMes { get; set; }
        public string NrperEliminado { get; set; }
        public string NrperUsuCreacion { get; set; }
        public string NrperFecCreacion { get; set; }
        public string NrperUsuModificacion { get; set; }
        public string NrperFecModificacion { get; set; }
        public int Accion { get; set; }
    }

    public class BusquedaNrPeriodoModel
    {
        public List<NrPeriodoDTO> ListaNrPeriodo { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
    }
}
