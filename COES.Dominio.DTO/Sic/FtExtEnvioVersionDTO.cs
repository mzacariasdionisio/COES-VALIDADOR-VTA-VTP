using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_VERSION
    /// </summary>
    public partial class FtExtEnvioVersionDTO : EntityBase
    {
        public int Ftevercodi { get; set; }
        public int Ftenvcodi { get; set; }
        public int Estenvcodi { get; set; }
        public int Ftevertipo { get; set; }
        public int Fteveroperacion { get; set; }
        public string Fteverdescripcion { get; set; }
        public string Fteverautoguardado { get; set; }
        public int Fteverconexion { get; set; }
        public string Fteverestado { get; set; }

        public DateTime Fteverfeccreacion { get; set; }
        public string Fteverusucreacion { get; set; }
    }

    public partial class FtExtEnvioVersionDTO
    {
        public string FteverfeccreacionDesc { get; set; }
        public List<FtExtEnvioEqDTO> ListaEquipoEnvio { get; set; }
        public List<FtExtEnvioReqDTO> ListaReqEnvio { get; set; }

        public int NumeroVersion { get; set; }
        public string FteveroperacionDesc { get; set; }
        public string FteverconexionDesc { get; set; }
        public string RealizadoPor { get; set; }

        public bool FlagUpdateVersion { get; set; }
        public List<int> ListaFteeqcodiUpdate { get; set; }
        public List<int> ListaFteeqcodiEliminar { get; set; }
    }
}
