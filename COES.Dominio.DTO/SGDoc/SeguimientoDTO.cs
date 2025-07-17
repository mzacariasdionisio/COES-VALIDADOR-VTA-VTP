using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.SGDoc
{
    public class SeguimientoAreaDTO
    {
        public int NumMsg { get; set; }
        public DateTime? FechaMax { get; set; }
        public string DescripAten { get; set; }
        public int FljCodigo { get; set; }
        public string NombreAreaPadre { get; set; }
        public int FljDetCodigo { get; set; }
        public string NombreArea { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public string Estado { get; set; }
        public string ComentarioPadre { get; set; }
        public DateTime? FechaAtencion { get; set; }
        public int CodigoArea { get; set; }
        public int CodigoAreaPadre { get; set; }
        public int FljNivel { get; set; }
        public int FljDetOrigen { get; set; }
        public int FljDetDestino { get; set; }
        public int Areacode { get; set; }
    }

    public class SeguimientoEspecialistaDTO
    {
        public int NumMsg { get; set; }
        public int Prioridad { get; set; }
        public DateTime? FechaMax { get; set; }
        public string DescripAten { get; set; }
        public int FljCodigo { get; set; }
        public string NombreAreaPadre { get; set; }
        public int FljDetCodigo { get; set; }
        public string NombreArea { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public string Estado { get; set; }        
        public string ComentarioPadre { get; set; }
        public DateTime? FechaAtencion { get; set; }
        public int CodigoArea { get; set; }
        public int CodigoAreaPadre { get; set; }
        public int FljNivel { get; set; }
        public int FljDetOrigen { get; set; }
        public int FljDetDestino { get; set; }
    }

}
