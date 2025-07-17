using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_SUSTENTO_DET
    /// </summary>
    public partial class InSustentoDetDTO : EntityBase, ICloneable
    {
        public int Instdcodi { get; set; }
        public int Instcodi { get; set; }
        public int Inpsticodi { get; set; }
        public string Instdrpta { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class InSustentoDetDTO
    {
        public string Inpstidesc { get; set; }
        public int Inpstitipo { get; set; }
        public bool PuedeCargarArchivoSoloFoto { get; set; }
        public bool PuedeCargarArchivo { get; set; }

        public int Subcarpetafiles { get; set; }
        public List<InArchivoDTO> ListaArchivo { get; set; }
    }
}
