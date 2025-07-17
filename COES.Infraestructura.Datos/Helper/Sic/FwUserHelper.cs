using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FW_USER
    /// </summary>
    public class FwUserHelper : HelperBase
    {
        public FwUserHelper(): base(Consultas.FwUserSql)
        {
        }

        public FwUserDTO Create(IDataReader dr)
        {
            FwUserDTO entity = new FwUserDTO();

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));

            int iUsername = dr.GetOrdinal(this.Username);
            if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

            int iUserpass = dr.GetOrdinal(this.Userpass);
            if (!dr.IsDBNull(iUserpass)) entity.Userpass = dr.GetString(iUserpass);

            int iUserconn = dr.GetOrdinal(this.Userconn);
            if (!dr.IsDBNull(iUserconn)) entity.Userconn = Convert.ToInt32(dr.GetValue(iUserconn));

            int iUsermaxconn = dr.GetOrdinal(this.Usermaxconn);
            if (!dr.IsDBNull(iUsermaxconn)) entity.Usermaxconn = Convert.ToInt32(dr.GetValue(iUsermaxconn));

            int iUserlogin = dr.GetOrdinal(this.Userlogin);
            if (!dr.IsDBNull(iUserlogin)) entity.Userlogin = dr.GetString(iUserlogin);

            int iUservalidate = dr.GetOrdinal(this.Uservalidate);
            if (!dr.IsDBNull(iUservalidate)) entity.Uservalidate = Convert.ToInt32(dr.GetValue(iUservalidate));

            int iUsercheck = dr.GetOrdinal(this.Usercheck);
            if (!dr.IsDBNull(iUsercheck)) entity.Usercheck = Convert.ToInt32(dr.GetValue(iUsercheck));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iUserstate = dr.GetOrdinal(this.Userstate);
            if (!dr.IsDBNull(iUserstate)) entity.Userstate = dr.GetString(iUserstate);

            int iEmpresas = dr.GetOrdinal(this.Empresas);
            if (!dr.IsDBNull(iEmpresas)) entity.Empresas = dr.GetString(iEmpresas);

            int iUserfcreacion = dr.GetOrdinal(this.Userfcreacion);
            if (!dr.IsDBNull(iUserfcreacion)) entity.Userfcreacion = dr.GetDateTime(iUserfcreacion);

            int iUserfactivacion = dr.GetOrdinal(this.Userfactivacion);
            if (!dr.IsDBNull(iUserfactivacion)) entity.Userfactivacion = dr.GetDateTime(iUserfactivacion);

            int iUserfbaja = dr.GetOrdinal(this.Userfbaja);
            if (!dr.IsDBNull(iUserfbaja)) entity.Userfbaja = dr.GetDateTime(iUserfbaja);

            int iUsertlf = dr.GetOrdinal(this.Usertlf);
            if (!dr.IsDBNull(iUsertlf)) entity.Usertlf = dr.GetString(iUsertlf);

            int iMotivocontacto = dr.GetOrdinal(this.Motivocontacto);
            if (!dr.IsDBNull(iMotivocontacto)) entity.Motivocontacto = dr.GetString(iMotivocontacto);

            int iUsercargo = dr.GetOrdinal(this.Usercargo);
            if (!dr.IsDBNull(iUsercargo)) entity.Usercargo = dr.GetString(iUsercargo);

            int iArealaboral = dr.GetOrdinal(this.Arealaboral);
            if (!dr.IsDBNull(iArealaboral)) entity.Arealaboral = dr.GetString(iArealaboral);

            int iUseremail = dr.GetOrdinal(this.Useremail);
            if (!dr.IsDBNull(iUseremail)) entity.Useremail = dr.GetString(iUseremail);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iUsersolicitud = dr.GetOrdinal(this.Usersolicitud);
            if (!dr.IsDBNull(iUsersolicitud)) entity.Usersolicitud = dr.GetString(iUsersolicitud);

            int iUserindreprleg = dr.GetOrdinal(this.Userindreprleg);
            if (!dr.IsDBNull(iUserindreprleg)) entity.Userindreprleg = dr.GetString(iUserindreprleg);

            int iUserucreacion = dr.GetOrdinal(this.Userucreacion);
            if (!dr.IsDBNull(iUserucreacion)) entity.Userucreacion = dr.GetString(iUserucreacion);

            int iUserad = dr.GetOrdinal(this.Userad);
            if (!dr.IsDBNull(iUserad)) entity.Userad = dr.GetString(iUserad);

            int iUsermovil = dr.GetOrdinal(this.Usermovil);
            if (!dr.IsDBNull(iUsermovil)) entity.Usermovil = dr.GetString(iUsermovil);

            int iUserflagpermiso = dr.GetOrdinal(this.Userflagpermiso);
            if (!dr.IsDBNull(iUserflagpermiso)) entity.Userflagpermiso = Convert.ToInt32(dr.GetValue(iUserflagpermiso));

            int iUserdoc = dr.GetOrdinal(this.Userdoc);
            if (!dr.IsDBNull(iUserdoc)) entity.Userdoc = dr.GetString(iUserdoc);

            int iUserfecregistro = dr.GetOrdinal(this.Userfecregistro);
            if (!dr.IsDBNull(iUserfecregistro)) entity.Userfecregistro = dr.GetDateTime(iUserfecregistro);

            return entity;
        }


        #region Mapeo de Campos

        public string Usercode = "USERCODE";
        public string Areacode = "AREACODE";
        public string Username = "USERNAME";
        public string Userpass = "USERPASS";
        public string Userconn = "USERCONN";
        public string Usermaxconn = "USERMAXCONN";
        public string Userlogin = "USERLOGIN";
        public string Uservalidate = "USERVALIDATE";
        public string Usercheck = "USERCHECK";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Userstate = "USERSTATE";
        public string Empresas = "EMPRESAS";
        public string Userfcreacion = "USERFCREACION";
        public string Userfactivacion = "USERFACTIVACION";
        public string Userfbaja = "USERFBAJA";
        public string Usertlf = "USERTLF";
        public string Motivocontacto = "MOTIVOCONTACTO";
        public string Usercargo = "USERCARGO";
        public string Arealaboral = "AREALABORAL";
        public string Useremail = "USEREMAIL";
        public string Emprcodi = "EMPRCODI";
        public string Usersolicitud = "USERSOLICITUD";
        public string Userindreprleg = "USERINDREPRLEG";
        public string Userucreacion = "USERUCREACION";
        public string Userad = "USERAD";
        public string Usermovil = "USERMOVIL";
        public string Userflagpermiso = "USERFLAGPERMISO";
        public string Userdoc = "USERDOC";
        public string Userfecregistro = "USERFECREGISTRO";

        #endregion

        #region Ficha tecnica
        public string SqlObtenerCorreos
        {
            get { return base.GetSqlXml("ObtenerCorreos"); }
        }
        #endregion
    }
}
