using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_USUARIO_URS
    /// </summary>
    public class SmaUsuarioUrsHelper : HelperBase
    {
        public SmaUsuarioUrsHelper(): base(Consultas.SmaUsuarioUrsSql)
        {
        }

        public SmaUsuarioUrsDTO Create(IDataReader dr)
        {
            SmaUsuarioUrsDTO entity = new SmaUsuarioUrsDTO();

            int iUurscodi = dr.GetOrdinal(this.Uurscodi);
            if (!dr.IsDBNull(iUurscodi)) entity.Uurscodi = Convert.ToInt32(dr.GetValue(iUurscodi));

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iUursusucreacion = dr.GetOrdinal(this.Uursusucreacion);
            if (!dr.IsDBNull(iUursusucreacion)) entity.Uursusucreacion = dr.GetString(iUursusucreacion);

            int iUursusumodificacion = dr.GetOrdinal(this.Uursusumodificacion);
            if (!dr.IsDBNull(iUursusumodificacion)) entity.Uursusumodificacion = dr.GetString(iUursusumodificacion);

            int iUursfecmodificacion = dr.GetOrdinal(this.Uursfecmodificacion);
            if (!dr.IsDBNull(iUursfecmodificacion)) entity.Uursfecmodificacion = dr.GetDateTime(iUursfecmodificacion);

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iUursestado = dr.GetOrdinal(this.Uursestado);
            if (!dr.IsDBNull(iUursestado)) entity.Uursestado = dr.GetString(iUursestado);

            int iUursfeccreacion = dr.GetOrdinal(this.Uursfeccreacion);
            if (!dr.IsDBNull(iUursfeccreacion)) entity.Uursfeccreacion = dr.GetDateTime(iUursfeccreacion);

// JOIN
            int iUrsnomb = dr.GetOrdinal(this.Ursnomb);
            if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

            int iUrstipo = dr.GetOrdinal(this.Urstipo);
            if (!dr.IsDBNull(iUrstipo)) entity.Urstipo = dr.GetString(iUrstipo);

            int iGruponom = dr.GetOrdinal(this.Gruponom);
            if (!dr.IsDBNull(iGruponom)) entity.Gruponom = dr.GetString(iGruponom);

            int iGrupotipo = dr.GetOrdinal(this.Grupotipo);
            if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iUsername = dr.GetOrdinal(this.Username);
            if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

            return entity;
        }


        public SmaUsuarioUrsDTO CreateList(IDataReader dr)
        {
            SmaUsuarioUrsDTO entity = new SmaUsuarioUrsDTO();

            int iUurscodi = dr.GetOrdinal(this.Uurscodi);
            if (!dr.IsDBNull(iUurscodi)) entity.Uurscodi = Convert.ToInt32(dr.GetValue(iUurscodi));

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iUursusucreacion = dr.GetOrdinal(this.Uursusucreacion);
            if (!dr.IsDBNull(iUursusucreacion)) entity.Uursusucreacion = dr.GetString(iUursusucreacion);

            int iUursusumodificacion = dr.GetOrdinal(this.Uursusumodificacion);
            if (!dr.IsDBNull(iUursusumodificacion)) entity.Uursusumodificacion = dr.GetString(iUursusumodificacion);

            int iUursfecmodificacion = dr.GetOrdinal(this.Uursfecmodificacion);
            if (!dr.IsDBNull(iUursfecmodificacion)) entity.Uursfecmodificacion = dr.GetDateTime(iUursfecmodificacion);

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iUursestado = dr.GetOrdinal(this.Uursestado);
            if (!dr.IsDBNull(iUursestado)) entity.Uursestado = dr.GetString(iUursestado);

            int iUursfeccreacion = dr.GetOrdinal(this.Uursfeccreacion);
            if (!dr.IsDBNull(iUursfeccreacion)) entity.Uursfeccreacion = dr.GetDateTime(iUursfeccreacion);

            // JOIN
            int iUrsnomb = dr.GetOrdinal(this.Ursnomb);
            if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

            int iUrstipo = dr.GetOrdinal(this.Urstipo);
            if (!dr.IsDBNull(iUrstipo)) entity.Urstipo = dr.GetString(iUrstipo);

            int iUsername = dr.GetOrdinal(this.Username);
            if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

            int iUseremail = dr.GetOrdinal(this.Useremail);
            if (!dr.IsDBNull(iUseremail)) entity.Useremail = dr.GetString(iUseremail);

            return entity;
        }


        #region Mapeo de Campos

        public string Uurscodi = "UURSCODI";
        public string Urscodi = "URSCODI";
        public string Uursusucreacion = "UURSUSUCREACION";
        public string Uursusumodificacion = "UURSUSUMODIFICACION";
        public string Uursfecmodificacion = "UURSFECMODIFICACION";
        public string Usercode = "USERCODE";
        public string Uursestado = "UURSESTADO";
        public string Uursfeccreacion = "UURSFECCREACION";

        // JOIN
        public string Ursnomb = "URSNOMB";
        public string Urstipo = "URSTIPO";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponom = "GRUPONOMB";
        public string Grupotipo = "GRUPOTIPO";
        public string Username = "USERNAME";
        public string Userlogin = "USERLOGIN";
        public string Useremail = "USEREMAIL";


        #endregion

        public string SqlGetByCriteriaMO
        {
            get { return base.GetSqlXml("GetByCriteriaMO"); }
        }

        public string SqlListGetUsuUrsAct
        {
            get { return base.GetSqlXml("ListGetUsuUrsAct"); }
        }


        public string SqlUpdateUsuAct
        {
            get { return base.GetSqlXml("UpdateUsuAct"); }
        }
    
    }
}
