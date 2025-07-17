using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CCC_VCOM
    /// </summary>
    public partial class CccVcomDTO : EntityBase
    {
        public int Vcomcodi { get; set; }
        public int Cccvercodi { get; set; }
        public int Fenergcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public int Equicodi { get; set; }
        public int Grupocodi { get; set; }
        public decimal? Vcomvalor { get; set; }
        public string Vcomcodigomop { get; set; }
        public string Vcomcodigotcomb { get; set; }
        public int Tinfcoes { get; set; }
        public int Tinfosi { get; set; }
    }

    public partial class CccVcomDTO
    {
        public string Emprabrev { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Fenergnomb { get; set; }
        public string Gruponomb { get; set; }

        public string Tinfcoesabrev { get; set; }
        public string Tinfosiabrev { get; set; }

        public decimal? ValorMedidaCoes { get; set; }
        public string VcomvalorDesc { get; set; }
    }
}
