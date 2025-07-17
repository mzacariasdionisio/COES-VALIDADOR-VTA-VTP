using System;
using System.Collections.Generic;
using COES.Base.Core;
using COES.Framework.Base.Tools;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_MEDICION24
    /// </summary>
    [Serializable]
    public class MeMedicion24DTO : EntityBase, IMeMedicion
    {
        public int Lectcodi { get; set; }
        public DateTime Medifecha { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Ptomedicodi { get; set; }
        public decimal? Meditotal { get; set; }
        public string Mediestado { get; set; }
        public decimal? H1 { get; set; }
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
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public int? T1 { get; set; }
        public int? T2 { get; set; }
        public int? T3 { get; set; }
        public int? T4 { get; set; }
        public int? T5 { get; set; }
        public int? T6 { get; set; }
        public int? T7 { get; set; }
        public int? T8 { get; set; }
        public int? T9 { get; set; }
        public int? T10 { get; set; }
        public int? T11 { get; set; }
        public int? T12 { get; set; }
        public int? T13 { get; set; }
        public int? T14 { get; set; }
        public int? T15 { get; set; }
        public int? T16 { get; set; }
        public int? T17 { get; set; }
        public int? T18 { get; set; }
        public int? T19 { get; set; }
        public int? T20 { get; set; }
        public int? T21 { get; set; }
        public int? T22 { get; set; }
        public int? T23 { get; set; }
        public int? T24 { get; set; }

        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Tipoinfoabrev { get; set; }
        public int Tipoptomedicodi { get; set; }
        public string Tipoptomedinomb { get; set; }
        public string Ptomedibarranomb { get; set; }
        public int Famcodi { get; set; }
        public string Famabrev { get; set; }
        public int IdCuenca { get; set; }
        public string Cuenca { get; set; }
        public string Gruponomb { get; set; }
        public string Ptomedielenomb { get; set; }
        public int Grupocodi { get; set; }
        public int ReporteOrden { get; set; }

        public string Osinergcodi { get; set; }
        public string Osicodi { get; set; } //Movisoft 2022-03-07

        #region PR5
        public int Equipadre { get; set; }
        public string Central { get; set; }
        public string Equipopadre { get; set; }
        public decimal CalculadoFactor { get; set; }
        public int Tgenercodi { get; set; }
        public string Tgenernomb { get; set; }
        public int TipoResultadoFecha { get; set; }
        public int? Reporcodi { get; set; }
        public int TipoReporte { get; set; }
        public int Fenergcodi { get; set; }
        public string Fenergnomb { get; set; }
        public string Fenergcolor { get; set; }
        public int Estcomcodi { get; set; }
        public string Estcomnomb { get; set; }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string Grupoabrev { get; set; }
        public string Grupotipo { get; set; }
        public string Equiabrev { get; set; }
        public decimal? Minimo { get; set; }
        public decimal? PotenciaEfectiva { get; set; }
        public string Digsilent { get; set; }
        public string Areanomb { get; set; }
        public DateTime? FechapropequiMin { get; set; }
        public DateTime? FechapropequiPotefec { get; set; }
        public string FechapropequiMinDesc { get; set; }
        public string FechapropequiPotefecDesc { get; set; }
        public decimal FactorUnidadFicticia { get; set; }
        public int NumUnidadesXGrupo { get; set; }
        public decimal? MVAxUnidad { get; set; }
        public decimal? PotenciaTotalMVA { get; set; }
        #endregion

        #region Mejoras RDO
        public int Recurcodi { get; set; }
        public int Catcodi { get; set; }
        public string E1 { get; set; }
        public string E2 { get; set; }
        public string E3 { get; set; }
        public string E4 { get; set; }
        public string E5 { get; set; }
        public string E6 { get; set; }
        public string E7 { get; set; }
        public string E8 { get; set; }
        public string E9 { get; set; }
        public string E10 { get; set; }
        public string E11 { get; set; }
        public string E12 { get; set; }
        public string E13 { get; set; }
        public string E14 { get; set; }
        public string E15 { get; set; }
        public string E16 { get; set; }
        public string E17 { get; set; }
        public string E18 { get; set; }
        public string E19 { get; set; }
        public string E20 { get; set; }
        public string E21 { get; set; }
        public string E22 { get; set; }
        public string E23 { get; set; }
        public string E24 { get; set; }
        #endregion

        #region Informes SGI
        public string DiaMaximaDemanda { get; set; }

        #endregion
    }
}
