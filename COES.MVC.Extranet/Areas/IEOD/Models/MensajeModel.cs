using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.IEOD.Models
{
    public class MensajeModel
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
        public string FormatCodi { get; set; }
        public string MsgUsuModificacion { get; set; }
        public DateTime? MsgFecModificacion { get; set; }
        public string MsgUsuCreacion { get; set; }

        public string MsgTo { get; set; }
        public string MsgFrom { get; set; }
        public string MsgFromName { get; set; }
        public string MsgAsunto { get; set; }
        public int MsgFlagAdj { get; set; }

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
    }
}