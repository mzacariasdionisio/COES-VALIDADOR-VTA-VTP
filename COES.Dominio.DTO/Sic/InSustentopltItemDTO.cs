using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_SUSTENTOPLT_ITEM
    /// </summary>
    public partial class InSustentopltItemDTO : EntityBase
    {
        public int Inpsticodi { get; set; }
        public string Inpstidesc { get; set; }
        public int Inpstcodi { get; set; }
        public int Inpstiorden { get; set; }
        public int Inpstitipo { get; set; }
    }

    public partial class InSustentopltItemDTO
    {
        public string Instdrpta { get; set; }
        public bool PuedeCargarArchivoSoloFoto { get; set; }
        public bool PuedeCargarArchivo { get; set; }
    }
}
