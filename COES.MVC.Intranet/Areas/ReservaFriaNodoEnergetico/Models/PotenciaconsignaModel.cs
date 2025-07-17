using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models
{
    public class NrPotenciaconsignaModel
    {
        public NrPotenciaconsignaDTO NrPotenciaconsigna { get; set; }
        public List<NrSubmoduloDTO> ListaNrSubmodulo { get; set; }
        public List<PrGrupoDTO> ListaPrGrupo { get; set; }
        public int NrpcCodi { get; set; }
        public int NrsmodCodi { get; set; }
        public int GrupoCodi { get; set; }
        public string NrpcFecha { get; set; }
        public string NrpcEliminado { get; set; }
        public string NrpcUsuCreacion { get; set; }
        public string NrpcFecCreacion { get; set; }
        public string NrpcUsuModificacion { get; set; }
        public string NrpcFecModificacion { get; set; }
        public string NrsmodNombre { get; set; }
        public string GrupoNomb { get; set; }
        public int Accion { get; set; }

        public string[][] Datos { get; set; }
    }

    public class BusquedaNrPotenciaconsignaModel
    {
        public List<NrPotenciaconsignaDTO> ListaNrPotenciaconsigna { get; set; }
        public List<NrSubmoduloDTO> ListaNrSubmodulo { get; set; }
        public List<PrGrupoDTO> ListaPrGrupo { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int? SubModulo { get; set; }
        public int? GrupoPotencia { get; set; }
        public string Estado { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
    }

    public class MergeModel
    {
        public int row { get; set; }
        public int col { get; set; }
        public int rowspan { get; set; }
        public int colspan { get; set; }
    }

    public class ValidacionListaCelda
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public List<string> Elementos { get; set; }
    }

}
