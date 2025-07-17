using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Xml.Serialization;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla SI_TABLA_AUDITABLE
    /// </summary>
    [XmlRoot("SI_TABLA_AUDITABLE")]
    public class SiTablaAuditableDTO : EntityBase
    {
        public int TauditCodi { get; set; } 
        public string TauditNomb { get; set; } 
        public int TauditTipAudit { get; set; } 
        public string TauditEstado { get; set; }
        public string TauditUsuarioCreacion { get; set; }
        public DateTime TauditFechaCreacion { get; set; }
        public string TauditUsuarioUpdate { get; set; }
        public DateTime TauditFechaUpdate { get; set; }

        #region FIT SGOCOES func A
        public string TauditDesc { get; set; }
        public string TauditQuery { get; set; }
        #endregion
    }

    #region FIT SGOCOES func A
    public class fwUserDTO
    {
        public int USERCODE { get; set; }
        public int AREACODE { get; set; }
        public string USERLOGIN { get; set; }
        public string USERNAME { get; set; }
        public string USEREMAIL { get; set; } //aplicativo Seg. Recomendaciones 
    }

    public class SiAuditoriaDTO
    {
        public int AUDITCODI { get; set; }
        public int AUDITREGCODI { get; set; }
        public int TAUDITCODI { get; set; }
        public string AUDITINDTIPO { get; set; }
        public string TAUDITNOMB { get; set; }
        public string TAUDITDESC { get; set; }
        public string TAUDITQUERY { get; set; }
    }

    public class SiAuditoriaHistorialDTO
    {
        public string TAUDITCONFCOLUMN { get; set; }
        public string TAUDITCONFDESC { get; set; }
        
    }
    public class SiAuditoriaXMLDTO
    {
        public string AUDITREGISTRO { get; set; }
        public DateTime AUDITREGISTRODATE { get; set; }
    }
    public class filtrosModelDTO
    {
        public int TauditCode { get; set; }
        public string UserCode { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        //public string Tipo { get; set; }
        public int IdRegistro { get; set; }
    }
    #endregion
}
