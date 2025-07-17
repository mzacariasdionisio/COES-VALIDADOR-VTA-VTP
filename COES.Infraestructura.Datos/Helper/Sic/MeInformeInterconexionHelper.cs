using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_INFORME_INTERCONEXION
    /// </summary>
    public class MeInformeInterconexionHelper : HelperBase
    {
        public MeInformeInterconexionHelper(): base(Consultas.MeInformeInterconexionSql)
        {
        }

        public MeInformeInterconexionDTO Create(IDataReader dr)
        {
            MeInformeInterconexionDTO entity = new MeInformeInterconexionDTO();

            int iInfintcodi = dr.GetOrdinal(this.Infintcodi);
            if (!dr.IsDBNull(iInfintcodi)) entity.Infintcodi = Convert.ToInt32(dr.GetValue(iInfintcodi));

            int iInfintanio = dr.GetOrdinal(this.Infintanio);
            if (!dr.IsDBNull(iInfintanio)) entity.Infintanio = Convert.ToInt32(dr.GetValue(iInfintanio));

            int iInfintnrosemana = dr.GetOrdinal(this.Infintnrosemana);
            if (!dr.IsDBNull(iInfintnrosemana)) entity.Infintnrosemana = Convert.ToInt32(dr.GetValue(iInfintnrosemana));

            int iInfintversion = dr.GetOrdinal(this.Infintversion);
            if (!dr.IsDBNull(iInfintversion)) entity.Infintversion = Convert.ToInt32(dr.GetValue(iInfintversion));

            int iInfintestado = dr.GetOrdinal(this.Infintestado);
            if (!dr.IsDBNull(iInfintestado)) entity.Infintestado = dr.GetString(iInfintestado);

            int iInfintusucreacion = dr.GetOrdinal(this.Infintusucreacion);
            if (!dr.IsDBNull(iInfintusucreacion)) entity.Infintusucreacion = dr.GetString(iInfintusucreacion);

            int iInfintfeccreacion = dr.GetOrdinal(this.Infintfeccreacion);
            if (!dr.IsDBNull(iInfintfeccreacion)) entity.Infintfeccreacion = dr.GetDateTime(iInfintfeccreacion);

            int iInfintusumodificacion = dr.GetOrdinal(this.Infintusumodificacion);
            if (!dr.IsDBNull(iInfintusumodificacion)) entity.Infintusumodificacion = dr.GetString(iInfintusumodificacion);

            int iInfintfecmodificacion = dr.GetOrdinal(this.Infintfecmodificacion);
            if (!dr.IsDBNull(iInfintfecmodificacion)) entity.Infintfecmodificacion = dr.GetDateTime(iInfintfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Infintcodi = "INFINTCODI";
        public string Infintanio = "INFINTANIO";
        public string Infintnrosemana = "INFINTNROSEMANA";
        public string Infintversion = "INFINTVERSION";
        public string Infintestado = "INFINTESTADO";
        public string Infintusucreacion = "INFINTUSUCREACION";
        public string Infintfeccreacion = "INFINTFECCREACION";
        public string Infintusumodificacion = "INFINTUSUMODIFICACION";
        public string Infintfecmodificacion = "INFINTFECMODIFICACION";

        #endregion
    }
}
