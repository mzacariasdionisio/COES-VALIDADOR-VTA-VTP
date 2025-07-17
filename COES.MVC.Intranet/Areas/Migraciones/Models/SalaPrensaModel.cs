using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Migraciones.Models
{
    public class SalaPrensaModel
    {
        public string Fecha { get; set; }
        public string Titulo { get; set; }
        [AllowHtml]
        public string Descripcion { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string Estado { get; set; }
        public int Evento { get; set; }
        public int Codigo { get; set; }
        public string Tipo { get; set; }
        public int nRegistros { get; set; }
        public string Imagen { get; set; }
        public WbComunicadosDTO Wbcomunicados { get; set; }
        public string Resultado { get; set; }
        public string Resumen { get; set; }
    }
}