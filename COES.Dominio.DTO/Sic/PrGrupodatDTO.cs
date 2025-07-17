using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_GRUPODAT
    /// </summary>
    [Serializable]
    public class PrGrupodatDTO : EntityBase
    {
        public DateTime? Fechadat { get; set; }
        public int Concepcodi { get; set; }
        public int Grupocodi { get; set; }
        public string Lastuser { get; set; }
        public string Formuladat { get; set; }
        public int Deleted { get; set; }
        public DateTime? Fechaact { get; set; }
        public string Gdatcomentario { get; set; }
        public string Gdatsustento { get; set; }
        public int? Gdatcheckcero { get; set; }

        public string Concepabrev { get; set; }
        public decimal Valor { set; get; }
        public decimal? ValorDecimal { set; get; }
        public int ValorEntero { get; set; }
        public string ConcepUni { get; set; }
        public string ConcepDesc { get; set; }
        public string ConcepDesc2 { get; set; }
        public string GrupoNomb { get; set; }
        public string ConcepTipo { get; set; }

        public int Grupopadre { get; set; }
        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        public string Centralnomb { get; set; }
        public string Tipocombustible { get; set; }
        public int Equipadre { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public string Central { get; set; }
        public string FlagModoEspecial { get; set; }
        public int? Grupocodimodo { get; set; }
        public int Famcodi { get; set; }
        public string Barrbarratransferencia { get; set; }

        //VALORES DE CONCEPTO (FICHA TÉCNICA)
        public int? Repcodi { get; set; }
        public string Concepfichatec { get; set; }// para filtro
        public string Concepnombficha { get; set; }
        public int NroItem { get; set; } // Indice para la importación
        public string Observaciones { get; set; }
        public bool EsSustentoConfidencial { get; set; }
        public int Conceporden { get; set; }
        public string Concepocultocomentario { get; set; }

        #region Curva Consumo

        public int GrupocodiNCP { get; set; }
        public int Curvcodi { get; set; }
        public int Curvgrupocodiprincipal { get; set; }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public string FechadatDesc { get; set; }
        public string TipogrupoDesc { get; set; }
        public string FechaactDesc { get; set; }
        public int? Deleted2 { get; set; }
        public string EstadoDesc { get; set; }
        public string Color { get; set; }
        public int Catecodi { get; set; }
        public int OrdenCatecodi { get; set; }
        public int Conceppropeq { get; set; }
        public int Conceppadre { get; set; }
        public int Cambio { get; set; }
        public string CeldaExcel { get; set; }
        public string GdatcheckceroDesc { get; set; }
        #endregion
        public string Grupoactivo { get; set; }
        public string Grupoestado { get; set; }

        #region Numerales Datos Base
        public string Dia { get; set; }
        public string Formula { get; set; }
        #endregion

        #region SIOSEIN
        public string Osinergcodi { get; set; }
        public string Osinergcodi2 { get; set; }
        public string Osinergcodi3 { get; set; }    //SIOSEIN-PRIE-2021
        public int? Grupocodi2 { get; set; }
        public int? Fenergcodi { get; set; }
        public string Tipogenerrer { get; set; }
        #endregion

        #region Subastas
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Acta { get; set; }
        public string BandaURS { get; set; }
        public string FechaIniciodesc { get; set; }
        public string FechaFindesc { get; set; }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class PotenciaEfectivaReporte : PrGrupodatDTO
    {

        public decimal Variacion { get; set; }
        public decimal ValorPeAct { get; set; }
        public decimal ValorPeAnt { get; set; }
        public decimal ValorPfAct { get; set; }
        public decimal ValorPfAnt { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }

    }
}
