using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Xml.Linq;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_AUDITORIA_REGISTRO
    /// </summary>
    public class SiAuditoriaRegistroHelper : HelperBase
    {
        public SiAuditoriaRegistroHelper(): base(Consultas.SiAuditoriaRegistroSql)
        {
        }

        public SiAuditoriaRegistroDTO Create(IDataReader dr)
        {
            SiAuditoriaRegistroDTO entity = new SiAuditoriaRegistroDTO();



            int iAUDITCODI = dr.GetOrdinal(this.AUDITCODI);
            if (!dr.IsDBNull(iAUDITCODI)) entity.AuditCodi = Convert.ToInt32(dr.GetValue(iAUDITCODI));

            int iTAUDITCODI = dr.GetOrdinal(this.TAUDITCODI);
            if (!dr.IsDBNull(iTAUDITCODI)) entity.TauditCodi = Convert.ToInt32(dr.GetValue(iTAUDITCODI));

            int iAUDITNOMBRESISTEMA = dr.GetOrdinal(this.AUDITNOMBRESISTEMA);
            if (!dr.IsDBNull(iAUDITNOMBRESISTEMA)) entity.AuditNombreSistema = dr.GetString(iAUDITNOMBRESISTEMA);

            int iAUDITTABLACODI = dr.GetOrdinal(this.AUDITTABLACODI);
            if (!dr.IsDBNull(iAUDITTABLACODI)) entity.AuditTablaCodi = Convert.ToInt32(dr.GetValue(iAUDITTABLACODI));

            int iAUDITREGDET = dr.GetOrdinal(this.AUDITREGDET);
            if (!dr.IsDBNull(iAUDITREGDET)) entity.AuditRegDet = dr.GetString(iAUDITREGDET);

            int iAUDITUSUARIOCREACION = dr.GetOrdinal(this.AUDITUSUARIOCREACION);
            if (!dr.IsDBNull(iAUDITUSUARIOCREACION)) entity.AuditUsuarioCreacion = dr.GetString(iAUDITUSUARIOCREACION);

            int iAUDITFECHACREACION = dr.GetOrdinal(this.AUDITFECHACREACION);
            if (!dr.IsDBNull(iAUDITFECHACREACION)) entity.AuditFechaCreacion = dr.GetDateTime(iAUDITFECHACREACION);

            int iAUDITUSUARIOUPDATE = dr.GetOrdinal(this.AUDITUSUARIOUPDATE);
            if (!dr.IsDBNull(iAUDITUSUARIOUPDATE)) entity.AuditUsuarioUpdate = dr.GetString(iAUDITUSUARIOUPDATE);

            int iAUDITFECHAUPDATE = dr.GetOrdinal(this.AUDITFECHAUPDATE);
            if (!dr.IsDBNull(iAUDITFECHAUPDATE)) entity.AuditFechaUpdate = dr.GetDateTime(iAUDITFECHAUPDATE);
            

            return entity;
        }


        #region Mapeo de Campos

        public string AUDITUSUARIOCREACION = "AUDITUSUARIOCREACION";
        public string AUDITFECHACREACION = "AUDITFECHACREACION";
        public string AUDITUSUARIOUPDATE = "AUDITUSUARIOUPDATE";
        public string AUDITFECHAUPDATE = "AUDITFECHAUPDATE";
        public string AUDITCODI = "AUDITCODI";
        public string AUDITREGDET = "AUDITREGDET";
        public string AUDITTABLACODI = "AUDITTABLACODI";
        public string TAUDITCODI = "TAUDITCODI";
        public string AUDITNOMBRESISTEMA = "AUDITNOMBRESISTEMA";
        
        #endregion
        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }
        public string SqlListPaginado
        {
            get { return base.GetSqlXml("ListPaginado"); }
        }
        public string SqlGetByUsuariosAuditoria
        {
            get { return base.GetSqlXml("GetByUsuariosAuditoria"); }
        }
        
    }
}
