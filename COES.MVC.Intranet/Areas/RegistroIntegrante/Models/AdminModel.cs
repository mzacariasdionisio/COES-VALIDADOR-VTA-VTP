using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Models
{
    public class AdminModel
    {
        public List<RiHistoricoDTO> Listado { get; set; }
        public RiHistoricoDTO Entidad { get; set; }

        public int Codigo { get; set; }
        public int Anio { get; set; }
        public string TipoOperacion { get; set; }
        public string Descripcion { get; set; }
        public string Fecha { get; set; }

    }
}