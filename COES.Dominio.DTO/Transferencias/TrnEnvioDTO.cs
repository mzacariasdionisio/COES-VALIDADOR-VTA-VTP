using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que mapea la tabla TRN_ENVIO
    /// </summary>
    public class TrnEnvioDTO
    {
        #region Campos Originales
        public int TrnEnvCodi { get; set; }
        public int PeriCodi { get; set; }
        public int RecaCodi { get; set; }
        public int EmprCodi { get; set; }
        public int TrnModCodi { get; set; }
        public string TrnEnvTipInf { get; set; }
        public string TrnEnvPlazo { get; set; }
        public string TrnEnvLiqVt { get; set; }
        public string TrnEnvUseIns { get; set; }
        public DateTime TrnEnvFecIns { get; set; }
        public string TrnEnvUseAct { get; set; }
        public DateTime TrnEnvFecAct { get; set; }
        public string TrnEnvUseCoes { get; set; }
        public DateTime TrnEnvFecCoes { get; set; }
        #endregion

        #region Campos Adicionales
        public string EmprNomb { get; set; }
        public string PeriNombre { get; set; }
        public string TrnModNombre { get; set; }
        #endregion
    }
}
