using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_COSTO_INCREMENTAL
    /// </summary>
    public class CmCostoIncrementalHelper : HelperBase
    {
        public CmCostoIncrementalHelper(): base(Consultas.CmCostoIncrementalSql)
        {
        }

        public CmCostoIncrementalDTO Create(IDataReader dr)
        {
            CmCostoIncrementalDTO entity = new CmCostoIncrementalDTO();

            int iCmcicodi = dr.GetOrdinal(this.Cmcicodi);
            if (!dr.IsDBNull(iCmcicodi)) entity.Cmcicodi = Convert.ToInt32(dr.GetValue(iCmcicodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iCmcifecha = dr.GetOrdinal(this.Cmcifecha);
            if (!dr.IsDBNull(iCmcifecha)) entity.Cmcifecha = dr.GetDateTime(iCmcifecha);

            int iCmciperiodo = dr.GetOrdinal(this.Cmciperiodo);
            if (!dr.IsDBNull(iCmciperiodo)) entity.Cmciperiodo = Convert.ToInt32(dr.GetValue(iCmciperiodo));

            int iCmgncorrelativo = dr.GetOrdinal(this.Cmgncorrelativo);
            if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

            int iCmcitramo1 = dr.GetOrdinal(this.Cmcitramo1);
            if (!dr.IsDBNull(iCmcitramo1)) entity.Cmcitramo1 = dr.GetDecimal(iCmcitramo1);

            int iCmcitramo2 = dr.GetOrdinal(this.Cmcitramo2);
            if (!dr.IsDBNull(iCmcitramo2)) entity.Cmcitramo2 = dr.GetDecimal(iCmcitramo2);

            int iCmciusucreacion = dr.GetOrdinal(this.Cmciusucreacion);
            if (!dr.IsDBNull(iCmciusucreacion)) entity.Cmciusucreacion = dr.GetString(iCmciusucreacion);

            int iCmcifeccreacion = dr.GetOrdinal(this.Cmcifeccreacion);
            if (!dr.IsDBNull(iCmcifeccreacion)) entity.Cmcifeccreacion = dr.GetDateTime(iCmcifeccreacion);

            int iCmciusumodificacion = dr.GetOrdinal(this.Cmciusumodificacion);
            if (!dr.IsDBNull(iCmciusumodificacion)) entity.Cmciusumodificacion = dr.GetString(iCmciusumodificacion);

            int iCmcifecmodificacion = dr.GetOrdinal(this.Cmcifecmodificacion);
            if (!dr.IsDBNull(iCmcifecmodificacion)) entity.Cmcifecmodificacion = dr.GetDateTime(iCmcifecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmcicodi = "CMCICODI";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Cmcifecha = "CMCIFECHA";
        public string Cmciperiodo = "CMCIPERIODO";
        public string Cmgncorrelativo = "CMGNCORRELATIVO";
        public string Cmcitramo1 = "CMCITRAMO1";
        public string Cmcitramo2 = "CMCITRAMO2";
        public string Cmciusucreacion = "CMCIUSUCREACION";
        public string Cmcifeccreacion = "CMCIFECCREACION";
        public string Cmciusumodificacion = "CMCIUSUMODIFICACION";
        public string Cmcifecmodificacion = "CMCIFECMODIFICACION";
        public string Grupopadre = "GRUPOPADRE";
        public string Equinomb = "NOMBRETNA";
        #endregion
    }
}
