using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class PrnExogenamedicionDTO : EntityBase
    {
        public int Exmedicodi { get; set; }
        public int Varexocodi { get; set; }
        public int Aremedcodi { get; set; }
        public DateTime Exmedifecha { get; set; }
        public int Tipoinfocodi { get; set; }
        public decimal? H1 { get; set; }
        public decimal Exmeditotal { get; set; }
        public decimal? H2 { get; set; }
        public decimal? H3 { get; set; }
        public decimal? H4 { get; set; }
        public decimal? H5 { get; set; }
        public decimal? H6 { get; set; }
        public decimal? H7 { get; set; }
        public decimal? H8 { get; set; }
        public decimal? H9 { get; set; }
        public decimal? H10 { get; set; }
        public decimal? H11 { get; set; }
        public decimal? H12 { get; set; }
        public decimal? H13 { get; set; }
        public decimal? H14 { get; set; }
        public decimal? H15 { get; set; }
        public decimal? H16 { get; set; }
        public decimal? H17 { get; set; }
        public decimal? H18 { get; set; }
        public decimal? H19 { get; set; }
        public decimal? H20 { get; set; }
        public decimal? H21 { get; set; }
        public decimal? H22 { get; set; }
        public decimal? H23 { get; set; }
        public decimal? H24 { get; set; }
        public DateTime Exmedifeccreacion { get; set; }

        #region Adicionales

        public Boolean valid { get; set; }
        public string Tipoinfoabrev { get; set; }

        public string AreaNomb { get; set; }//prueba
        #endregion

    }
}
