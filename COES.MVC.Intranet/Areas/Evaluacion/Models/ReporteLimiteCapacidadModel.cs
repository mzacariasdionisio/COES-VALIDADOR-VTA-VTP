using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
namespace COES.MVC.Intranet.Areas.Evaluacion.Models
{
    public class ReporteLimiteCapacidadModel
    {
        public List<EprEquipoDTO> ListaRevision { get; set; }
        public HandsonModel Handson { get; set; }

    }

    public class ListadoReporteLimiteCapacidadModel
    {
        public List<EprEquipoDTO> ListaReporteLimiteCapacidad { get; set; }
        public string RevisionMax { get; set; }
    }

    public class ReporteLimiteCapacidadEditarModel
    {
        public string Codigo { get; set; }
        public string Revision { get; set; }
        public string Descripcion { get; set; }
        public string EmitidoEl { get; set; }
        public string ElaboradoPor { get; set; }
        public string RevisadoPor { get; set; }
        public string AprobadoPor { get; set; }
        public string EsRegistro { get; set; }
        public string UsuarioCreacion { get; set; }
        public string FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }
        public string Accion { get; set; }

    }

    public class ReporteLimiteCapacidadEliminarModel
    {
        public int Estado { get; set; }
        public string Mensaje { get; set; }
    }
}