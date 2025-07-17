using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.InformesOsinergmin.Models
{
    public class InformeModel
    {
        public string Fecha { get; set; }
        public double[][] Cuadro1 { get; set; }
        public double[][] Cuadro2 { get; set; }
        public double[][] Cuadro3 { get; set; }
        public int NroDias { get; set; }
        public List<MedicionReporteDTO> ListaComprativo { get; set; }

    }
}