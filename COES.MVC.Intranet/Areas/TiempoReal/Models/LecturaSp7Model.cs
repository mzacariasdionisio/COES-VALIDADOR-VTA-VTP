using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TiempoReal.Models
{
    public class FLecturaSp7Model
    {
        public FLecturaSp7DTO FLecturaSp7 { get; set; }
        public List<MeGpsDTO> ListaMeGps { get; set; }
        public string FechaHora { get; set; }
        public int GpsCodi { get; set; }
        public decimal? Vsf { get; set; }
        public decimal? Maximo { get; set; }
        public decimal? Minimo { get; set; }
        public decimal? Voltaje { get; set; }
        public int? Num { get; set; }
        public decimal? Desv { get; set; }
        public decimal? H0 { get; set; }
        public decimal? H1 { get; set; }
        public decimal? H2 { get; set; }
        public decimal? H3 { get; set; }
        public decimal? H4 { get; set; }
        public decimal? H5 { get; set; }
        public decimal? H6 { get; set; }
        public decimal? H7 { get; set; }
        public decimal? H8 { get; set; }
        public decimal? H9 { get; set; }
        public decimal? H10 { get; set; }
        public decimal? H11 { get; set; }
        public decimal? H12 { get; set; }
        public decimal? H13 { get; set; }
        public decimal? H14 { get; set; }
        public decimal? H15 { get; set; }
        public decimal? H16 { get; set; }
        public decimal? H17 { get; set; }
        public decimal? H18 { get; set; }
        public decimal? H19 { get; set; }
        public decimal? H20 { get; set; }
        public decimal? H21 { get; set; }
        public decimal? H22 { get; set; }
        public decimal? H23 { get; set; }
        public decimal? H24 { get; set; }
        public decimal? H25 { get; set; }
        public decimal? H26 { get; set; }
        public decimal? H27 { get; set; }
        public decimal? H28 { get; set; }
        public decimal? H29 { get; set; }
        public decimal? H30 { get; set; }
        public decimal? H31 { get; set; }
        public decimal? H32 { get; set; }
        public decimal? H33 { get; set; }
        public decimal? H34 { get; set; }
        public decimal? H35 { get; set; }
        public decimal? H36 { get; set; }
        public decimal? H37 { get; set; }
        public decimal? H38 { get; set; }
        public decimal? H39 { get; set; }
        public decimal? H40 { get; set; }
        public decimal? H41 { get; set; }
        public decimal? H42 { get; set; }
        public decimal? H43 { get; set; }
        public decimal? H44 { get; set; }
        public decimal? H45 { get; set; }
        public decimal? H46 { get; set; }
        public decimal? H47 { get; set; }
        public decimal? H48 { get; set; }
        public decimal? H49 { get; set; }
        public decimal? H50 { get; set; }
        public decimal? H51 { get; set; }
        public decimal? H52 { get; set; }
        public decimal? H53 { get; set; }
        public decimal? H54 { get; set; }
        public decimal? H55 { get; set; }
        public decimal? H56 { get; set; }
        public decimal? H57 { get; set; }
        public decimal? H58 { get; set; }
        public decimal? H59 { get; set; }
        public decimal? DevSec { get; set; }
        public string FrecSpUsuCreacion { get; set; }
        public string FrecSpFecCreacion { get; set; }
        public string FrecSpUsuModificacion { get; set; }
        public string FrecSpFecModificacion { get; set; }
        public string Nombre { get; set; }
        public int Accion { get; set; }
    }

    public class BusquedaFLecturaSp7Model
    {
        public List<FLecturaSp7DTO> ListaFLecturaSp7 { get; set; }
        public List<FLecturaSp7DTO> ListaFLecturaSp7Grafica { get; set; }
        public List<MeGpsDTO> ListaMeGps { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public GraficoWeb Grafico { get; set; }
        public string SheetName { get; set; }
    }

}
