using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models
{
    public class MeMedicion48Model
    {
        public MeMedicion48DTO MeMedicion48 { get; set; }
        public List<MeLecturaDTO> ListaMeLectura { get; set; }
        public List<SiTipoinformacionDTO> ListaSiTipoinformacion { get; set; }
        public List<MePtomedicionDTO> ListaMePtomedicion { get; set; }
        public int Lectcodi { get; set; }
        public DateTime Medifecha { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Ptomedicodi { get; set; }
        public decimal? H1 { get; set; }
        public decimal? Meditotal { get; set; }
        public string Mediestado { get; set; }
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
        public string Lastuser { get; set; }
        public string Lastdate { get; set; }
        public string LectAbrev { get; set; }
        public string TipoinfoAbrev { get; set; }
        public string PtomediDesc { get; set; }
        public int Accion { get; set; }

        public string[][] Datos { get; set; }
        public int[] IndicesTitulo { get; set; }
        public int[] IndicesSubtitulo { get; set; }
        public List<ValidacionListaCelda> Validaciones { get; set; }
        public int Indicador { get; set; }
        public int[] IndicesAgrupacion { get; set; }
        public int[] IndicesComentario { get; set; }
    }


    public class BusquedaMeMedicion48Model
    {
        public List<MeMedicion48DTO> ListaMeMedicion48 { get; set; }
        public List<MeLecturaDTO> ListaMeLectura { get; set; }
        public List<SiTipoinformacionDTO> ListaSiTipoinformacion { get; set; }
        public List<MePtomedicionDTO> ListaMePtomedicion { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int? Lectura { get; set; }
        public int? GrupoCentral { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }        
    }
}
