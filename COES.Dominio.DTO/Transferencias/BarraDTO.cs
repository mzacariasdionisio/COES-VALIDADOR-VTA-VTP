using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_BARRA
    /// </summary>
    public class BarraDTO
    {
        public System.Int32 BarrCodi { get; set; }
        public System.String BarrNombre { get; set; }
        public System.String BarrTension { get; set; }
        public System.String BarrPuntoSumirer { get; set; }
        public System.String BarrBarraBgr { get; set; }
        public System.String BarrEstado { get; set; }
        public System.String BarrFlagBarrTran { get; set; }
        public System.Int32 AreaCodi { get; set; }
        public System.String BarrNombBarrTran { get; set; }
        public System.String BarrFlagDesbalance { get; set; }
        public System.String BarrUserName { get; set; }
        public System.DateTime BarrFecIns { get; set; }
        public System.DateTime BarrFecAct { get; set; }
        public System.String AreaNombre { get; set; }
        public System.Int32 Grupocodi { get; set; }
        public System.String Gruponomb { get; set; }
        public System.Int32 Grupopadre { get; set; }
        public System.Int32 Emprcodi { get; set; }
        public System.String Emprnomb { get; set; }
        public decimal BarrFactorPerdida { get; set; }
        #region siosein2
        public int Equicodi { get; set; }
        public int Ordencabecera { get; set; }
        #endregion

        #region SIOSEIN-PRIE-2021
        public System.String OsinergCodi { get; set; }
        #endregion

        //CPPA-2024-CU04
        public string BarrBarraTransferencia { get; set; }
        public string BarrNombreConcatenado { get; set; }
    }
}
