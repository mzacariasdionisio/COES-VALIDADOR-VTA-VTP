using COES.Servicios.Aplicacion.FormatoMedicion;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{    
    public class QnMensualModel
    {
        #region Campos Comunes
        public bool TienePermisoNuevo { get; set; }
        public bool UsarLayoutModulo { get; set; }
        public string Resultado { get; set; }
        public string Resultado2 { get; set; }
        public string Mensaje { get; set; }

        public string Detalle { get; set; }
        #endregion

        //public bool EsNuevo { get; set; }
        public string Anio { get; set; }
        public string Mes { get; set; }

        public string MesAnioSemanal { get; set; }
        public string MesAnioMensual { get; set; }
        public string RangoIni { get; set; }
        public string RangoFin { get; set; }
        public string HtmlListadoSeriesHidrologica { get; set; }
        public int CodigoEnvio { get; set; }
        //public int NumEstaciones { get; set; }
        public HandsonModel DataHandsonSeriesHidrologica { get; set; }
        public string NotaVersion { get; set; }
        public int NumFilas { get; set; }
        public int CodigoInfoBase { get; set; }
        public List<string> LstCabeceras { get; set; }
        public string NotaNuevoOficial { get; set; }

        //log de notificacion
        public string NameFileLog { get; set; }
        public int Accion { get; set; }
    }
}