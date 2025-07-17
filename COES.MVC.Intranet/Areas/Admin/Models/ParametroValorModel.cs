using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class SiParametroValorModel
    {
        public SiParametroValorDTO SiParametroValor { get; set; }
        public List<SiParametroDTO> ListaSiParametro { get; set; }
        public int SiparvCodi { get; set; }
        public int SiparCodi { get; set; }
        public string SiparvFechaInicial { get; set; }
        public string SiparvFechaFinal { get; set; }
        public decimal? SiparvValor { get; set; }
        public string SiparvNnota { get; set; }
        public string SiparvEliminado { get; set; }
        public string SiparvUsuCreacion { get; set; }
        public string SiparvFecCreacion { get; set; }
        public string SiparvUsuModificacion { get; set; }
        public string SiparvFecModificacion { get; set; }
        public string SiparAbrev { get; set; }
        public int Accion { get; set; }
        public int IdParametroAGC { get; set; }
    }

    public class BusquedaSiParametroValorModel
    {
        public List<SiParametroValorDTO> ListaSiParametroValor { get; set; }
        public List<SiParametroDTO> ListaSiParametro { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }

        public int IdParametroAGC { get; set; }
    }
}
