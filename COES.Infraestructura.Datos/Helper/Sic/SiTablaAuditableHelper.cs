using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using SiAuditoriaDTO = COES.Dominio.DTO.Transferencias.SiAuditoriaDTO;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_TABLA_AUDITABLE
    /// </summary>
    public class SiTablaAuditableHelper : HelperBase
    {
        public SiTablaAuditableHelper()
            : base(Consultas.SiTablaAuditableSql)
        {
        }

        public SiTablaAuditableDTO Create(IDataReader dr)
        {
            SiTablaAuditableDTO entity = new SiTablaAuditableDTO();

            int iTauditcodi = dr.GetOrdinal(this.Tauditcodi);
            if (!dr.IsDBNull(iTauditcodi)) entity.TauditCodi = Convert.ToInt32(dr.GetValue(iTauditcodi));

            int iTauditnomb = dr.GetOrdinal(this.Tauditnomb);
            if (!dr.IsDBNull(iTauditnomb)) entity.TauditNomb = dr.GetString(iTauditnomb);

            int iTaudittipaudit = dr.GetOrdinal(this.Taudittipaudit);
            if (!dr.IsDBNull(iTaudittipaudit)) entity.TauditTipAudit = dr.GetInt32(iTaudittipaudit);

            int iTauditestado = dr.GetOrdinal(this.Tauditestado);
            if (!dr.IsDBNull(iTauditestado)) entity.TauditEstado = dr.GetString(iTauditestado);

            int iTauditusuariocreacion = dr.GetOrdinal(this.Tauditusuariocreacion);
            if (!dr.IsDBNull(iTauditusuariocreacion)) entity.TauditUsuarioCreacion = dr.GetString(iTauditusuariocreacion);

            int iTauditfechacreacion = dr.GetOrdinal(this.Tauditfechacreacion);
            if (!dr.IsDBNull(iTauditfechacreacion)) entity.TauditFechaCreacion = dr.GetDateTime(iTauditfechacreacion);

            int iTauditusuarioupdate = dr.GetOrdinal(this.Tauditusuarioupdate);
            if (!dr.IsDBNull(iTauditusuarioupdate)) entity.TauditUsuarioUpdate = dr.GetString(iTauditusuarioupdate);

            int iTauditfechaupdate = dr.GetOrdinal(this.Tauditfechaupdate);
            if (!dr.IsDBNull(iTauditfechaupdate)) entity.TauditFechaUpdate = dr.GetDateTime(iTauditfechaupdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Tauditcodi = "TAUDITCODI";
        public string Tauditnomb = "TAUDITNOMB";
        public string Taudittipaudit = "TAUDITTIPAUDIT";
        public string Tauditestado = "TAUDITESTADO";
        public string Tauditusuariocreacion = "TAUDITUSUARIOCREACION";
        public string Tauditfechacreacion = "TAUDITFECHACREACION";
        public string Tauditusuarioupdate = "TAUDITUSUARIOUPDATE";
        public string Tauditfechaupdate = "TAUDITFECHAUPDATE";
        public string usercode = "USERCODE";
        public string areacode = "AREACODE";
        public string userlogin = "USERLOGIN";
        public string username = "USERNAME";
        public string useremail = "USEREMAIL"; //aplicativo Seg. Recomendaciones 

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListUserRol
        {
            get { return base.GetSqlXml("ListUserRol"); }
        }

        public fwUserDTO CreateUser(IDataReader dr)
        {
            fwUserDTO entity = new fwUserDTO();

            int iusercode = dr.GetOrdinal(this.usercode);
            if (!dr.IsDBNull(iusercode)) entity.USERCODE = Convert.ToInt32(dr.GetValue(iusercode));

            int iareacode = dr.GetOrdinal(this.areacode);
            if (!dr.IsDBNull(iareacode)) entity.AREACODE = Convert.ToInt32(dr.GetValue(iareacode));

            int iuserlogin = dr.GetOrdinal(this.userlogin);
            if (!dr.IsDBNull(iuserlogin)) entity.USERLOGIN = dr.GetString(iuserlogin);

            int iusername = dr.GetOrdinal(this.username);
            if (!dr.IsDBNull(iusername)) entity.USERNAME = dr.GetString(iusername);
            
            #region Inicio aplicativo Seg. Recomendaciones
            int iuseremail = dr.GetOrdinal(this.useremail);
            if (!dr.IsDBNull(iuseremail)) entity.USEREMAIL = dr.GetString(iuseremail);
            #endregion Fin aplicativo Seg. Recomendaciones

            return entity;
        }

    }
}
