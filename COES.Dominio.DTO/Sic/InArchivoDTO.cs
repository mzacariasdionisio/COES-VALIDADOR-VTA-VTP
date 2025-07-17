using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_ARCHIVO
    /// </summary>
    public partial class InArchivoDTO : EntityBase, ICloneable
    {
        public int Inarchcodi { get; set; }
        public string Inarchnombreoriginal { get; set; }
        public string Inarchnombrefisico { get; set; }
        public int? Inarchorden { get; set; }
        public int Inarchestado { get; set; }
        public int Inarchtipo { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class InArchivoDTO
    {
        public int Infmmcodi { get; set; }
        public int Infvercodi { get; set; }
        public int Infmmhoja { get; set; }
        public bool EsNuevo { get; set; }

        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        public string Equiabrev { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }

        public int Intercodi { get; set; }
        public int Intercarpetafiles { get; set; }
        public int Progrcodi { get; set; }

        public int Msgcodi { get; set; }
        public int Instdcodi { get; set; }
        public int Instcodi { get; set; }
        public int Inpsticodi { get; set; }

        public int Ancho { get; set; }
        public int Alto { get; set; }
        public string PathArchivo { get; set; }
        public string Base64Imagen { get; set; }

        public bool TieneVistaPreviaOffice { get; set; }
        public bool TieneVistaPreviaNoOffice { get; set; }

        public string Modulo { get; set; }
    }
}
