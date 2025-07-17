using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{

    public class EvePAleatoriasModel
    {
        public List<SiPersonaDTO> ListaProgramador { get; set; }
        public EvePaleatoriaDTO PAleatoria { get; set; }
        public int Accion { get; set; }
        public string Pafecha { get; set; }
        public string Sic2hop { get; set; }
        public string Hop2ut30d { get; set; }
        public string Ut30d2sort { get; set; }
        public string Sort2prue { get; set; }
        public string Prueno2pa { get; set; }
        public string Pa2fin { get; set; }
        public string Pruesi2gprue { get; set; }
        public string Gprueno2nprue { get; set; }
        public string Nprue2fin { get; set; }
        public string Gpruesi2uprue { get; set; }
        public string Uprue2rprue { get; set; }
        public string Rprue2oa { get; set; }
        public string Oa2priarr { get; set; }
        public string Priarrsi2exit { get; set; }
        public string Priarrno2rearr { get; set; }
        public string Rearrno2noexit { get; set; }
        public string Rearrsi2segarr { get; set; }
        public string Segarrno2noexit { get; set; }
        public string Segarrsi2exit { get; set; }
        public string Exitno2fallunid { get; set; }
        public string Fallunidsi2noexit { get; set; }
        public string Exitsi2resprue { get; set; }
        public string Fallunidno2pabort { get; set; }
        public string Pabort2resprue { get; set; }
        public string Resprue2fin { get; set; }
        public string Noexit2resreslt { get; set; }
        public string Resreslt2fin { get; set; }
        public string Final { get; set; }
        public string Lastuser { get; set; }
        public string Lastdate { get; set; }
        public string Programador { get; set; }        
        public string Paobservacion { get; set; }
        public string Paverifdatosing { get; set; }
        
    }

    public class BusquedaPAleatoriasModel
    {
        public List<EvePaleatoriaDTO> ListaPAleatoria { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
        public int isBotonNuevoHabilitado { get; internal set; }
    }

}