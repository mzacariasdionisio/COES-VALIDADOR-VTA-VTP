using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que mapea la tabla TRN_PONTENCIA_CONTRATADA
    /// </summary>
    public class TrnPotenciaContratadaDTO : EntityBase
    {
        #region Campos Originales
        public int TrnPctCodi { get; set; }
        public int PeriCodi { get; set; }
        public int indexBarra { get; set; }
        public int esPrimerRegistro { get; set; }
        public int EmprCodi { get; set; }
        public int CoresoCodi { get; set; }
        public int CoresoCodiFirst { get; set; }
        public string TrnPctPtoSumins { get; set; }
        public decimal? TrnPctTotalMwFija { get; set; }
        public decimal? TrnPctHpMwFija { get; set; }
        public decimal? TrnPctHfpMwFija { get; set; }
        public decimal? TrnPctTotalMwVariable { get; set; }
        public decimal? TrnPctHpMwFijaVariable { get; set; }
        public decimal? TrnPctHfpMwFijaVariable { get; set; }
        public string TrnPctComeObs { get; set; }
        public int TrnpcTipoPotencia { get; set; }
        public string TrnPctUserNameIns { get; set; }
        public int TrnPctExcel { get; set; }
        public DateTime TrnPctFecIns { get; set; }
        #endregion

        #region Campos Adicionales
        public string CoresoCodigo { get; set; }
        public int CliEmprCodi { get; set; }
        public int TipConCodi { get; set; }
        public int TipUsuCodi { get; set; }
        public int? BarrCodi { get; set; }
        public string PeriNombre { get; set; }
        public DateTime CoresoFechaInicio { get; set; }
        public DateTime CoresoFechaFin { get; set; }
        public string EmprNomb { get; set; }
        public string CliNombre { get; set; }
        public string TipConNombre { get; set; }
        public string TipUsuNombre { get; set; }
        public string BarrBarraTransferencia { get; set; }
        public string BarrSuministro { get; set; }
        public DateTime CoresoFechaRegistro { get; set; }
        public string CoresoDescripcion { get; set; }
        public string CoresoDetalle { get; set; }


        #endregion



        //Nuevo
        #region Nuevas Propiedades
        public int? CoregeCodi { get; set; }
        public int? CoregeCodiFirst { get; set; }
        public int? TrnpcNumOrd { get; set; }
        public int? TrnpCcodiCas { get; set; }
        public int? TrnpCagrp { get; set; }
        public int TrnnpcCodicas { get; set; }
        public int? TrnPcEnvCodi { get; set; }
        public string TrnPcEnvUsuario { get; set; }
        public DateTime TrnPcEnvFechaRe { get; set; }
        public string TrnPcEnvHoraRe { get; set; }
        public string TrnpcTipoCasoAgrupado { get; set; }

        #endregion Nuevas Propiedades

    }
}
