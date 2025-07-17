using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    /// <summary>
    /// Model para los comparativos de datos
    /// </summary>
    public class ComparativoModel
    {
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string FechaPeriodo { get; set; }
        public string Fecha { get; set; }
        public string FechaInicio { get; set; }        
        public List<CmConfigbarraDTO> ListaBarra { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqEquipoDTO> ListaCentral { get; set; }
        public List<PrGrupoDTO> ListaModo { get; set; }

        public List<PrGrupoDTO> ListaGrupoCentral { get; set; }
        public List<PrGrupoDTO> ListaGrupoDespacho { get; set; }

        public List<ReporteComparativoHOvsDespacho> ListaReporte { get; set; }
        public List<string> ListaMensajeValidacion { get; set; }

        public List<ReporteComparativoHOvsRsvaSec> ListaRsvaSec { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public decimal UmbralCI { get; set; }
    }
}