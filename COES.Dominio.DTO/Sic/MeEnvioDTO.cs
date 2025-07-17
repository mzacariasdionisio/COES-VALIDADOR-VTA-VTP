using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_ENVIO
    /// </summary>
    public partial class MeEnvioDTO : EntityBase
    {
        public int Enviocodi { get; set; }
        public DateTime? Enviofecha { get; set; }
        public DateTime? Enviofechaperiodo { get; set; }
        public DateTime? Enviofechaini { get; set; }
        public DateTime? Enviofechafin { get; set; }
        public int? Estenvcodi { get; set; }
        public int? Archcodi { get; set; }
        public string Envioplazo { get; set; }
        public string Userlogin { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public int? Emprcodi { get; set; }
        public int? Formatcodi { get; set; }
        public int? Cfgenvcodi { get; set; }

        public string Formatnombre { get; set; }
        public string Archnomborig { get; set; }
        public string Archnombpatron { get; set; }
        public string Lectnomb { get; set; }
        public string Emprnomb { get; set; }
        public string Username { get; set; }
        public string Usertlf { get; set; }
        public string Estenvnombre { get; set; }
        public string Periodo { get; set; }
        public string FechaPeriodo { get; set; }
        public int Indcumplimiento { get; set; }
        public int Formatperiodo { get; set; }
        public int? Modcodi { get; set; }
        public int? Fdatcodi { get; set; }

        /// <summary>
        /// Campos para aplicativo pr16
        /// </summary>        
        public int? EnvioCodiAct { get; set; }
        public int? EnvioCodiAnt { get; set; }
        public DateTime? EnvioFechaPeriodoAct { get; set; }
        public DateTime? EnvioFechaPeriodoAnt { get; set; }
        public DateTime? EnvioFechaIniAct { get; set; }
        public DateTime? EnvioFechaFinAct { get; set; }
        public DateTime? EnvioFechaIniAnt { get; set; }
        public DateTime? EnvioFechaFinAnt { get; set; }       
        public string Etiqueta { get; set; }
        public string UserEmail { get; set; }
        public int Item { get; set; }
        public string Cumplimiento { get; set; }
        public string TipoEmpresa { get; set; }
        public string RucEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public int NroEnvios { get; set; }
        public DateTime? FechaPrimerEnvio { get; set; }
        public DateTime? FechaUltimoEnvio { get; set; }
        public DateTime IniRemision { get; set; }
        public DateTime FinRemision { get; set; }
        public DateTime IniPeriodo { get; set; }

        //- Campos adicionados para reporte hidrologia
        public int Areacodi { get; set; }
        public string Areanomb { get; set; }
        public int Lectcodi { get; set; }
        public int Lectnro { get; set; }
        public int Lectperiodo { get; set; }
        public int? Formatresolucion { get; set; }
        public int Formathorizonte { get; set; }
        public int Ptomedicodi { get; set; }
        public string TipoInfoabrev { get; set; }
        public string Ptomedibarranomb { get; set; }
        public string Ptomedidesc { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public int Formatdiaplazo { get; set; }
        public int Formatminplazo { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Lecttipo { get; set; }
        public int Formatmesplazo { get; set; }
        public int Formatmesfinplazo { get; set; }
        public int Formatdiafinfueraplazo { get; set; }
        public int Formatminfinfueraplazo { get; set; }
        public int Formatdiafinplazo { get; set; }
        public int Formatminfinplazo { get; set; }
        public int Formatmesfinfueraplazo { get; set; }
        public int Formatcheckplazopunto { get; set; }

        public DateTime? Enviofechaplazoini { get; set; }
        public DateTime? Enviofechaplazofin { get; set; }
    }

    public partial class MeEnvioDTO
    {
        public string EnviofechaDesc { get; set; } = string.Empty;

        //- Modificado para PMPO
        public int Envionumbloques { get; set; }
        public int Envioorigen { get; set; }
        public bool TieneMensaje { get; set; }

        public int EnviocodiDerivoCOES { get; set; }
        public string UsuarioDerivo { get; set; } = string.Empty;
        public bool TieneDerivacion { get; set; }

        public string Comentario { get; set; }
        public int MensajesPendientes { get; set; }
        public double EnvioCumplimiento { get; set; }
        public int MensajesPendientesCOES { get; set; }

        public string Enviodesc { get; set; }
        public string EstenvcodiDesc { get; set; }

        public string IndNotifApertura { get; set; } = string.Empty;
        public string IndNotifPendiente { get; set; } = string.Empty;

        // Modificado para CTAF
        public string Enviofecha2 { get; set; }
        public string EnvioplazoDesc { get; set; }

        public int? Enviobloquehora { get; set; }
        public string TipoInforme { get; set; }
        public string EtapaInforme { get; set; }

        public int? Evencodi { get; set; }
        public string Evenasunto { get; set; }
        public DateTime? Evenini { get; set; }

        public int Envevencodi { get; set; }
        public string Eveinfrutaarchivo { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string TipoFalla { get; set; }


        #region Mejoras-RDO-II
        public int HorarioCodi { get; set; }
        #endregion
    }
}
