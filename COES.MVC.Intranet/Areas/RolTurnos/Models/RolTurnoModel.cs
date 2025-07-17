using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.RolTurnos.Models
{
    public class RolTurnoModel
    {
        public string Fecha { get; set; }
        public ReporteRolTurno Reporte { get; set; }
    }
}