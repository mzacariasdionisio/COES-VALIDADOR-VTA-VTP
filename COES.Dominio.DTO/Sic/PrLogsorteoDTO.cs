using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_LOGSORTEO
    /// </summary>
    public class PrLogsorteoDTO : EntityBase
    {
        public string Logusuario { get; set; }
        public DateTime Logfecha { get; set; }
        public string Logdescrip { get; set; }
        public string Logtipo { get; set; }
        public string Logcoordinador { get; set; }
        public string Logdocoes { get; set; }
        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        public string Equiabrev { get; set; }
        public int Equicodi { get; set; }
        public DateTime Fechaini { get; set; }
        public DateTime Fechafin { get; set; }
        public string Subcausadesc { get; set; }
        public string Evenclase { get; set; }
        public string Evendescrip { get; set; }
        #region FIT SGOCCOES func A

        public DateTime Hophorini { get; set; }
        public string Hopfalla { get; set; }
        public int Subcausacodi { get; set; }
        public int? Equipadre { get; set; }
        public DateTime Evenfin { get; set; }
        public DateTime Evenini { get; set; }
        public DateTime Ichorfin { get; set; }
        public DateTime Ichorini { get; set; }

        public int PRUNDCODI { get; set; }
        public int GRUPOCODI { get; set; }
        public DateTime PRUNDFECHA { get; set; }
        public int PRUNDESCENARIO { get; set; }
        public DateTime PRUNDHORAORDENARRANQUE { get; set; }
        public DateTime PRUNDHORASINCRONIZACION { get; set; }
        public DateTime PRUNDHORAINIPLENACARGA { get; set; }
        public DateTime PRUNDHORAFALLA { get; set; }
        public DateTime PRUNDHORAORDENARRANQUE2 { get; set; }
        public DateTime PRUNDHORASINCRONIZACION2 { get; set; }
        public DateTime PRUNDHORAINIPLENACARGA2 { get; set; }
        public string PRUNDSEGUNDADESCONX { get; set; }
        public string PRUNDFALLAOTRANOSINCRONZ { get; set; }
        public string PRUNDFALLAOTRAUNIDSINCRONZ { get; set; }
        public string PRUNDFALLAEQUIPOSINREINGRESO { get; set; }
        public string PRUNDCALCHAYREGMEDID { get; set; }
        public DateTime PRUNDCALCHORAFINEVAL { get; set; }
        public string PRUNDCALHAYINDISP { get; set; }
        public string PRUNDCALCPRUEBAEXITOSA { get; set; }
        public int PRUNDCALCPERIODOPROGPRUEBA { get; set; }
        public string PRUNDCALCCONDHORATARR { get; set; }
        public string PRUNDCALCCONDHORAPROGTARR { get; set; }
        public string PRUNDCALCINDISPPRIMTRAMO { get; set; }
        public string PRUNDCALCINDISPSEGTRAMO { get; set; }
        public int PRUNDRPF { get; set; }
        public int PRUNDTIEMPOPRUEBA { get; set; }
        public string PRUNDUSUCREACION { get; set; }
        public DateTime PRUNDFECCREACION { get; set; }
        public string PRUNDUSUMODIFICACION { get; set; }
        public DateTime PRUNDFECMODIFICACION { get; set; }
        public string PRUNDELIMINADO { get; set; }
        public int PRUNDPOTEFECTIVA { get; set; }
        public int PRUNDTIEMPOENTARRANQ { get; set; }
        public int PRUNDTIEMPOARRANQASINC { get; set; }
        public int PRUNDTIEMPOSINCAPOTEFECT { get; set; }

        #endregion
    }

    #region FIT SGOCOES func A
    public class Prequipos_validosDTO : EntityBase
    {
        public string equicodi1 { get; set; }
        public string equicodi2 { get; set; }
        public int i_codigo { get; set; }
    }

    #endregion
}
