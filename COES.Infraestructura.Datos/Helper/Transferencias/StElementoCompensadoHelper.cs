using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_ELEMENTO_COMPENSADO
    /// </summary>
    public class StElementoCompensadoHelper : HelperBase
    {
        public StElementoCompensadoHelper(): base(Consultas.StElementoCompensadoSql)
        {
        }

        public StElementoCompensadoDTO Create(IDataReader dr)
        {
            StElementoCompensadoDTO entity = new StElementoCompensadoDTO();

            int iElecmpcodi = dr.GetOrdinal(this.Elecmpcodi);
            if (!dr.IsDBNull(iElecmpcodi)) entity.Elecmpcodi = Convert.ToInt32(dr.GetValue(iElecmpcodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iStfactcodi = dr.GetOrdinal(this.Stfactcodi);
            if (!dr.IsDBNull(iStfactcodi)) entity.Stfactcodi = Convert.ToInt32(dr.GetValue(iStfactcodi));

            int iStcompcodi = dr.GetOrdinal(this.Stcompcodi);
            if (!dr.IsDBNull(iStcompcodi)) entity.Stcompcodi = Convert.ToInt32(dr.GetValue(iStcompcodi));

            int iElecmpmonto = dr.GetOrdinal(this.Elecmpmonto);
            if (!dr.IsDBNull(iElecmpmonto)) entity.Elecmpmonto = dr.GetDecimal(iElecmpmonto);

            int iElecmpusucreacion = dr.GetOrdinal(this.Elecmpusucreacion);
            if (!dr.IsDBNull(iElecmpusucreacion)) entity.Elecmpusucreacion = dr.GetString(iElecmpusucreacion);

            int iElecmpfeccreacion = dr.GetOrdinal(this.Elecmpfeccreacion);
            if (!dr.IsDBNull(iElecmpfeccreacion)) entity.Elecmpfeccreacion = dr.GetDateTime(iElecmpfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Elecmpcodi = "ELECMPCODI";
        public string Strecacodi = "STRECACODI";
        public string Stfactcodi = "STFACTCODI";
        public string Stcompcodi = "STCOMPCODI";
        public string Elecmpmonto = "ELECMPMONTO";
        public string Elecmpusucreacion = "ELECMPUSUCREACION";
        public string Elecmpfeccreacion = "ELECMPFECCREACION";

        #endregion
    }
}
