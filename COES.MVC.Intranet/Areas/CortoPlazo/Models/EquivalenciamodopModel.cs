using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class CmEquivalenciamodopModel
    {        
        public CmEquivalenciamodopDTO CmEquivalenciamodop { get; set; }
        public List<PrGrupoDTO> ListaPrGrupo { get; set; }
        public int EquimoCodi { get; set; }
        public int Grupocodi { get; set; }
        public string EquimoNombrencp { get; set; }
        public string EquimoUsucreacion { get; set; }
        public string EquimoFeccreacion { get; set; }
        public string EquimoUsumodificacion { get; set; }
        public string EquimoFecmodificacion { get; set; }
        public string Gruponomb { get; set; }
        public int Accion { get; set; }
        public string[][] Datos { get; set; }
    }

    public class BusquedaCmEquivalenciamodopModel
    {
        public List<CmEquivalenciamodopDTO> ListaCmEquivalenciamodop { get; set; }
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
