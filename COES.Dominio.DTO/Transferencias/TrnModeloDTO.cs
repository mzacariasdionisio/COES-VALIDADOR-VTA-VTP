using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que mapea la tabla TRN_MODELO
    /// </summary>
    public class TrnModeloDTO : EntityBase
    {
        #region Campos Originales
        public int TrnModCodi { get; set; }
        public string TrnModNombre { get; set; }
        public int EmprCodi { get; set; }
        public string TrnModUseIns { get; set; }
        public DateTime TrnModFecIns { get; set; }
        public string TrnModUseAct { get; set; }
        public DateTime TrnModFecAct { get; set; }
        #endregion

        #region Campos Adicionales
        public string EmprNomb { get; set; }
        #endregion      
    }

    /// <summary>
    /// Clase que mapea la tabla TRN_MODELO_RETIRO
    /// </summary>
    public class TrnModeloRetiroDTO : EntityBase
    {
        #region Campos Originales
        public int TrnMreCodi { get; set; }
        public int TrnModCodi { get; set; }
        public int BarrCodi { get; set; }
        public int CoresoCodi { get; set; }
        public string CoresoCodigo { get; set; }
        public string TrnModRetUseIns { get; set; }
        public DateTime TrnModRetFecIns { get; set; }
        public string TrnModRetUseAct { get; set; }
        public DateTime TrnModRetFecAct { get; set; }
        #endregion

        #region Campos Adicionales
        public string TrnModNombre { get; set; }
        public string BarrBarraTransferencia { get; set; }
        #endregion
    }
}
