using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_GRUPOXCNFBAR
    /// </summary>
    public class PrGrupoxcnfbarHelper : HelperBase
    {
        public PrGrupoxcnfbarHelper()
            : base(Consultas.PrGrupoxcnfbarSql)
        {
        }

        public PrGrupoxcnfbarDTO Create(IDataReader dr)
        {
            PrGrupoxcnfbarDTO entity = new PrGrupoxcnfbarDTO();

            int iGrcnfbcodi = dr.GetOrdinal(this.Grcnfbcodi);
            if (!dr.IsDBNull(iGrcnfbcodi)) entity.Grcnfbcodi = Convert.ToInt32(dr.GetValue(iGrcnfbcodi));

            int iCnfbarcodi = dr.GetOrdinal(this.Cnfbarcodi);
            if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGrcnfbestado = dr.GetOrdinal(this.Grcnfbestado);
            if (!dr.IsDBNull(iGrcnfbestado)) entity.Grcnfbestado = Convert.ToInt32(dr.GetValue(iGrcnfbestado));

            int iGrcnfbusucreacion = dr.GetOrdinal(this.Grcnfbusucreacion);
            if (!dr.IsDBNull(iGrcnfbusucreacion)) entity.Grcnfbusucreacion = dr.GetString(iGrcnfbusucreacion);

            int iGrcnfbfeccreacion = dr.GetOrdinal(this.Grcnfbfeccreacion);
            if (!dr.IsDBNull(iGrcnfbfeccreacion)) entity.Grcnfbfeccreacion = dr.GetDateTime(iGrcnfbfeccreacion);

            int iGrcnfbusumodificacion = dr.GetOrdinal(this.Grcnfbusumodificacion);
            if (!dr.IsDBNull(iGrcnfbusumodificacion)) entity.Grcnfbusumodificacion = dr.GetString(iGrcnfbusumodificacion);

            int iGrcnfbfecmodificacion = dr.GetOrdinal(this.Grcnfbfecmodificacion);
            if (!dr.IsDBNull(iGrcnfbfecmodificacion)) entity.Grcnfbfecmodificacion = dr.GetDateTime(iGrcnfbfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Grcnfbcodi = "GRCNFBCODI";
        public string Cnfbarcodi = "CNFBARCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Grcnfbestado = "GRCNFBESTADO";
        public string Grcnfbusucreacion = "GRCNFBUSUCREACION";
        public string Grcnfbfeccreacion = "GRCNFBFECCREACION";
        public string Grcnfbusumodificacion = "GRCNFBUSUMODIFICACION";
        public string Grcnfbfecmodificacion = "GRCNFBFECMODIFICACION";

        #endregion

        #region Mapeo de Consultas

        public string SqlGetByGrupocodi
        {
            get { return base.GetSqlXml("GetByGrupocodi"); }
        }

        #endregion
    }
}
