using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RI_HISTORICO
    /// </summary>
    public class RiHistoricoHelper : HelperBase
    {
        public RiHistoricoHelper(): base(Consultas.RiHistoricoSql)
        {
        }

        public RiHistoricoDTO Create(IDataReader dr)
        {
            RiHistoricoDTO entity = new RiHistoricoDTO();

            int iHisricodi = dr.GetOrdinal(this.Hisricodi);
            if (!dr.IsDBNull(iHisricodi)) entity.Hisricodi = Convert.ToInt32(dr.GetValue(iHisricodi));

            int iHisrianio = dr.GetOrdinal(this.Hisrianio);
            if (!dr.IsDBNull(iHisrianio)) entity.Hisrianio = Convert.ToInt32(dr.GetValue(iHisrianio));

            int iHisritipo = dr.GetOrdinal(this.Hisritipo);
            if (!dr.IsDBNull(iHisritipo)) entity.Hisritipo = dr.GetString(iHisritipo);

            int iHisridesc = dr.GetOrdinal(this.Hisridesc);
            if (!dr.IsDBNull(iHisridesc)) entity.Hisridesc = dr.GetString(iHisridesc);

            int iHisrifecha = dr.GetOrdinal(this.Hisrifecha);
            if (!dr.IsDBNull(iHisrifecha)) entity.Hisrifecha = dr.GetDateTime(iHisrifecha);

            int iHisriestado = dr.GetOrdinal(this.Hisriestado);
            if (!dr.IsDBNull(iHisriestado)) entity.Hisriestado = dr.GetString(iHisriestado);

            int iHisriusucreacion = dr.GetOrdinal(this.Hisriusucreacion);
            if (!dr.IsDBNull(iHisriusucreacion)) entity.Hisriusucreacion = dr.GetString(iHisriusucreacion);

            int iHisrifeccreacion = dr.GetOrdinal(this.Hisrifeccreacion);
            if (!dr.IsDBNull(iHisrifeccreacion)) entity.Hisrifeccreacion = dr.GetDateTime(iHisrifeccreacion);

            int iHisriusumodificacion = dr.GetOrdinal(this.Hisriusumodificacion);
            if (!dr.IsDBNull(iHisriusumodificacion)) entity.Hisriusumodificacion = dr.GetString(iHisriusumodificacion);

            int iHisrifecmodificacion = dr.GetOrdinal(this.Hisrifecmodificacion);
            if (!dr.IsDBNull(iHisrifecmodificacion)) entity.Hisrifecmodificacion = dr.GetDateTime(iHisrifecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Hisricodi = "HISRICODI";
        public string Hisrianio = "HISRIANIO";
        public string Hisritipo = "HISRITIPO";
        public string Hisridesc = "HISRIDESC";
        public string Hisrifecha = "HISRIFECHA";
        public string Hisriestado = "HISRIESTADO";
        public string Hisriusucreacion = "HISRIUSUCREACION";
        public string Hisrifeccreacion = "HISRIFECCREACION";
        public string Hisriusumodificacion = "HISRIUSUMODIFICACION";
        public string Hisrifecmodificacion = "HISRIFECMODIFICACION";

        #endregion

        public string SqlObtenerPorFecha
        {
            get { return GetSqlXml("ObtenerPorFecha"); }
        }
    }
}
