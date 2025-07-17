using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    public class EscenarioSDDPModel
    {
        #region Campos Comunes
        public bool TienePermisoNuevo { get; set; }
        public bool UsarLayoutModulo { get; set; }
        public string Resultado { get; set; }        
        public string Mensaje { get; set; }

        public string Detalle { get; set; }
        #endregion

       
        public int CodigoEscenario { get; set; }
        public string Periodo { get; set; }
        public string Resolucion { get; set; }
        public string Mes { get; set; }
        public string MesAnioSemanal { get; set; }
        public string MesAnioMensual { get; set; }
        
        public string HtmlListadoEscenariosSddp { get; set; }
        
        public int Accion { get; set; }
        public int VersionMostrada { get; set; }
        
        public string StrListaCentralesHidro { get; set; }
    }
}