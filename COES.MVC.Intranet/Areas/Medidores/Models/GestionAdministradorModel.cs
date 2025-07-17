using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Medidores.Models
{
    public class GestionAdministradorModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Mes { get; set; }
        public string MesInicio { get; set; }
        public string MesFin { get; set; }
        public List<MeEstadoenvioDTO> ListaEstadoEnvio { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }

        public List<MeEnvioDTO> ListaEnvio { get; set; }
        public List<MeCambioenvioDTO> ListaCambioEnvio { get; set; }
        public List<MeValidacionDTO> ListaValidacion { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }

        public string Resultado { get; set; }
        public string NombreFortmato { get; set; }

        public int Columnas { get; set; }
        public int Resolucion { get; set; }

        public string Fecha { get; set; }
        public string FechaPlazo { get; set; }
        public int HoraPlazo { get; set; }
        public int DiaMes { get; set; }
    }
}