using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_OSIG_SUMiNISTRO_UL
    /// </summary>
    public class IioOsigSuministroUlDTO
    {       
        public int Psiclicodi { get; set; }
        public string Ulsumicodempresa { get; set; }
        public string Ulsumicodsuministro { get; set; }
        public string Ulsuminombreusuariolibre { get; set; }
        public string Ulsumidireccionsuministro { get; set; }
        public string Ulsuminrosuministroemp { get; set; }
        public string Ulsumiubigeo { get; set; }
        public string Ulsumicodusuariolibre { get; set; }
        public string Ulsumicodciiu { get; set; }
        public int Equicodi { get; set; }
        public string Ulsumiusucreacion { get; set; }
        public DateTime Ulsumifeccreacion { get; set; }
        public string Ulsumiusumodificacion { get; set; }
        public DateTime Ulsumifecmodificacion { get; set; }

    }
}