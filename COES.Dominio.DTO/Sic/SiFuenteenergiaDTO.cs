using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_FUENTEENERGIA
    /// </summary>
    public partial class SiFuenteenergiaDTO : EntityBase
    {
        public int Fenergcodi { get; set; }
        public string Fenergabrev { get; set; }
        public string Fenergnomb { get; set; }
        public int? Tgenercodi { get; set; }
        public string Fenergcolor { get; set; }
        public int Estcomcodi { get; set; }
        public string Tgenernomb { get; set; }
        public string Osinergcodi { get; set; }
        public int Tinfcoes { get; set; }
        public int Tinfosi { get; set; }

        public string Tinfcoesabrev { get; set; }
        public string Tinfosiabrev { get; set; }
        public string Tinfcoesdesc { get; set; }
        public string Tinfosidesc { get; set; }

    }

    public partial class SiFuenteenergiaDTO
    {
        public string Grupocomb { get; set; }

        public int Ctgdetcodi { get; set; }
        public string Ctgdetnomb { get; set; }
        public bool CtgdetFicticio { get; set; }

        public bool ValidarDatoObligatorio { get; set; }

        #region SIOSEIN
        public decimal? Promedio { get; set; }
        public decimal? Total { get; set; }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public decimal? ValorTotalHoras { get; set; }
        public int Fenergorden { get; set; }
        public List<int> ListaEquicodiXFenerg { get; set; } = new List<int>();
        public int GrupocodiMaxPe { get; set; }
        #endregion

        #region Informes SGI

        public int TotalMinuto { get; set; }
        public decimal? Potencia { get; set; }
        public bool EsRer { get; set; }

        #endregion
    }
}
