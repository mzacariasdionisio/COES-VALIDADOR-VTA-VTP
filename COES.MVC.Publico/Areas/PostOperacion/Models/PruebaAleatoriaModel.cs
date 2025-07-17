using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.PostOperacion.Models
{
    /// <summary>
    /// Model para el reporte de pruebas aleatorias
    /// </summary>
    public class PruebaAleatoriaModel
    {
        public List<PrLogsorteoDTO> ListadoSorteo { get; set; }
        public List<PrLogsorteoDTO> ListadoSituacionUnidades { get; set; }
        public List<PrLogsorteoDTO> ListadoMantenimientos { get; set; }
    }
}