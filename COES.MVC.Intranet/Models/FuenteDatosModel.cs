using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Models
{
    public class FuenteDatosModel
    {
        public List<SiPlazoenvioDTO> ListaSiPlazoEnvioFuenteDatos { get; set; }
        public List<SiAmplazoenvioDTO> ListaSiAmplazoenvio { get; set; }
        public List<SiFuentedatosDTO> ListaFuentedatos { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public SiPlazoenvioDTO PlazoEnvioEdicion;

        public string FechaPeriodo { get; set; }
        public string FechaAmpliacion { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int HoraPlazo { get; set; }

        public string Mensaje { get; set; }
        public string Resultado { get; set; }
    }
}