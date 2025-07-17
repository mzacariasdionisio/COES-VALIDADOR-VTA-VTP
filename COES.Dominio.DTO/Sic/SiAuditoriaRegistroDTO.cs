using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_AUDITORIA_REGISTRO
    /// </summary>
    [XmlRoot("SI_AUDITORIA_REGISTRO")]
    public class SiAuditoriaRegistroDTO : EntityBase
    {
        public int AuditCodi { get; set; } 
        public int TauditCodi {get; set;}
        public string AuditNombreSistema {get; set;}
        public int AuditTablaCodi {get; set;}
        public string AuditRegDet {get;set;}
        public string AuditUsuarioCreacion {get;set;}
        public DateTime AuditFechaCreacion {get;set;}
        public string AuditUsuarioUpdate {get;set;}
        public DateTime AuditFechaUpdate {get;set;}
    }
}
