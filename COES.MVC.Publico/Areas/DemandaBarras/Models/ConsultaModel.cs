using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.DemandaBarras.Models
{
    public class ConsultaModel
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string HistoricoDesde { get; set; }
        public string HistoricoHasta { get; set; }
        public string DiarioDesde { get; set; }
        public string DiarioHasta { get; set; }
        public string SemanalDesde { get; set; }
        public string SemanalHasta { get; set; }
        public int Tipo { get; set; }
    }
}