using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MMM_CAMBIOVERSION
    /// </summary>
    public class MmmCambioversionDTO : EntityBase
    {
        public int Camvercodi { get; set; }
        public int Vermmcodi { get; set; }
        public int Mmmdatcodi { get; set; }
        public int Camvertipo { get; set; }
        public decimal? Camvervalor { get; set; }
        public DateTime Camverfeccreacion { get; set; }
        public string Camverusucreacion { get; set; }
        public DateTime Mmmdatfecha { get; set; }
    }
}