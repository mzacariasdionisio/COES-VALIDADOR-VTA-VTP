using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_PRUEBAUNIDAD
    /// </summary>
    public class EvePruebaunidadDTO : EntityBase
    {
        public int Prundcodi { get; set; }
        public DateTime? Prundfecha { get; set; }
        public int? Prundescenario { get; set; }
        public DateTime? Prundhoraordenarranque { get; set; }
        public DateTime? Prundhorasincronizacion { get; set; }
        public DateTime? Prundhorainiplenacarga { get; set; }
        public DateTime? Prundhorafalla { get; set; }
        public DateTime? Prundhoraordenarranque2 { get; set; }
        public DateTime? Prundhorasincronizacion2 { get; set; }
        public DateTime? Prundhorainiplenacarga2 { get; set; }
        public string Prundsegundadesconx { get; set; }
        public string Prundfallaotranosincronz { get; set; }
        public string Prundfallaotraunidsincronz { get; set; }
        public string Prundfallaequiposinreingreso { get; set; }
        public string Prundcalchayregmedid { get; set; }
        public DateTime? Prundcalchorafineval { get; set; }
        public string Prundcalhayindisp { get; set; }
        public string Prundcalcpruebaexitosa { get; set; }
        public decimal? Prundcalcperiodoprogprueba { get; set; }
        public string Prundcalccondhoratarr { get; set; }
        public string Prundcalccondhoraprogtarr { get; set; }
        public string Prundcalcindispprimtramo { get; set; }
        public string Prundcalcindispsegtramo { get; set; }
        public decimal? Prundrpf { get; set; }
        public decimal? Prundtiempoprueba { get; set; }
        public string Prundusucreacion { get; set; }
        public DateTime? Prundfeccreacion { get; set; }
        public string Prundusumodificacion { get; set; }
        public DateTime? Prundfecmodificacion { get; set; }
        public int Grupocodi { get; set; }
        public string Prundeliminado { get; set; }
        public string PrundUnidad { get; set; }
        public decimal Prundpotefectiva { get; set; }
        public decimal Prundtiempoentarranq { get; set; }
        public decimal Prundtiempoarranqasinc { get; set; }
        public decimal Prundtiemposincapotefect { get; set; }
        #region INDISPONIBILIDADES
        public string Emprnomb { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public int Emprcodi { get; set; }
        #endregion
    }
}
