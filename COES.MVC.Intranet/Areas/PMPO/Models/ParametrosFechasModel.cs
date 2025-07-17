using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    
    public class ParametrosFechasModel
    {
        #region Campos Comunes
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }
        public bool UsarLayoutModulo { get; set; }
        public string Resultado { get; set; }        
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        #endregion

        public string Fecha { get; set; }
        public string Anio { get; set; }
        public string FechaIniAnio { get; set; }
        public string FechaFinAnio { get; set; }
        public string DiaNombre { get; set; }
        public bool EsNuevo { get; set; }
        public bool EsProcesado { get; set; }

        public int Accion { get; set; }

        public string FechaIniRango { get; set; }
        public string FechaFinRango { get; set; }

        public List<PmoMesDTO> ListaSemanaMes { get; internal set; }
        public List<PmoFeriadoDTO> ListaFeriados { get; internal set; }
        public string HtmlListadoAniosOp { get; set; }
        public int NumVersion { get; set; }
    }
}