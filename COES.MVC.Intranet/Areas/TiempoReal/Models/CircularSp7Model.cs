using COES.Dominio.DTO.Scada;
using COES.Framework.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TiempoReal.Models
{
    public class TrCircularSp7Model
    {
        public DatosSP7DTO TrCircularSp7 { get; set; }
        public List<TrZonaSp7DTO> ListaTrZonaSp7 { get; set; }
        public int Canalcodi { get; set; }
        public string Fecha { get; set; }
        public string FechaSistema { get; set; }
        public string Path { get; set; }
        public decimal Valor { get; set; }
        public int Calidad { get; set; }      
        public string Nombre { get; set; }
        public int Accion { get; set; }
    }

    public class BusquedaTrCircularSp7Model
    {
        public List<DatosSP7DTO> ListaTrCircularSp7 { get; set; }
        public List<DatosSP7DTO> ListaTrCircularSp7Grafica { get; set; }
        public List<TrZonaSp7DTO> ListaTrZonaSp7 { get; set; }
        public List<TrCanalSp7DTO> ListaTrCanalSp7 { get; set; }
        public List<TrCalidadSp7DTO> ListaTrCalidadSp7 { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string Fecha { get; set; }
        public string HoraIni { get; set; }
        public string HoraFin { get; set; }
        public GraficoWeb Grafico { get; set; }
        public string SheetName { get; set; }
    }

}
