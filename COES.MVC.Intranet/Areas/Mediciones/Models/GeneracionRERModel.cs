using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Mediciones.Models
{
    public class GeneracionRERModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<string> ListaSemanas { get; set; }
        public int NroSemana { get; set; }
        public string Fecha { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public List<int> ListaAnios { get; set; }
        public int Anio { get; set; }
        public string Anho { get; set; }
        public List<GenericoDTO> ListaSemanas2 { get; set; }

        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<MeEnvioDTO> ListaReporte { get; set; }
        public bool TienePermisoDTI { get; set; }
    }
}