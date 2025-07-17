using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo.Models
{
    public class ServicioModel
    {
        public List<ServicioRpfDTO> ListaConsulta { get; set; }
        public string FechaConsulta { get; set; }
        public string Usuario { get; set; }
        public int IndicadorReporte { get; set; }
        public List<ServicioGps> ListaGPS { get; set; }
        public bool IndicadorExportar { get; set; }
    }
}