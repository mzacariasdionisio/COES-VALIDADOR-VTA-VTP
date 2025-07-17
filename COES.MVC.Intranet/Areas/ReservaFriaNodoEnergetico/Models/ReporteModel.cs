using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models
{
    public class ReporteModel
    {

    }

    public class BusquedaReporteModel
    {
        public string FechaGeneral { get; set; }
        public List<NrSubmoduloDTO> ListaNrSubmodulo { get; set; }

        public int Submodulo { get; set; }
        public List<MeMedicion96DTO> ListaMeMedicion96 { get; set; }
        public List<NrProcesoDTO> ListaNrProceso { get; set; }
        public List<NrPeriodoDTO> ListaNrPeriodo { get; set; }
        public int NrperCodi { get; set; }
        public string Tag { get; set; }
        public bool Hora { get; set; }

        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public List<MeMedicion96DTO> ListaMeMedicion96Ipf { get; set; }
        public List<MeMedicion96DTO> ListaMeMedicion96Itp { get; set; }
        public List<MeMedicion96DTO> ListaMeMedicion96Itf { get; set; }
        public List<NrProcesoDTO> ListaSitf { get; set; }
        public List<NrProcesoDTO> ListaSipf { get; set; }

        public List<NrProcesoDTO> ListaHda  { get; set; }
        public List<NrProcesoDTO> ListaHmpe  { get; set; }
        public List<NrProcesoDTO> ListaHmce { get; set; }
    }
}