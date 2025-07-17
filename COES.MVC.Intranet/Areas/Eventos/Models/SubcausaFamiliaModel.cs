using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class EveSubcausaFamiliaModel
    {
        public EveSubcausaFamiliaDTO EveSubcausaFamilia { get; set; }
        public List<EveSubcausaeventoDTO> ListaEveSubcausaevento { get; set; }
        public List<EqFamiliaDTO> ListaEqFamilia { get; set; }
        public int ScaufaCodi { get; set; }
        public int SubcausaCodi { get; set; }
        public int FamCodi { get; set; }
        public string ScaufaEliminado { get; set; }
        public string ScaufaUsuCreacion { get; set; }
        public string ScaufaFecCreacion { get; set; }
        public string ScaufaUsuModificacion { get; set; }
        public string ScaufaFecModificacion { get; set; }
        public string SubcausaDesc { get; set; }
        public string FamAbrev { get; set; }
        public int Accion { get; set; }
    }

    public class BusquedaEveSubcausaFamiliaModel
    {
        public List<EveSubcausaFamiliaDTO> ListaEveSubcausaFamilia { get; set; }
        public List<EveSubcausaeventoDTO> ListaEveSubcausaevento { get; set; }
        public List<EqFamiliaDTO> ListaEqFamilia { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
        public string Estado { get; set; }
        
    }

}
