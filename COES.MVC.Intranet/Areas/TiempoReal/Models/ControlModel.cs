using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TiempoReal.Models
{
    public class AgcControlModel
    {
        public AgcControlDTO AgcControl { get; set; }
        public List<MePtomedicionDTO> ListaMePtomedicion { get; set; }
        public List<EqEquipoDTO> ListaEqEquipo { get; set; }
        public List<AgcControlPuntoDTO> ListaControlPunto { get; set; }
        public int AgccCodi { get; set; }
        public string AgccTipo { get; set; }
        public string AgccDescrip { get; set; }
        public int PtomediCodi { get; set; }
        public string AgccB2 { get; set; }
        public string AgccB3 { get; set; }
        public string AgccValido { get; set; }
        public string AgccUsuCreacion { get; set; }
        public string AgccFecCreacion { get; set; }
        public string AgccUsuModificacion { get; set; }
        public string AgccFecModificacion { get; set; }
        public string PtomediBarraNomb { get; set; }
        public int Accion { get; set; }
    }

    public class BusquedaAgcControlModel
    {
        public List<AgcControlDTO> ListaAgcControl { get; set; }     
        public List<MePtomedicionDTO> ListaMePtomedicion { get; set; }
        public List<EqEquipoDTO> ListaEqEquipoPropiedad { get; set; }
        public List<PrRepcvDTO> ListaCostoVariable { get; set; }        
        public string FechaGeneral { get; set; }
        public string FechaCVIni { get; set; }
        public string FechaCVFin { get; set; }
        public string DespExpOpcReprograma { get; set; }
        public string VaguaOpcReprograma { get; set; }
        public string PotenciaOpcReprograma { get; set; }     
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        [AllowHtml]
        public string Contenido { get; set; }        
        public string Archivo { get; set; }
        public string Control { get; set; }
    }       

    public class MePtomedicionModel
    {
        public MePtomedicionDTO MePtomedicion { get; set; }
        public PrGrupoDTO PrGrupo { get; set; }
        public List<PrGrupoDTO> ListaPrGrupo { get; set; }
        public List<MePtomedicionDTO> ListaMePtomedicion { get; set; }
        public int PtomediCodi { get; set; }
        public int OriglectCodi { get; set; }
        public string PtomediBarraNomb { get; set; }
        public string PtomediEleNomb { get; set; }
        public int? Orden { get; set; }
        public string PtomediDesc { get; set; }
        public int? Codref { get; set; }
        public int? EquiCodi { get; set; }
        public string OsiCodi { get; set; }
        public int? TipoInfocodi { get; set; }
        public int? GrupoCodi { get; set; }
        public int? EmprCodi { get; set; }
        public string LastUser { get; set; }
        public string LastDate { get; set; }
        public int? TptomediCodi { get; set; }
        public string PtomediEstado { get; set; }
        public string OrigLectNombre { get; set; }
        public int Accion { get; set; }
    }

    public class PotenciaMW
    {
        public double PotenciaMinima;
        public double PotenciaEfectiva;
        public double[] MW = new double[48];
        public double[] PAsignada = new double[48]; //si esta en mantenimiento -1
        public double[] Delta = new double[48];
        public string B2 = "";
        public string B3 = "";

        public PotenciaMW()
        {

        }
    }
}
