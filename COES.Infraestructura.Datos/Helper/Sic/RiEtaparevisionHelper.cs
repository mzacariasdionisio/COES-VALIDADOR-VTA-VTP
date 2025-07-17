using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RI_ETAPAREVISION
    /// </summary>
    public class RiEtaparevisionHelper : HelperBase
    {
        public RiEtaparevisionHelper(): base(Consultas.RiEtaparevisionSql)
        {
        }

        public RiEtaparevisionDTO Create(IDataReader dr)
        {
            RiEtaparevisionDTO entity = new RiEtaparevisionDTO();

            int iEtrvcodi = dr.GetOrdinal(this.Etrvcodi);
            if (!dr.IsDBNull(iEtrvcodi)) entity.Etrvcodi = Convert.ToInt32(dr.GetValue(iEtrvcodi));

            int iEtrvnombre = dr.GetOrdinal(this.Etrvnombre);
            if (!dr.IsDBNull(iEtrvnombre)) entity.Etrvnombre = dr.GetString(iEtrvnombre);

            int iEtrvestado = dr.GetOrdinal(this.Etrvestado);
            if (!dr.IsDBNull(iEtrvestado)) entity.Etrvestado = dr.GetString(iEtrvestado);

            int iEtrvusucreacion = dr.GetOrdinal(this.Etrvusucreacion);
            if (!dr.IsDBNull(iEtrvusucreacion)) entity.Etrvusucreacion = dr.GetString(iEtrvusucreacion);

            int iEtrvfeccreacion = dr.GetOrdinal(this.Etrvfeccreacion);
            if (!dr.IsDBNull(iEtrvfeccreacion)) entity.Etrvfeccreacion = dr.GetDateTime(iEtrvfeccreacion);

            int iEtrvusumodificacion = dr.GetOrdinal(this.Etrvusumodificacion);
            if (!dr.IsDBNull(iEtrvusumodificacion)) entity.Etrvusumodificacion = dr.GetString(iEtrvusumodificacion);

            int iEtrvfecmodificacion = dr.GetOrdinal(this.Etrvfecmodificacion);
            if (!dr.IsDBNull(iEtrvfecmodificacion)) entity.Etrvfecmodificacion = dr.GetDateTime(iEtrvfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Etrvcodi = "ETRVCODI";
        public string Etrvnombre = "ETRVNOMBRE";
        public string Etrvestado = "ETRVESTADO";
        public string Etrvusucreacion = "ETRVUSUCREACION";
        public string Etrvfeccreacion = "ETRVFECCREACION";
        public string Etrvusumodificacion = "ETRVUSUMODIFICACION";
        public string Etrvfecmodificacion = "ETRVFECMODIFICACION";

        #endregion
    }
}
