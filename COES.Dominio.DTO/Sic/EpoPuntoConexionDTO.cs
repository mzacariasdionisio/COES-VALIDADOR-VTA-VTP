using System;
using COES.Base.Core;
namespace COES.Dominio.DTO.Sic
{
    public class EpoPuntoConexionDTO : EntityBase
    {
        public int PuntCodi { get; set; }
        public string PuntDescripcion { get; set; }
        public string PuntActivo { get; set; }
        public DateTime LastDate { get; set; }
        public string LastUser { get; set; }
        public int ZonCodi { get; set; }

        public string ZonDescripcion { get; set; }
    }
}
