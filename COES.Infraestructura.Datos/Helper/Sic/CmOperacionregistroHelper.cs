using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_OPERACIONREGISTRO
    /// </summary>
    public class CmOperacionregistroHelper : HelperBase
    {
        public CmOperacionregistroHelper(): base(Consultas.CmOperacionregistroSql)
        {
        }

        public CmOperacionregistroDTO Create(IDataReader dr)
        {
            CmOperacionregistroDTO entity = new CmOperacionregistroDTO();

            int iOperegcodi = dr.GetOrdinal(this.Operegcodi);
            if (!dr.IsDBNull(iOperegcodi)) entity.Operegcodi = Convert.ToInt32(dr.GetValue(iOperegcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iOperegfecinicio = dr.GetOrdinal(this.Operegfecinicio);
            if (!dr.IsDBNull(iOperegfecinicio)) entity.Operegfecinicio = dr.GetDateTime(iOperegfecinicio);

            int iOperegfecfin = dr.GetOrdinal(this.Operegfecfin);
            if (!dr.IsDBNull(iOperegfecfin)) entity.Operegfecfin = dr.GetDateTime(iOperegfecfin);

            int iOperegusucreacion = dr.GetOrdinal(this.Operegusucreacion);
            if (!dr.IsDBNull(iOperegusucreacion)) entity.Operegusucreacion = dr.GetString(iOperegusucreacion);

            int iOperegfeccreacion = dr.GetOrdinal(this.Operegfeccreacion);
            if (!dr.IsDBNull(iOperegfeccreacion)) entity.Operegfeccreacion = dr.GetDateTime(iOperegfeccreacion);

            int iOperegusumodificacion = dr.GetOrdinal(this.Operegusumodificacion);
            if (!dr.IsDBNull(iOperegusumodificacion)) entity.Operegusumodificacion = dr.GetString(iOperegusumodificacion);

            int iOperegfecmodificacion = dr.GetOrdinal(this.Operegfecmodificacion);
            if (!dr.IsDBNull(iOperegfecmodificacion)) entity.Operegfecmodificacion = dr.GetDateTime(iOperegfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Operegcodi = "OPEREGCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Operegfecinicio = "OPEREGFECINICIO";
        public string Operegfecfin = "OPEREGFECFIN";
        public string Operegusucreacion = "OPEREGUSUCREACION";
        public string Operegfeccreacion = "OPEREGFECCREACION";
        public string Operegusumodificacion = "OPEREGUSUMODIFICACION";
        public string Operegfecmodificacion = "OPEREGFECMODIFICACION";
        public string Gruponomb = "GRUPONOMB";
        public string Subcausadesc = "SUBCAUSADESC";

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
