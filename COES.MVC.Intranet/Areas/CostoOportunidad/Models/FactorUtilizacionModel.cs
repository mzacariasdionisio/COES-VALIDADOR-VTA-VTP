
namespace COES.MVC.Intranet.Areas.CostoOportunidad.Models
{
    public class FactorUtilizacionModel
    {
        #region Campos Comunes
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }
        public bool UsarLayoutModulo { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string HtmlErrores { get; set; }
        #endregion

        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string FechaProcesoManual { get; set; }
        public bool MostrarBtnRT { get; set; }
    }
}