using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.Planificacion.Models
{
    public class DescargaModPlanModel
    {
        public List<WbVersionModplanDTO> Listado { get; set; }
        public WbVersionModplanDTO Entidad { get; set; }
        public List<WbVersionModplanDTO> ListaPadres { get; set; }
        public List<WbRegistroModplanDTO> ListaReporte { get; set; }
        public int? Padre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int Codigo { get; set; }
        public int Tipo { get; set; }
        public string NombreVersion { get; set; }
        public string NombrePlan { get; set; }
        public string NombreModelo { get; set; }
        public string NombreManual { get; set; }
        public int Resultado { get; set; }
        public FwAccesoModeloDTO EntidadAcceso { get; set; }
        public string Identificador { get; set; }
        public string FechaConclusion { get; set; }
    }
}