using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class BitacoraModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public string Fecha { get; set; }
        public List<MeJustificacionDTO> Bitacora { get; set; }
        public List<Tuple<int, string, bool>> ListTipoEmpresa { get; set; }
        public List<Tuple<int, string, bool>> ListTipoDemanda { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }

    }
}