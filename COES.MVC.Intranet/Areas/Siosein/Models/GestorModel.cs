using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Siosein.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace COES.MVC.Intranet.Areas.Siosein.Models
{
    public class GestorModel
    {
        public int MsgCodi { get; set; }
        public DateTime? MsgFecha { get; set; }
        public int? FDatCodi { get; set; }
        public int? TMsgCodi { get; set; }
        public int? EstMsgCodi { get; set; }
        public string MsgContenido { get; set; }
        public DateTime? MsgFechaPeriodo { get; set; }
        public int ModCodi { get; set; }
        public int EmprCodi { get; set; }
        public int FormatCodi { get; set; }
        public string MsgUsuModificacion { get; set; }
        public DateTime? MsgFecModificacion { get; set; }
        public string MsgUsuCreacion { get; set; }

        public string MsgTo { get; set; }
        public string MsgFrom { get; set; }
        public string MsgFromName { get; set; }
        public string MsgAsunto { get; set; }
        public int MsgFlagAdj { get; set; }
        public int CarCodi { get; set; }
        public int MsgTipo { get; set; }


        public int Alerta { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }

        public DateTime? FechaAmpliacion { get; set; }

        public List<SiMensajeDTO> ListaMensajes { get; set; }
        public List<SiTipoMensajeDTO> ListaTipoMensaje { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }

        public List<GenericoDTO> ListaEnlaces { get; set; }
        public List<GenericoDTO> ListaFormatos { get; set; }
        public List<GenericoDTO> ListaPendientes { get; set; }
        public List<SiFuentedatosDTO> ListaFuenteDao { get; set; }
        public List<SiLogDTO> ListaLog { get; set; }
        public List<SiBandejamensajeUserDTO> ListaCarpetas { get; set; }

        public string Resultado { get; set; }

        //Informes SGI
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public List<SiVersionDTO> ListaVersion { get; set; }
        public FechasPR5 ObjFecha { get; set; }

        public List<SioTablaprieDTO> ListaTablasPrie { get; set; }

        public List<GenericoMensaje> ListaTempMensajes { get; set; }
        public List<ListaGenerica> CarpetasLista { get; set; }

        public string FechaFiltro { get; set; }

        //
        public ConstantesSiosein.Estado Estado { get; set; }
        //

        public class GenericoMensaje
        {
            public int MsgCodi { get; set; }
            public DateTime? MsgFecha { get; set; }
            public int? FDatCodi { get; set; }
            public int? TMsgCodi { get; set; }
            public int? EstMsgCodi { get; set; }
            public string MsgContenido { get; set; }
            public DateTime? MsgFechaPeriodo { get; set; }
            public int ModCodi { get; set; }
            public int EmprCodi { get; set; }
            public int FormatCodi { get; set; }
            public string MsgUsuModificacion { get; set; }
            public DateTime? MsgFecModificacion { get; set; }
            public string MsgUsuCreacion { get; set; }
            public string MsgTo { get; set; }
            public string MsgFrom { get; set; }
            public string MsgFromName { get; set; }
            public string MsgAsunto { get; set; }
            public int MsgFlagAdj { get; set; }
            public int CarCodi { get; set; }

            //Cambio SIOSEIN
            public string Tmsgnombre { get; set; }
            public string Tmsgcolor { get; set; }
        }


        public class ListaGenerica
        {
            public DateTime? ValorFecha1 { get; set; }
            public DateTime? ValorFecha2 { get; set; }
            public int? Entero1 { get; set; }
            public int? Entero2 { get; set; }
            public string String1 { get; set; }
            public string String2 { get; set; }
            public string String3 { get; set; }

        }
    }
}