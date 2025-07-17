using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Medidores.Models
{
    public class InterconexionModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<string> ListaSemanas { get; set; }
        //public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }
        public int NroSemana { get; set; }
        public string Fecha { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaPlazo { get; set; }
        public int HoraPlazo { get; set; }
        public string Resultado { get; set; }
        public int empresa { get; set; }
        public List<int> ListaAnios { get; set; }
        public int Anio { get; set; }
        public string ValidacionPlazo { get; set; }
        public List<string> Validaciones { get; set; }
        //public List<CeldaInfo> ListaErrores { get; set; }
        //public List<ExtArchivoDTO> ListaEnvios { get; set; }
        //public List<MeMedidorDTO> ListaMedidor { get; set; }
        //public List<PeriodoMedidor> ListaPeriodo { get; set; }
        public int PeriodoIni { get; set; }
        public int Estado { get; set; }
        public bool EnPlazo { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
    }
}