using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_SUBSCRIPCION
    /// </summary>
    public class WbSubscripcionHelper : HelperBase
    {
        public WbSubscripcionHelper(): base(Consultas.WbSubscripcionSql)
        {
        }

        public WbSubscripcionDTO Create(IDataReader dr)
        {
            WbSubscripcionDTO entity = new WbSubscripcionDTO();

            int iSubscripcodi = dr.GetOrdinal(this.Subscripcodi);
            if (!dr.IsDBNull(iSubscripcodi)) entity.Subscripcodi = Convert.ToInt32(dr.GetValue(iSubscripcodi));

            int iSubscripnombres = dr.GetOrdinal(this.Subscripnombres);
            if (!dr.IsDBNull(iSubscripnombres)) entity.Subscripnombres = dr.GetString(iSubscripnombres);

            int iSubscripapellidos = dr.GetOrdinal(this.Subscripapellidos);
            if (!dr.IsDBNull(iSubscripapellidos)) entity.Subscripapellidos = dr.GetString(iSubscripapellidos);

            int iSubscripemail = dr.GetOrdinal(this.Subscripemail);
            if (!dr.IsDBNull(iSubscripemail)) entity.Subscripemail = dr.GetString(iSubscripemail);

            int iSubscriptelefono = dr.GetOrdinal(this.Subscriptelefono);
            if (!dr.IsDBNull(iSubscriptelefono)) entity.Subscriptelefono = dr.GetString(iSubscriptelefono);

            int iSubscripempresa = dr.GetOrdinal(this.Subscripempresa);
            if (!dr.IsDBNull(iSubscripempresa)) entity.Subscripempresa = dr.GetString(iSubscripempresa);

            int iSubscripestado = dr.GetOrdinal(this.Subscripestado);
            if (!dr.IsDBNull(iSubscripestado)) entity.Subscripestado = dr.GetString(iSubscripestado);

            int iSubscripfecha = dr.GetOrdinal(this.Subscripfecha);
            if (!dr.IsDBNull(iSubscripfecha)) entity.Subscripfecha = dr.GetDateTime(iSubscripfecha);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Subscripcodi = "SUBSCRIPCODI";
        public string Subscripnombres = "SUBSCRIPNOMBRES";
        public string Subscripapellidos = "SUBSCRIPAPELLIDOS";
        public string Subscripemail = "SUBSCRIPEMAIL";
        public string Subscriptelefono = "SUBSCRIPTELEFONO";
        public string Subscripempresa = "SUBSCRIPEMPRESA";
        public string Subscripestado = "SUBSCRIPESTADO";
        public string Subscripfecha = "SUBSCRIPFECHA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Publicname = "PUBLICNOMBRE";

        public string SqlObtenerExportacion
        {
            get { return base.GetSqlXml("ObtenerExportacion"); }
        }

        #endregion
    }
}
