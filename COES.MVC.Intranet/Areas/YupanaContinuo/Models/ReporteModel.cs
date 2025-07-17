using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.YupanaContinuo.Models
{
    public class ReporteModel
    {
        public int Tyupcodi { get; set; }
        public int Yupcfgcodi { get; set; }

        public string Fecha { get; set; }
        public List<GenericoDTO> ListaHora { get; set; }

        public List<CpYupconCfgdetDTO> ListaConfiguracion { get; set; }
        public List<CpRecursoDTO> ListaRecurso { get; set; }
        public List<MePtomedicionDTO> ListaPto { get; set; }

        public bool TienePermisoAdmin { get; set; }
        public int CodigoEnvio { get; set; }
        public List<CpYupconEnvioDTO> ListaEnvio { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Reporte { get; set; }
        public string HtmlList { get; set; }
    }
}