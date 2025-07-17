using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_REP_CABECERA
    /// </summary>
    public partial class CbRepCabeceraDTO : EntityBase
    {
        public int Cbrcabcodi { get; set; } 
        public int Cbrprocodi { get; set; } 
        public int Cbrepcodi { get; set; } 
        public string Cbrcabdescripcion { get; set; } 
    }
    public partial class CbRepCabeceraDTO 
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
    }
}
