using COES.Dominio.DTO.Scada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TiempoReal.Models
{
    public class MeScadaFiltroSp7Model
    {
        public MeScadaFiltroSp7DTO MeScadaFiltroSp7 { get; set; }
        public List<TrZonaSp7DTO> ListaTrZonaSp7 { get; set; }
        public List<TrCanalSp7DTO> ListaTrCanalSp7 { get; set; }

        public int FiltroCodi { get; set; }
        public string FiltroNomb { get; set; }
        public string FiltroUser { get; set; }
        public string ScdFiFecCreacion { get; set; }
        public string ScdFiUsuModificacion { get; set; }
        public string ScdFiFecModificacion { get; set; }
        public int Accion { get; set; }
        public string FiltroCanal { get; set; } 
    }

    public class BusquedaMeScadaFiltroSp7Model
    {
        public List<MeScadaFiltroSp7DTO> ListaMeScadaFiltroSp7 { get; set; }
        public List<TrZonaSp7DTO> ListaTrZonaSp7 { get; set; }
        public List<TrCanalSp7DTO> ListaTrCanalSp7 { get; set; }

        public string FechaIni { get; set; }
        public string FechaFin { get; set; }

        public string FiltroNombre { get; set; }
        public string FiltroCreador { get; set; }
        public string FiltroModificador { get; set; }


        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
    }

}
