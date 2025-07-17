using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_ENERGIAUNIDAD_DET
    /// </summary>
    public class RerEnergiaUnidadDetHelper : HelperBase
    {
        public RerEnergiaUnidadDetHelper() : base(Consultas.RerEnergiaUnidadDetSql)
        {
        }

        #region Mapeo de Campos
        public string Rereudcodi = "REREUDCODI";
        public string Rereucodi = "REREUCODI";
        public string Rereudenergiaunidad = "REREUDENERGIAUNIDAD";
        #endregion

        public RerEnergiaUnidadDetDTO Create(IDataReader dr)
        {
            RerEnergiaUnidadDetDTO entity = new RerEnergiaUnidadDetDTO();

            int iRereudcodi = dr.GetOrdinal(this.Rereudcodi);
            if (!dr.IsDBNull(iRereudcodi)) entity.Rereudcodi = Convert.ToInt32(dr.GetValue(iRereudcodi));

            int iRereucodi = dr.GetOrdinal(this.Rereucodi);
            if (!dr.IsDBNull(iRereucodi)) entity.Rereucodi = Convert.ToInt32(dr.GetValue(iRereucodi));

            int iRereudenergiaunidad = dr.GetOrdinal(this.Rereudenergiaunidad);
            if (!dr.IsDBNull(iRereudenergiaunidad)) entity.Rereudenergiaunidad = dr.GetString(iRereudenergiaunidad);

            return entity;
        }


    }
}

