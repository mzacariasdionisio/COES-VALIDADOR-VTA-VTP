using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_ARCHIVO
    /// </summary>
    public partial class FtExtEnvioArchivoDTO : EntityBase
    {
        public int Ftearccodi { get; set; }
        public string Ftearcnombreoriginal { get; set; }
        public string Ftearcnombrefisico { get; set; }
        public int Ftearcorden { get; set; }
        public int Ftearcestado { get; set; }
        public string Ftearcflagsustentoconf { get; set; }
        public int Ftearctipo { get; set; }
    }

    public partial class FtExtEnvioArchivoDTO 
    {
        public bool EsNuevo { get; set; }
        public string TipoArchivo { get; set; }

        public int Ftitcodi { get; set; }
        public int Fevrqcodi { get; set; }
        public int Ftereqcodi { get; set; } //requisito
        public int Fterdacodi { get; set; }
        public int Fteeqcodi { get; set; }
        public int Ftedatcodi { get; set; }
        public int Ftrevcodi { get; set; }
        public int Revacodi { get; set; }
        public FtExtEnvioRelrevareaarchivoDTO RelacionArchivoRevArea { get; set; }
    }
}
