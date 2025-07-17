using System;
using System.Collections.Generic;
using COES.Dominio.DTO.ReportesFrecuencia;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Models
{
    public class ReporteFrecuenciaAuditModel
    {

        public List<FrecuenciasAudit> ListaAudit { get; set; }
        public string sError { get; set; }
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int IdGPS { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Accion { get; set; }
    }
  
}