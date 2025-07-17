using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class CmOperacionregistroModel
    {
        public CmOperacionregistroDTO CmOperacionregistro { get; set; }
        public List<CmOperacionregistroDTO> ListaOperacionRegistro { get; set; }
        public List<PrGrupoDTO> ListaPrGrupo { get; set; }
        public List<EveSubcausaeventoDTO> ListaEveSubcausaevento { get; set; }
        public int OperegCodi { get; set; }
        public int Grupocodi { get; set; }
        public int SubcausaCodi { get; set; }
        public string OperegFecinicio { get; set; }
        public string OperegFecfin { get; set; }
        public string OperegUsucreacion { get; set; }
        public string OperegFeccreacion { get; set; }
        public string OperegUsumodificacion { get; set; }
        public string OperegFecmodificacion { get; set; }
        public string Gruponomb { get; set; }
        public string SubcausaDesc { get; set; }
        public int Accion { get; set; }

        public string[][] Datos { get; set; }
    }

    public class BusquedaCmOperacionregistroModel
    {
        public List<CmOperacionregistroDTO> ListaCmOperacionregistro { get; set; }
        public List<PrGrupoDTO> ListaPrGrupo { get; set; }
        public List<EveSubcausaeventoDTO> ListaEveSubcausaevento { get; set; }
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
