using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCE_GRUPO_EXCLUIDO
    /// </summary>
    public class VceGrupoExcluidoHelper : HelperBase
    {
        public VceGrupoExcluidoHelper()
            : base(Consultas.VceGrupoExcluidoSql)
        {
        }

        public VceGrupoExcluidoDTO Create(IDataReader dr)
        {
            VceGrupoExcluidoDTO entity = new VceGrupoExcluidoDTO();

            int iCrgexccodi = dr.GetOrdinal(this.Crgexccodi);
            if (!dr.IsDBNull(iCrgexccodi)) entity.Crgexccodi = Convert.ToInt32(dr.GetValue(iCrgexccodi));

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.Pecacodi = Convert.ToInt32(dr.GetValue(iPecacodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));


            int iCrgexusucreacion = dr.GetOrdinal(this.Crgexcusucreacion);
            if (!dr.IsDBNull(iCrgexusucreacion)) entity.Crgexcusucreacion = dr.GetString(iCrgexusucreacion);

            int iCrgexcfeccreacion = dr.GetOrdinal(this.Crgexcfeccreacion);
            if (!dr.IsDBNull(iCrgexcfeccreacion)) entity.Crgexcfeccreacion = dr.GetDateTime(iCrgexcfeccreacion);

            int iCrgexcusumodificacion = dr.GetOrdinal(this.Crgexcusumodificacion);
            if (!dr.IsDBNull(iCrgexcusumodificacion)) entity.Crgexcusumodificacion = dr.GetString(iCrgexcusumodificacion);

            int iCrgexcfecmodificacion = dr.GetOrdinal(this.Crgexcfecmodificacion);
            if (!dr.IsDBNull(iCrgexcfecmodificacion)) entity.Crgexcfecmodificacion = dr.GetDateTime(iCrgexcfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Crgexccodi = "CRGEXCCODI";
        public string Pecacodi = "PECACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Crgexcusucreacion = "CRGEXCUSUCREACION";
        public string Crgexcfeccreacion = "CRGEXCFECCREACION";
        public string Crgexcusumodificacion = "CRGEXCUSUMODIFICACION";
        public string Crgexcfecmodificacion = "CRGEXCFECMODIFICACION";
      

        #endregion

        
        public string SqlDeleteByVersion
        {
            get { return base.GetSqlXml("DeleteByVersion"); }
        }
    }
}
