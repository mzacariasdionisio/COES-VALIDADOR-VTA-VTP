using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_TIPOIMPUGNACION
    /// </summary>
    public class WbTipoimpugnacionDTO : EntityBase
    {
        public int Timpgcodi { get; set; } 
        public string Timpgnombre { get; set; }
        public string Timpgnombdecision { get; set; }
        public string Timpgnombrefecha { get; set; } 
    }
}
