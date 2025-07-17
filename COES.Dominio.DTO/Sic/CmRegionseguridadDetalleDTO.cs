using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_REGIONSEGURIDAD_DETALLE
    /// </summary>
    public class CmRegionseguridadDetalleDTO : EntityBase
    {
        public int Regdetcodi { get; set; } 
        public int Regsegcodi { get; set; } 
        public int Equicodi { get; set; } 
        public string Regsegusucreacion { get; set; } 
        public DateTime? Regsegfeccreacion { get; set; } 
        public string Nombretna { get; set; }
        public string Tipoequipo { get; set; }
        public int Famcodi { get; set; }

        //Movisoft 26-04-2022
        public int RegsegcodiExcel { get; set; }
    }
}
