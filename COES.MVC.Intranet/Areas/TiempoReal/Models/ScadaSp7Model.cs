using COES.Dominio.DTO.Scada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TiempoReal.Models
{
    public class MeScadaSp7Model
    {
        public MeScadaSp7DTO MeScadaSp7 { get; set; }
        public List<TrZonaSp7DTO> ListaTrZonaSp7 { get; set; }
        public List<MeScadaFiltroSp7DTO> ListaMeScadaFiltroSp7 { get; set; }
        public int CanalCodi { get; set; }
        public string MediFecha { get; set; }
        public decimal? MediTotal { get; set; }
        public string MediEstado { get; set; }
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
        public decimal? H60 { get; set; }
        public decimal? H61 { get; set; }
        public decimal? H62 { get; set; }
        public decimal? H63 { get; set; }
        public decimal? H64 { get; set; }
        public decimal? H65 { get; set; }
        public decimal? H66 { get; set; }
        public decimal? H67 { get; set; }
        public decimal? H68 { get; set; }
        public decimal? H69 { get; set; }
        public decimal? H70 { get; set; }
        public decimal? H71 { get; set; }
        public decimal? H72 { get; set; }
        public decimal? H73 { get; set; }
        public decimal? H74 { get; set; }
        public decimal? H75 { get; set; }
        public decimal? H76 { get; set; }
        public decimal? H77 { get; set; }
        public decimal? H78 { get; set; }
        public decimal? H79 { get; set; }
        public decimal? H80 { get; set; }
        public decimal? H81 { get; set; }
        public decimal? H82 { get; set; }
        public decimal? H83 { get; set; }
        public decimal? H84 { get; set; }
        public decimal? H85 { get; set; }
        public decimal? H86 { get; set; }
        public decimal? H87 { get; set; }
        public decimal? H88 { get; set; }
        public decimal? H89 { get; set; }
        public decimal? H90 { get; set; }
        public decimal? H91 { get; set; }
        public decimal? H92 { get; set; }
        public decimal? H93 { get; set; }
        public decimal? H94 { get; set; }
        public decimal? H95 { get; set; }
        public decimal? H96 { get; set; }
        public string MediCambio { get; set; }
        public string MedScdUsuCreacion { get; set; }
        public string MedScdFecCreacion { get; set; }
        public string MedScdUsuModificacion { get; set; }
        public string MedScdFecModificacion { get; set; }
        public string CanalNomb { get; set; }
        public int Accion { get; set; }
    }

    public class BusquedaMeScadaSp7Model
    {
        public List<MeScadaSp7DTO> ListaMeScadaSp7 { get; set; }
        public List<TrZonaSp7DTO> ListaTrZonaSp7 { get; set; }
        public List<MeScadaFiltroSp7DTO> ListaMeScadaFiltroSp7 { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
    }

}
