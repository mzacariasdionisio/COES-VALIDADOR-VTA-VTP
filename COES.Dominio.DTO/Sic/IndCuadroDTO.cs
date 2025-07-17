using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_CUADRO
    /// </summary>
    public partial class IndCuadroDTO : EntityBase
    {
        public int Icuacodi { get; set; }
        public string Icuatitulo { get; set; }
        public string Icuanombre { get; set; }
        public string Icuasubtitulo { get; set; }
    }

    public partial class IndCuadroDTO
    {
        public int Famcodi { get; set; }
        public List<int> ListaFamcodi { get; set; }
        public bool TieneDivisionTablaXTipo { get; set; }
        public bool TieneDivisionTablaXTiempo { get; set; }
        public bool TieneColumnaTipo { get; set; }
        public bool TieneColumnaTiempo { get; set; }
        public bool TieneColumnaMedicion { get; set; }
        public List<string> ListaNota { get; set; }
        public string Tgenernomb { get; set; }

        public DateTime PeriodoIniHistorico { get; set; }
        public DateTime PeriodoFinHistorico { get; set; }
        public string PeriodoIniHistoricoDesc { get; set; }
        public string PeriodoFinHistoricoDesc { get; set; }
    }
}
