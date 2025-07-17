using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.ReportesFrecuencia
{
    public class CargaVirtualDTO
    {
        public int IdCarga { get; set; }
        public int GPSCodi { get; set; }
        public int CodEmpresa { get; set; }
        public string CodCentral { get; set; }
        public int CodUnidad { get; set; }
        public string FechaCargaInicio { get; set; }
        public string FechaCargaFin { get; set; }
        public string TipoCarga { get; set; }
        public string ArchivoCarga { get; set; }
        public string DataCarga { get; set; }
        public string FechaCargaIni { get; set; }
        public string UsuCarga { get; set; }
        public string UsuCreacion { get; set; }
        public string NombreEquipo { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreUnidad { get; set; }
        public string FechaCreacion { get; set; }
        public string Mensaje { get; set; }
        public int Resultado { get; set; }
    }
}
