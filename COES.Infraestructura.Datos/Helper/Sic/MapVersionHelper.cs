using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MAP_VERSION
    /// </summary>
    public class MapVersionHelper : HelperBase
    {
        public MapVersionHelper(): base(Consultas.MapVersionSql)
        {
        }

        public MapVersionDTO Create(IDataReader dr)
        {
            MapVersionDTO entity = new MapVersionDTO();

            int iVermcodi = dr.GetOrdinal(this.Vermcodi);
            if (!dr.IsDBNull(iVermcodi)) entity.Vermcodi = Convert.ToInt32(dr.GetValue(iVermcodi));

            int iVermfechaperiodo = dr.GetOrdinal(this.Vermfechaperiodo);
            if (!dr.IsDBNull(iVermfechaperiodo)) entity.Vermfechaperiodo = dr.GetDateTime(iVermfechaperiodo);

            int iVermusucreacion = dr.GetOrdinal(this.Vermusucreacion);
            if (!dr.IsDBNull(iVermusucreacion)) entity.Vermusucreacion = dr.GetString(iVermusucreacion);

            int iVermestado = dr.GetOrdinal(this.Vermestado);
            if (!dr.IsDBNull(iVermestado)) entity.Vermestado = Convert.ToInt32(dr.GetValue(iVermestado));

            int iVermfeccreacion = dr.GetOrdinal(this.Vermfeccreacion);
            if (!dr.IsDBNull(iVermfeccreacion)) entity.Vermfeccreacion = dr.GetDateTime(iVermfeccreacion);

            int iVermusumodificacion = dr.GetOrdinal(this.Vermusumodificacion);
            if (!dr.IsDBNull(iVermusumodificacion)) entity.Vermusumodificacion = dr.GetString(iVermusumodificacion);

            int iVermfecmodificacion = dr.GetOrdinal(this.Vermfecmodificacion);
            if (!dr.IsDBNull(iVermfecmodificacion)) entity.Vermfecmodificacion = dr.GetDateTime(iVermfecmodificacion);

            int iVermnumero = dr.GetOrdinal(this.Vermnumero);
            if (!dr.IsDBNull(iVermnumero)) entity.Vermnumero = dr.GetInt16(iVermnumero);

            return entity;
        }


        #region Mapeo de Campos

        public string Vermcodi = "VERMCODI";
        public string Vermfechaperiodo = "VERMFECHAPERIODO";
        public string Vermusucreacion = "VERMUSUCREACION";
        public string Vermestado = "VERMESTADO";
        public string Vermfeccreacion = "VERMFECCREACION";
        public string Vermusumodificacion = "VERMUSUMODIFICACION";
        public string Vermfecmodificacion = "VERMFECMODIFICACION";
        public string Vermnumero = "VERMNUMERO";

        #endregion

        public string SqlGetMaxVermnumero
        {
            get { return GetSqlXml("GetMaxVermnumero"); }
        }
    }
}
