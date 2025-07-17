using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_VERSION
    /// </summary>
    public partial class CbVersionDTO : EntityBase
    {
        public int Cbenvcodi { get; set; }
        public int Cbvercodi { get; set; }
        public int Cbvernumversion { get; set; }
        public string Cbverestado { get; set; }
        public string Cbverusucreacion { get; set; }
        public DateTime Cbverfeccreacion { get; set; }
        public int? Cbveroperacion { get; set; }
        public string Cbverdescripcion { get; set; }
        public int? Cbverconexion { get; set; }
        public int? Cbvertipo { get; set; }
    }

    public partial class CbVersionDTO
    {
        public string CbverfeccreacionDesc { get; set; }
        public string CbveroperacionDesc { get; set; }
        public string CbverconexionDesc { get; set; }

        public List<CbArchivoenvioDTO> ListaArchivo { get; set; }
        public List<CbDatosDTO> ListaDato { get; set; }

        public List<CbEnvioCentralDTO> ListaCentralXVersion { get; set; }
    }
}
