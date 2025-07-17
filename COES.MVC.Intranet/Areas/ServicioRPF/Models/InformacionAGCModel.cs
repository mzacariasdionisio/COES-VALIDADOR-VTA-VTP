using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using System.Collections.Generic;


namespace COES.MVC.Intranet.Areas.ServicioRPF.Models
{
    public class InformacionAGCModel
    {
        #region Campos Comunes
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }
        public bool UsarLayoutModulo { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string HtmlListado { get; set; }
        #endregion

        public string LstUrsSinPtoMedicion { get; set; }
        public string Fecha { get; set; }
        public string FechaExportacion { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EveRsfdetalleDTO> ListaUrs { get; set; }
        public List<EveRsfdetalleDTO> ListaUrsPopup { get; set; }
        public List<EqEquipoDTO> ListaEquipos { get; set; }
        public GraficoWeb Grafico { get; set; }
    }
}