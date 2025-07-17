using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_VERSIONREP
    /// </summary>
    public class SpoVersionrepHelper : HelperBase
    {
        public SpoVersionrepHelper(): base(Consultas.SpoVersionrepSql)
        {
        }

        public SpoVersionrepDTO Create(IDataReader dr)
        {
            SpoVersionrepDTO entity = new SpoVersionrepDTO();

            int iVerrcodi = dr.GetOrdinal(this.Verrcodi);
            if (!dr.IsDBNull(iVerrcodi)) entity.Verrcodi = Convert.ToInt32(dr.GetValue(iVerrcodi));

            int iRepcodi = dr.GetOrdinal(this.Repcodi);
            if (!dr.IsDBNull(iRepcodi)) entity.Repcodi = Convert.ToInt32(dr.GetValue(iRepcodi));

            int iVerrfechaperiodo = dr.GetOrdinal(this.Verrfechaperiodo);
            if (!dr.IsDBNull(iVerrfechaperiodo)) entity.Verrfechaperiodo = dr.GetDateTime(iVerrfechaperiodo);

            int iVerrusucreacion = dr.GetOrdinal(this.Verrusucreacion);
            if (!dr.IsDBNull(iVerrusucreacion)) entity.Verrusucreacion = dr.GetString(iVerrusucreacion);

            int iVerrestado = dr.GetOrdinal(this.Verrestado);
            if (!dr.IsDBNull(iVerrestado)) entity.Verrestado = Convert.ToInt32(dr.GetValue(iVerrestado));

            int iVerrnro = dr.GetOrdinal(this.Verrnro);
            if (!dr.IsDBNull(iVerrnro)) entity.Verrnro = Convert.ToInt32(dr.GetValue(iVerrnro));

            int iVerrfeccreacion = dr.GetOrdinal(this.Verrfeccreacion);
            if (!dr.IsDBNull(iVerrfeccreacion)) entity.Verrfeccreacion = dr.GetDateTime(iVerrfeccreacion);

            int iVerrusumodificacion = dr.GetOrdinal(this.Verrusumodificacion);
            if (!dr.IsDBNull(iVerrusumodificacion)) entity.Verrusumodificacion = dr.GetString(iVerrusumodificacion);

            int iVerrfecmodificacion = dr.GetOrdinal(this.Verrfecmodificacion);
            if (!dr.IsDBNull(iVerrfecmodificacion)) entity.Verrfecmodificacion = dr.GetDateTime(iVerrfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Verrcodi = "VERRCODI";
        public string Repcodi = "REPCODI";
        public string Verrfechaperiodo = "VERRFECHAPERIODO";
        public string Verrusucreacion = "VERRUSUCREACION";
        public string Verrestado = "VERRESTADO";
        public string Verrnro = "VERRNRO";
        public string Verrfeccreacion = "VERRFECCREACION";
        public string Verrusumodificacion = "VERRUSUMODIFICACION";
        public string Verrfecmodificacion = "VERRFECMODIFICACION";
        #endregion

        public string SqlGetMaxIdVersion
        {
            get { return base.GetSqlXml("GetMaxIdVersion"); }
        }

        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }
    }
}
