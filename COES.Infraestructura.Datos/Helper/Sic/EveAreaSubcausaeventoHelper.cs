using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_AREA_SUBCAUSAEVENTO
    /// </summary>
    public class EveAreaSubcausaeventoHelper : HelperBase
    {
        public EveAreaSubcausaeventoHelper()
            : base(Consultas.EveAreaSubcausaeventoSql)
        {
        }

        public EveAreaSubcausaeventoDTO Create(IDataReader dr)
        {
            EveAreaSubcausaeventoDTO entity = new EveAreaSubcausaeventoDTO();

            int iArscaucodi = dr.GetOrdinal(this.Arscaucodi);
            if (!dr.IsDBNull(iArscaucodi)) entity.Arscaucodi = Convert.ToInt32(dr.GetValue(iArscaucodi));

            int iArscauusucreacion = dr.GetOrdinal(this.Arscauusucreacion);
            if (!dr.IsDBNull(iArscauusucreacion)) entity.Arscauusucreacion = dr.GetString(iArscauusucreacion);

            int iArscaufeccreacion = dr.GetOrdinal(this.Arscaufeccreacion);
            if (!dr.IsDBNull(iArscaufeccreacion)) entity.Arscaufeccreacion = dr.GetDateTime(iArscaufeccreacion);

            int iArscauusumodificacion = dr.GetOrdinal(this.Arscauusumodificacion);
            if (!dr.IsDBNull(iArscauusumodificacion)) entity.Arscauusumodificacion = dr.GetString(iArscauusumodificacion);

            int iArscaufecmodificacion = dr.GetOrdinal(this.Arscaufecmodificacion);
            if (!dr.IsDBNull(iArscaufecmodificacion)) entity.Arscaufecmodificacion = dr.GetDateTime(iArscaufecmodificacion);

            int iArscauactivo = dr.GetOrdinal(this.Arscauactivo);
            if (!dr.IsDBNull(iArscauactivo)) entity.Arscauactivo = Convert.ToInt32(dr.GetValue(iArscauactivo));

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));

            return entity;
        }

        #region Mapeo de Campos

        public string Arscaucodi = "ARSCAUCODI";
        public string Arscauusucreacion = "ARSCAUUSUCREACION";
        public string Arscaufeccreacion = "ARSCAUFECCREACION";
        public string Arscauusumodificacion = "ARSCAUUSUMODIFICACION";
        public string Arscaufecmodificacion = "ARSCAUFECMODIFICACION";
        public string Arscauactivo = "ARSCAUACTIVO";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Areacode = "AREACODE";

        #endregion

        public string SqlListarSubcausacodiRegistrados
        {
            get { return base.GetSqlXml("ListarSubcausacodiRegistrados"); }
        }

        public string SqlListBySubcausacodi
        {
            get { return base.GetSqlXml("ListBySubcausacodi"); }
        }
    }
}
