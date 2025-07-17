using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_VOLUMEN_INSENSIBILIDAD
    /// </summary>
    public class CmVolumenInsensibilidadHelper : HelperBase
    {
        public CmVolumenInsensibilidadHelper(): base(Consultas.CmVolumenInsensibilidadSql)
        {
        }

        public CmVolumenInsensibilidadDTO Create(IDataReader dr)
        {
            CmVolumenInsensibilidadDTO entity = new CmVolumenInsensibilidadDTO();

            int iVolinscodi = dr.GetOrdinal(this.Volinscodi);
            if (!dr.IsDBNull(iVolinscodi)) entity.Volinscodi = Convert.ToInt32(dr.GetValue(iVolinscodi));

            int iVolinsfecha = dr.GetOrdinal(this.Volinsfecha);
            if (!dr.IsDBNull(iVolinsfecha)) entity.Volinsfecha = dr.GetDateTime(iVolinsfecha);

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iVolinsvolmin = dr.GetOrdinal(this.Volinsvolmin);
            if (!dr.IsDBNull(iVolinsvolmin)) entity.Volinsvolmin = dr.GetDecimal(iVolinsvolmin);

            int iVolinsvolmax = dr.GetOrdinal(this.Volinsvolmax);
            if (!dr.IsDBNull(iVolinsvolmax)) entity.Volinsvolmax = dr.GetDecimal(iVolinsvolmax);

            int iVolinsinicio = dr.GetOrdinal(this.Volinsinicio);
            if (!dr.IsDBNull(iVolinsinicio)) entity.Volinsinicio = dr.GetDateTime(iVolinsinicio);

            int iVolinsfin = dr.GetOrdinal(this.Volinsfin);
            if (!dr.IsDBNull(iVolinsfin)) entity.Volinsfin = dr.GetDateTime(iVolinsfin);

            int iVolinsusucreacion = dr.GetOrdinal(this.Volinsusucreacion);
            if (!dr.IsDBNull(iVolinsusucreacion)) entity.Volinsusucreacion = dr.GetString(iVolinsusucreacion);

            int iVolinsfecreacion = dr.GetOrdinal(this.Volinsfecreacion);
            if (!dr.IsDBNull(iVolinsfecreacion)) entity.Volinsfecreacion = dr.GetDateTime(iVolinsfecreacion);

            int iVolinsusumodificacion = dr.GetOrdinal(this.Volinsusumodificacion);
            if (!dr.IsDBNull(iVolinsusumodificacion)) entity.Volinsusumodificacion = dr.GetString(iVolinsusumodificacion);

            int iVolinsfecmodificacion = dr.GetOrdinal(this.Volinsfecmodificacion);
            if (!dr.IsDBNull(iVolinsfecmodificacion)) entity.Volinsfecmodificacion = dr.GetDateTime(iVolinsfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Volinscodi = "VOLINSCODI";
        public string Volinsfecha = "VOLINSFECHA";
        public string Recurcodi = "RECURCODI";
        public string Topcodi = "TOPCODI";
        public string Volinsvolmin = "VOLINSVOLMIN";
        public string Volinsvolmax = "VOLINSVOLMAX";
        public string Volinsinicio = "VOLINSINICIO";
        public string Volinsfin = "VOLINSFIN";
        public string Volinsusucreacion = "VOLINSUSUCREACION";
        public string Volinsfecreacion = "VOLINSFECREACION";
        public string Volinsusumodificacion = "VOLINSUSUMODIFICACION";
        public string Volinsfecmodificacion = "VOLINSFECMODIFICACION";

        #endregion

        public string SqlObtenerRegistros
        {
            get { return base.GetSqlXml("ObtenerRegistros"); }
        }
    }
}
