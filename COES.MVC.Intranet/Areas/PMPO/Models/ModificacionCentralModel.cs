using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.PMPO.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    public class ModificacionCentralModel
    {
        #region Campos Comunes
        public bool TienePermisoNuevo { get; set; }
        public bool UsarLayoutModulo { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        #endregion

        public int Topcodi { get; set; }
        public int Action { get; set; }
        public bool EsNuevo { get; set; }
        public string FechaFutura { get; set; }
        public MpTopologiaDTO ModificacionCentral { get; set; }
        public List<MpRecursoDTO> ListaSddp { get; set; }
        public string HtmlListadoModificacionCentral { get; set; }
        public int Accion { get; set; }

        //codigo de las propiedades de modificación
        public int propcodiPotencia { get; set; }
        public int propcodiCoefProd { get; set; }
        public int propcodiCaudalMinTur { get; set; }
        public int propcodiCaudalMaxTur { get; set; }
        public int propcodiDefluenciaTotMin { get; set; }
        public int propcodiICP { get; set; }
        public int propcodiIH { get; set; }
        public int propcodiVolumenMax { get; set; }
        public int propcodiIndicadorEA { get; set; }
    }
}