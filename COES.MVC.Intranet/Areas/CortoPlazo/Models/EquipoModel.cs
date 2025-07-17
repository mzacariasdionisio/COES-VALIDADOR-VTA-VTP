using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class EqEquipoModel
    {

        public EqEquipoDTO EqEquipo { get; set; }
        public List<SiEmpresaDTO> ListaSiEmpresa { get; set; }
        public List<EqAreaDTO> ListaEqArea { get; set; }
        public List<EqFamiliaDTO> ListaEqFamilia { get; set; }
        public int EquiCodi { get; set; }
        public int EmprCodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Elecodi { get; set; }
        public int Areacodi { get; set; }
        public int FamCodi { get; set; }
        public string EquiAbrev { get; set; }
        public string EquiNomb { get; set; }
        public string EquiAbrev2 { get; set; }
        public decimal? EquiTension { get; set; }
        public int? EquiPadre { get; set; }
        public decimal? EquiPot { get; set; }
        public string Lastuser { get; set; }
        public string Lastdate { get; set; }
        public string Ecodigo { get; set; }
        public string EquiEstado { get; set; }
        public string Osigrupocodi { get; set; }
        public int? Lastcodi { get; set; }
        public string EquiFechiniopcom { get; set; }
        public string EquiFechfinopcom { get; set; }
        public string EquiManiobra { get; set; }
        public string Usuarioupdate { get; set; }
        public string Fechaupdate { get; set; }
        public string EquiMantrelev { get; set; }
        public string Osinergcodi { get; set; }
        public string EmprNomb { get; set; }
        public string Areaabrev { get; set; }
        public string FamAbrev { get; set; }
        public int Accion { get; set; }
    }

    public class ValorPropiedadModel
    {
        public int Equicodi { get; set; }
        public int Propcodi { get; set; }
        public string PropNomb { get; set; }
        public string Valor { get; set; }
        public string Fecha { get; set; }  
      
        public int Accion { get; set; }
    }

    public class BusquedaEqEquipoModel
    {
        public List<EqEquipoDTO> ListaEqEquipo { get; set; }
        public List<SiEmpresaDTO> ListaSiEmpresa { get; set; }
        public List<EqAreaDTO> ListaEqArea { get; set; }
        public List<EqFamiliaDTO> ListaEqFamilia { get; set; }
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
