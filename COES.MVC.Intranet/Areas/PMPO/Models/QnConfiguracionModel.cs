using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.PMPO.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    public class QnConfiguracionModel
    {
        #region Campos Comunes
        public bool TienePermisoNuevo { get; set; }
        public bool UsarLayoutModulo { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        
        public string Detalle { get; set; }
        #endregion

        public bool EsNuevo { get; set; }
        public PmoEstacionhDTO EstacionH { get; internal set; }
        public List<PuntosSDDP> ListaPtosMedicion { get; set; }
        public List<PuntosSDDP> ListaPtosXEstacion { get; set; }
        //public List<MePtomedicionDTO> ListaSddp { get; internal set; }
        public List<PmoSddpCodigoDTO> ListaSddp { get; internal set; }
        public string HtmlListadoEstacionesH { get; set; }
        public string Descripcion { get; set; }
        public int Accion { get; set; }
    }
}