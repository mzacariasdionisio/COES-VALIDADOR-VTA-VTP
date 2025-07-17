using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models
{
    public class NrPotenciaconsignaDetalleModel
    {
        public NrPotenciaconsignaDetalleDTO NrPotenciaconsignaDetalle { get; set; }
        public List<NrPotenciaconsignaDTO> ListaNrPotenciaconsigna { get; set; }
        public int NrpcdCodi { get; set; }
        public int NrpcCodi { get; set; }
        public string NrpcdFecha { get; set; }
        public decimal? NrpcdMw { get; set; }
        public string NrpcdMaximaGeneracion { get; set; }
        public string NrpcdUsuCreacion { get; set; }
        public string NrpcdFecCreacion { get; set; }
        public string NrpcdUsuModificacion { get; set; }
        public string NrpcdFecModificacion { get; set; }
        public string NrpcEliminado { get; set; }
        public int Accion { get; set; }

        public string[][] Datos { get; set; }
        public int[] IndicesTitulo { get; set; }
        public int[] IndicesSubtitulo { get; set; }
        public List<ValidacionListaCelda> Validaciones { get; set; }
        public int Indicador { get; set; }
        public int[] IndicesAgrupacion { get; set; }
        public int[] IndicesComentario { get; set; }
    }

    public class BusquedaNrPotenciaconsignaDetalleModel
    {
        public List<NrPotenciaconsignaDetalleDTO> ListaNrPotenciaconsignaDetalle { get; set; }
        public List<NrPotenciaconsignaDTO> ListaNrPotenciaconsigna { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
    }

}
