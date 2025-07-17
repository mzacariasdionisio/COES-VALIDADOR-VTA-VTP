using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRAGRUPO
    /// </summary>
    public class SiMigragrupoHelper : HelperBase
    {
        public SiMigragrupoHelper(): base(Consultas.SiMigragrupoSql)
        {
        }

        public SiMigragrupoDTO Create(IDataReader dr)
        {
            SiMigragrupoDTO entity = new SiMigragrupoDTO();

            int iMiggrucodi = dr.GetOrdinal(this.Miggrucodi);
            if (!dr.IsDBNull(iMiggrucodi)) entity.Miggrucodi = Convert.ToInt32(dr.GetValue(iMiggrucodi));

            int iMigempcodi = dr.GetOrdinal(this.Migempcodi);
            if (!dr.IsDBNull(iMigempcodi)) entity.Migempcodi = Convert.ToInt32(dr.GetValue(iMigempcodi));

            int iGrupocodimigra = dr.GetOrdinal(this.Grupocodimigra);
            if (!dr.IsDBNull(iGrupocodimigra)) entity.Grupocodimigra = Convert.ToInt32(dr.GetValue(iGrupocodimigra));

            int iGrupocodibajanuevo = dr.GetOrdinal(this.Grupocodibajanuevo);
            if (!dr.IsDBNull(iGrupocodibajanuevo)) entity.Grupocodibajanuevo = Convert.ToInt32(dr.GetValue(iGrupocodibajanuevo));

            int iMiggruusucreacion = dr.GetOrdinal(this.Miggruusucreacion);
            if (!dr.IsDBNull(iMiggruusucreacion)) entity.Miggruusucreacion = dr.GetString(iMiggruusucreacion);

            int iMiggrufeccreacion = dr.GetOrdinal(this.Miggrufeccreacion);
            if (!dr.IsDBNull(iMiggrufeccreacion)) entity.Miggrufeccreacion = dr.GetDateTime(iMiggrufeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Miggrucodi = "MIGGRUCODI";
        public string Migempcodi = "MIGEMPCODI";
        public string Grupocodimigra = "GRUPOCODIMIGRA";
        public string Grupocodibajanuevo = "GRUPOCODIBAJANUEVO";
        public string Miggruusucreacion = "MIGGRUUSUCREACION";
        public string Miggrufeccreacion = "MIGGRUFECCREACION";

        #endregion
    }
}
