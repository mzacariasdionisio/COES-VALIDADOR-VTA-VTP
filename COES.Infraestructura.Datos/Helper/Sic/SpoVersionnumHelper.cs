using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_VERSIONNUM
    /// </summary>
    public class SpoVersionnumHelper : HelperBase
    {
        public SpoVersionnumHelper(): base(Consultas.SpoVersionnumSql)
        {
        }

        public SpoVersionnumDTO Create(IDataReader dr)
        {
            SpoVersionnumDTO entity = new SpoVersionnumDTO();

            int iVerncodi = dr.GetOrdinal(this.Verncodi);
            if (!dr.IsDBNull(iVerncodi)) entity.Verncodi = Convert.ToInt32(dr.GetValue(iVerncodi));

            int iNumecodi = dr.GetOrdinal(this.Numecodi);
            if (!dr.IsDBNull(iNumecodi)) entity.Numecodi = Convert.ToInt32(dr.GetValue(iNumecodi));

            int iVernfechaperiodo = dr.GetOrdinal(this.Vernfechaperiodo);
            if (!dr.IsDBNull(iVernfechaperiodo)) entity.Vernfechaperiodo = dr.GetDateTime(iVernfechaperiodo);

            int iVernusucreacion = dr.GetOrdinal(this.Vernusucreacion);
            if (!dr.IsDBNull(iVernusucreacion)) entity.Vernusucreacion = dr.GetString(iVernusucreacion);

            int iVernestado = dr.GetOrdinal(this.Vernestado);
            if (!dr.IsDBNull(iVernestado)) entity.Vernestado = Convert.ToInt32(dr.GetValue(iVernestado));

            int iVernnro = dr.GetOrdinal(this.Vernnro);
            if (!dr.IsDBNull(iVernnro)) entity.Vernnro = Convert.ToInt32(dr.GetValue(iVernnro));

            int iVernfeccreacion = dr.GetOrdinal(this.Vernfeccreacion);
            if (!dr.IsDBNull(iVernfeccreacion)) entity.Vernfeccreacion = dr.GetDateTime(iVernfeccreacion);

            int iVernusumodificacion = dr.GetOrdinal(this.Vernusumodificacion);
            if (!dr.IsDBNull(iVernusumodificacion)) entity.Vernusumodificacion = dr.GetString(iVernusumodificacion);

            int iVernfecmodificacion = dr.GetOrdinal(this.Vernfecmodificacion);
            if (!dr.IsDBNull(iVernfecmodificacion)) entity.Vernfecmodificacion = dr.GetDateTime(iVernfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Verncodi = "VERNCODI";
        public string Numecodi = "NUMECODI";
        public string Vernfechaperiodo = "VERNFECHAPERIODO";
        public string Vernusucreacion = "VERNUSUCREACION";
        public string Vernestado = "VERNESTADO";
        public string Vernnro = "VERNNRO";
        public string Vernfeccreacion = "VERNFECCREACION";
        public string Vernusumodificacion = "VERNUSUMODIFICACION";
        public string Vernfecmodificacion = "VERNFECMODIFICACION";

        public string Numhisabrev = "NUMHISABREV";

        #endregion

        public string SqlGetMaxIdVersion
        {
            get { return base.GetSqlXml("GetMaxIdVersion"); }
        }

        public string SqlUpdateEstado
        {
            get { return GetSqlXml("UpdateEstado"); }
        }
    }
}
