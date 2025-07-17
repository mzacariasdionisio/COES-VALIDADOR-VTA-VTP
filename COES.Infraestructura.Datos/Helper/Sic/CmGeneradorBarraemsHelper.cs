using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_GENERADOR_BARRAEMS
    /// </summary>
    public class CmGeneradorBarraemsHelper : HelperBase
    {
        public CmGeneradorBarraemsHelper(): base(Consultas.CmGeneradorBarraemsSql)
        {
        }

        public CmGeneradorBarraemsDTO Create(IDataReader dr)
        {
            CmGeneradorBarraemsDTO entity = new CmGeneradorBarraemsDTO();

            int iGenbarcodi = dr.GetOrdinal(this.Genbarcodi);
            if (!dr.IsDBNull(iGenbarcodi)) entity.Genbarcodi = Convert.ToInt32(dr.GetValue(iGenbarcodi));

            int iRelacioncodi = dr.GetOrdinal(this.Relacioncodi);
            if (!dr.IsDBNull(iRelacioncodi)) entity.Relacioncodi = Convert.ToInt32(dr.GetValue(iRelacioncodi));

            int iCnfbarcodi = dr.GetOrdinal(this.Cnfbarcodi);
            if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));

            int iGenbarusucreacion = dr.GetOrdinal(this.Genbarusucreacion);
            if (!dr.IsDBNull(iGenbarusucreacion)) entity.Genbarusucreacion = dr.GetString(iGenbarusucreacion);

            int iGenbarfeccreacion = dr.GetOrdinal(this.Genbarfeccreacion);
            if (!dr.IsDBNull(iGenbarfeccreacion)) entity.Genbarfeccreacion = dr.GetDateTime(iGenbarfeccreacion);

            int iGenbarusumodificacion = dr.GetOrdinal(this.Genbarusumodificacion);
            if (!dr.IsDBNull(iGenbarusumodificacion)) entity.Genbarusumodificacion = dr.GetString(iGenbarusumodificacion);

            int iGenbarfecmodificacion = dr.GetOrdinal(this.Genbarfecmodificacion);
            if (!dr.IsDBNull(iGenbarfecmodificacion)) entity.Genbarfecmodificacion = dr.GetDateTime(iGenbarfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Genbarcodi = "GENBARCODI";
        public string Relacioncodi = "RELACIONCODI";
        public string Cnfbarcodi = "CNFBARCODI";
        public string Genbarusucreacion = "GENBARUSUCREACION";
        public string Genbarfeccreacion = "GENBARFECCREACION";
        public string Genbarusumodificacion = "GENBARUSUMODIFICACION";
        public string Genbarfecmodificacion = "GENBARFECMODIFICACION";
        public string Cnfbarnombre = "CNFBARNOMBRE";

        #endregion
    }
}
