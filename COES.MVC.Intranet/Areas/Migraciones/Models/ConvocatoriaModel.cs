using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Migraciones.Models
{
    public class ConvocatoriaModel
    {
        public string Fecha { get; set; }
        public string Abreviatura { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string Estado { get; set; }
        public int Codigo { get; set; }
        public WbConvocatoriasDTO Wbconvocatorias { get; set; }
        public string Resultado { get; set; }
        public int nRegistros { get; set; }
    }
}