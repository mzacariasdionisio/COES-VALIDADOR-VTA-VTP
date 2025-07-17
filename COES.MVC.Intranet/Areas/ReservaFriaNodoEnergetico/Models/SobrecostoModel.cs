using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models
{
    public class NrSobrecostoModel
    {
        public NrSobrecostoDTO NrSobrecosto { get; set; }
        public List<PrGrupoDTO> ListaPrGrupo { get; set; }
        public int NrscCodi { get; set; }
        //public int GrupoCodi { get; set; }
        public string NrscFecha { get; set; }
        public decimal? NrscCodespacho0 { get; set; }
        public decimal? NrscCodespacho1 { get; set; }
        public decimal? NrscSobrecosto { get; set; }
        public string NrscNota { get; set; }        
        public string NrscEliminado { get; set; }
        public int? NrscPadre { get; set; }
        public string NrscUsucreacion { get; set; }
        public string NrscFecCreacion { get; set; }
        public string NrscUsuModificacion { get; set; }
        public string NrscFecModificacion { get; set; }
        public string GrupoNomb { get; set; }
        public int Accion { get; set; }
    }

    public class BusquedaNrSobrecostoModel
    {
        public List<NrSobrecostoDTO> ListaNrSobrecosto { get; set; }
        public List<PrGrupoDTO> ListaPrGrupo { get; set; }
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
