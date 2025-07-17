using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_LOGMIGRA
    /// </summary>
    public class SiLogmigraHelper : HelperBase
    {
        public SiLogmigraHelper(): base(Consultas.SiLogmigraSql)
        {
        }

        public SiLogmigraDTO Create(IDataReader dr)
        {
            SiLogmigraDTO entity = new SiLogmigraDTO();

            int iMigracodi = dr.GetOrdinal(this.Migracodi);
            if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));

            int iLogcodi = dr.GetOrdinal(this.Logcodi);
            if (!dr.IsDBNull(iLogcodi)) entity.Logcodi = Convert.ToInt32(dr.GetValue(iLogcodi));

            int iLogmigusucreacion = dr.GetOrdinal(this.Logmigusucreacion);
            if (!dr.IsDBNull(iLogmigusucreacion)) entity.Logmigusucreacion = dr.GetString(iLogmigusucreacion);

            int iLogmigfeccreacion = dr.GetOrdinal(this.Logmigfeccreacion);
            if (!dr.IsDBNull(iLogmigfeccreacion)) entity.Logmigfeccreacion = dr.GetDateTime(iLogmigfeccreacion);

            int iLogmigtipo = dr.GetOrdinal(this.Logmigtipo);
            if (!dr.IsDBNull(iLogmigtipo)) entity.Logmigtipo = Convert.ToInt32(dr.GetValue(iLogmigtipo));

            int iMiqubacodi = dr.GetOrdinal(this.Miqubacodi);
            if (!dr.IsDBNull(iMiqubacodi)) entity.Miqubacodi = Convert.ToInt32(dr.GetValue(iMiqubacodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Migracodi = "MIGRACODI";
        public string Logcodi = "LOGCODI";
        public string Logmigusucreacion = "LOGMIGUSUCREACION";
        public string Logmigfeccreacion = "LOGMIGFECCREACION";
        public string Logmigtipo = "LOGMIGTIPO";
        public string Miqubacodi = "MIQUBACODI";

        #endregion
    }
}
