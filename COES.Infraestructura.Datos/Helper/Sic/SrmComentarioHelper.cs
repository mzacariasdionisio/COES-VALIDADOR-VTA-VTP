using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SRM_COMENTARIO
    /// </summary>
    public class SrmComentarioHelper : HelperBase
    {
        public SrmComentarioHelper(): base(Consultas.SrmComentarioSql)
        {
        }

        public SrmComentarioDTO Create(IDataReader dr)
        {
            SrmComentarioDTO entity = new SrmComentarioDTO();

            int iSrmcomcodi = dr.GetOrdinal(this.Srmcomcodi);
            if (!dr.IsDBNull(iSrmcomcodi)) entity.Srmcomcodi = Convert.ToInt32(dr.GetValue(iSrmcomcodi));

            int iSrmreccodi = dr.GetOrdinal(this.Srmreccodi);
            if (!dr.IsDBNull(iSrmreccodi)) entity.Srmreccodi = Convert.ToInt32(dr.GetValue(iSrmreccodi));

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iSrmcomfechacoment = dr.GetOrdinal(this.Srmcomfechacoment);
            if (!dr.IsDBNull(iSrmcomfechacoment)) entity.Srmcomfechacoment = dr.GetDateTime(iSrmcomfechacoment);

            int iSrmcomgruporespons = dr.GetOrdinal(this.Srmcomgruporespons);
            if (!dr.IsDBNull(iSrmcomgruporespons)) entity.Srmcomgruporespons = dr.GetString(iSrmcomgruporespons);

            int iSrmcomcomentario = dr.GetOrdinal(this.Srmcomcomentario);
            if (!dr.IsDBNull(iSrmcomcomentario)) entity.Srmcomcomentario = dr.GetString(iSrmcomcomentario);

            int iSrmcomactivo = dr.GetOrdinal(this.Srmcomactivo);
            if (!dr.IsDBNull(iSrmcomactivo)) entity.Srmcomactivo = dr.GetString(iSrmcomactivo);

            int iSrmcomusucreacion = dr.GetOrdinal(this.Srmcomusucreacion);
            if (!dr.IsDBNull(iSrmcomusucreacion)) entity.Srmcomusucreacion = dr.GetString(iSrmcomusucreacion);

            int iSrmcomfeccreacion = dr.GetOrdinal(this.Srmcomfeccreacion);
            if (!dr.IsDBNull(iSrmcomfeccreacion)) entity.Srmcomfeccreacion = dr.GetDateTime(iSrmcomfeccreacion);

            int iSrmcomusumodificacion = dr.GetOrdinal(this.Srmcomusumodificacion);
            if (!dr.IsDBNull(iSrmcomusumodificacion)) entity.Srmcomusumodificacion = dr.GetString(iSrmcomusumodificacion);

            int iSrmcomfecmodificacion = dr.GetOrdinal(this.Srmcomfecmodificacion);
            if (!dr.IsDBNull(iSrmcomfecmodificacion)) entity.Srmcomfecmodificacion = dr.GetDateTime(iSrmcomfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Srmcomcodi = "SRMCOMCODI";
        public string Srmreccodi = "SRMRECCODI";
        public string Usercode = "USERCODE";
        public string Emprcodi = "EMPRCODI";
        public string Srmcomfechacoment = "SRMCOMFECHACOMENT";
        public string Srmcomgruporespons = "SRMCOMGRUPORESPONS";
        public string Srmcomcomentario = "SRMCOMCOMENTARIO";
        public string Srmcomactivo = "SRMCOMACTIVO";
        public string Srmcomusucreacion = "SRMCOMUSUCREACION";
        public string Srmcomfeccreacion = "SRMCOMFECCREACION";
        public string Srmcomusumodificacion = "SRMCOMUSUMODIFICACION";
        public string Srmcomfecmodificacion = "SRMCOMFECMODIFICACION";
        public string Srmrecfecharecomend = "SRMRECFECHARECOMEND";
        public string Username = "USERNAME";
        public string Emprnomb = "EMPRNOMB";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        #endregion
    }
}
