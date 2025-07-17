using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.PMPO.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    public class CentralSDDPModel
    {
        #region Campos Comunes
        public bool TienePermisoNuevo { get; set; }
        public bool UsarLayoutModulo { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        #endregion

        public bool EsNuevo { get; set; }       
        public List<EqEquipoDTO> ListaTotalCentralesHidro { get; set; }
        public string StrListaCentralesHidro { get; set; }
        public string StrListaEmbalses { get; set; }
        public List<EqEquipoDTO> ListaTotalEmbalses { get; set; }        
        public List<FormatoPtoMedicion> ListaFormatoPtosSemanal { get; set; }
        public List<FormatoPtoMedicion> ListaFormatoPtosMensual { get; set; }
        public List<CentralHidroelectrica> ListaCentralesHidro { get; set; }
        public List<Embalse> ListaEmbalses { get; set; }

        public List<CoeficienteEvaporacion> ListaEvaporacion { get; internal set; }
        public List<VolumenArea> ListaVolumenArea { get; internal set; }
        public string HtmlListadoCentralesSDDP { get; set; }
        public string HtmlListadoCentralesHidro { get; set; } 
        public string HtmlListadoEmbalse { get; set; }         
        public int Accion { get; set; }
        public int AccionEsc { get; set; }
        public int? TopologiaMostrada { get; set; }
        public bool EsSoloLectura { get; set; }
        public string Titulo { get; set; }
        public string SddpNum { get; set; }
        public CentralSddp CentralSddp { get; set; }
        public List<PmoSddpCodigoDTO> ListaCodigosSddp { get; set; }
        public List<EstacionHidroAsociada> ListaEstacionesHidro { get; set; }
        public List<MpRecursoDTO> ListaCentralesSddp { get; set; }
    }
}