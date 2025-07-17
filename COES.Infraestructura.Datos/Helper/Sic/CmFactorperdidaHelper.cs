using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_FACTORPERDIDA
    /// </summary>
    public class CmFactorperdidaHelper : HelperBase
    {
        public CmFactorperdidaHelper(): base(Consultas.CmFactorperdidaSql)
        {
        }

        public CmFactorperdidaDTO Create(IDataReader dr)
        {
            CmFactorperdidaDTO entity = new CmFactorperdidaDTO();

            int iCmfpmcodi = dr.GetOrdinal(this.Cmfpmcodi);
            if (!dr.IsDBNull(iCmfpmcodi)) entity.Cmfpmcodi = Convert.ToInt32(dr.GetValue(iCmfpmcodi));

            int iCmpercodi = dr.GetOrdinal(this.Cmpercodi);
            if (!dr.IsDBNull(iCmpercodi)) entity.Cmpercodi = Convert.ToInt32(dr.GetValue(iCmpercodi));

            int iCmfpmfecha = dr.GetOrdinal(this.Cmfpmfecha);
            if (!dr.IsDBNull(iCmfpmfecha)) entity.Cmfpmfecha = dr.GetDateTime(iCmfpmfecha);

            int iCmfpmestado = dr.GetOrdinal(this.Cmfpmestado);
            if (!dr.IsDBNull(iCmfpmestado)) entity.Cmfpmestado = dr.GetString(iCmfpmestado);

            int iCmfpmusucreacion = dr.GetOrdinal(this.Cmfpmusucreacion);
            if (!dr.IsDBNull(iCmfpmusucreacion)) entity.Cmfpmusucreacion = dr.GetString(iCmfpmusucreacion);

            int iCmfpmfeccreacion = dr.GetOrdinal(this.Cmfpmfeccreacion);
            if (!dr.IsDBNull(iCmfpmfeccreacion)) entity.Cmfpmfeccreacion = dr.GetDateTime(iCmfpmfeccreacion);

            int iCmfpmusumodificacion = dr.GetOrdinal(this.Cmfpmusumodificacion);
            if (!dr.IsDBNull(iCmfpmusumodificacion)) entity.Cmfpmusumodificacion = dr.GetString(iCmfpmusumodificacion);

            int iCmfpmfecmodificacion = dr.GetOrdinal(this.Cmfpmfecmodificacion);
            if (!dr.IsDBNull(iCmfpmfecmodificacion)) entity.Cmfpmfecmodificacion = dr.GetDateTime(iCmfpmfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmfpmcodi = "CMFPMCODI";
        public string Cmpercodi = "CMPERCODI";
        public string Cmfpmfecha = "CMFPMFECHA";
        public string Cmfpmestado = "CMFPMESTADO";
        public string Cmfpmusucreacion = "CMFPMUSUCREACION";
        public string Cmfpmfeccreacion = "CMFPMFECCREACION";
        public string Cmfpmusumodificacion = "CMFPMUSUMODIFICACION";
        public string Cmfpmfecmodificacion = "CMFPMFECMODIFICACION";

        #endregion
    }
}
