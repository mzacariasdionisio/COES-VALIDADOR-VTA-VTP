using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_ENTIDAD_DAT
    /// </summary>
    public class PfrEntidadDatHelper : HelperBase
    {
        public PfrEntidadDatHelper(): base(Consultas.PfrEntidadDatSql)
        {
        }

        public PfrEntidadDatDTO Create(IDataReader dr)
        {
            PfrEntidadDatDTO entity = new PfrEntidadDatDTO();

            int iPfrentcodi = dr.GetOrdinal(this.Pfrentcodi);
            if (!dr.IsDBNull(iPfrentcodi)) entity.Pfrentcodi = Convert.ToInt32(dr.GetValue(iPfrentcodi));

            int iPfrcnpcodi = dr.GetOrdinal(this.Pfrcnpcodi);
            if (!dr.IsDBNull(iPfrcnpcodi)) entity.Pfrcnpcodi = Convert.ToInt32(dr.GetValue(iPfrcnpcodi));

            int iPrfdatfechavig = dr.GetOrdinal(this.Prfdatfechavig);
            if (!dr.IsDBNull(iPrfdatfechavig)) entity.Prfdatfechavig = dr.GetDateTime(iPrfdatfechavig);

            int iPfrdatdeleted = dr.GetOrdinal(this.Pfrdatdeleted);
            if (!dr.IsDBNull(iPfrdatdeleted)) entity.Pfrdatdeleted = Convert.ToInt32(dr.GetValue(iPfrdatdeleted));

            int iPfrdatvalor = dr.GetOrdinal(this.Pfrdatvalor);
            if (!dr.IsDBNull(iPfrdatvalor)) entity.Pfrdatvalor = dr.GetString(iPfrdatvalor);

            int iPfrdatfeccreacion = dr.GetOrdinal(this.Pfrdatfeccreacion);
            if (!dr.IsDBNull(iPfrdatfeccreacion)) entity.Pfrdatfeccreacion = dr.GetDateTime(iPfrdatfeccreacion);

            int iPfrdatusucreacion = dr.GetOrdinal(this.Pfrdatusucreacion);
            if (!dr.IsDBNull(iPfrdatusucreacion)) entity.Pfrdatusucreacion = dr.GetString(iPfrdatusucreacion);

            int iPfrdatfecmodificacion = dr.GetOrdinal(this.Pfrdatfecmodificacion);
            if (!dr.IsDBNull(iPfrdatfecmodificacion)) entity.Pfrdatfecmodificacion = dr.GetDateTime(iPfrdatfecmodificacion);

            int iPfrdatusumodificacion = dr.GetOrdinal(this.Pfrdatusumodificacion);
            if (!dr.IsDBNull(iPfrdatusumodificacion)) entity.Pfrdatusumodificacion = dr.GetString(iPfrdatusumodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrentcodi = "PFRENTCODI";
        public string Pfrcnpcodi = "PFRCNPCODI";
        public string Prfdatfechavig = "PRFDATFECHAVIG";
        public string Pfrdatdeleted = "PFRDATDELETED";
        public string Pfrdatvalor = "PFRDATVALOR";
        public string Pfrdatfeccreacion = "PFRDATFECCREACION";
        public string Pfrdatusucreacion = "PFRDATUSUCREACION";
        public string Pfrdatfecmodificacion = "PFRDATFECMODIFICACION";
        public string Pfrdatusumodificacion = "PFRDATUSUMODIFICACION";

        public string Pfrdatdeleted2 = "PFRDATDELETED2";
        public string Pfrcnpnomb = "PFRCNPNOMB";
        public string Pfrcatcodi = "PFRCATCODI";

        #endregion

        #region Sentencias SQL
        public string SqlListarPfrentidadVigente
        {
            get { return GetSqlXml("ListarPfrentidadVigente"); }
        }

        #endregion
    }
}
