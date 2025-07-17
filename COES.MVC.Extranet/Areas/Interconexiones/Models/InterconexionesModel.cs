using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.MVC.Extranet.Models;

namespace COES.MVC.Extranet.Areas.Interconexiones.Models
{
    public class InterconexionesModel : FormatoModel
    {
        public string FechaPlazo { get; set; }
        public string Resultado { get; set; }
        public int HoraPlazo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }
        public List<MeMedidorDTO> ListaMedidor { get; set; }
        public List<PeriodoMedidor> ListaPeriodoMedidor { get; set; }
        public int PeriodoIni { get; set; }
        public int Estado { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public string TituloMedidorPrincipal { get; set; }
        public string TituloMedidorSecundario { get; set; }
        public int HojaMedidorPrincipal { get; set; }
        public int HojaMedidorSecundario { get; set; }
        public int IdMedidor { get; set; }
    }

    public class PeriodoMedidor
    {
        public int PeriodoIni { get; set; }
        public int PeriodoFin { get; set; }
        public int IdMedidor { get; set; }
        public string NombreMedidor { get; set; }
    }
}