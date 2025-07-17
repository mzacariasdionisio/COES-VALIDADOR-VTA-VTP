using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_PERIODO
    /// </summary>
    public class PfrPeriodoHelper : HelperBase
    {
        public PfrPeriodoHelper(): base(Consultas.PfrPeriodoSql)
        {
        }

        public PfrPeriodoDTO Create(IDataReader dr)
        {
            PfrPeriodoDTO entity = new PfrPeriodoDTO();

            int iPfrpercodi = dr.GetOrdinal(this.Pfrpercodi);
            if (!dr.IsDBNull(iPfrpercodi)) entity.Pfrpercodi = Convert.ToInt32(dr.GetValue(iPfrpercodi));

            int iPfrpernombre = dr.GetOrdinal(this.Pfrpernombre);
            if (!dr.IsDBNull(iPfrpernombre)) entity.Pfrpernombre = dr.GetString(iPfrpernombre);

            int iPfrperanio = dr.GetOrdinal(this.Pfrperanio);
            if (!dr.IsDBNull(iPfrperanio)) entity.Pfrperanio = Convert.ToInt32(dr.GetValue(iPfrperanio));

            int iPfrpermes = dr.GetOrdinal(this.Pfrpermes);
            if (!dr.IsDBNull(iPfrpermes)) entity.Pfrpermes = Convert.ToInt32(dr.GetValue(iPfrpermes));

            int iPfrperaniomes = dr.GetOrdinal(this.Pfrperaniomes);
            if (!dr.IsDBNull(iPfrperaniomes)) entity.Pfrperaniomes = Convert.ToInt32(dr.GetValue(iPfrperaniomes));

            int iPfrperusucreacion = dr.GetOrdinal(this.Pfrperusucreacion);
            if (!dr.IsDBNull(iPfrperusucreacion)) entity.Pfrperusucreacion = dr.GetString(iPfrperusucreacion);

            int iPfrperfeccreacion = dr.GetOrdinal(this.Pfrperfeccreacion);
            if (!dr.IsDBNull(iPfrperfeccreacion)) entity.Pfrperfeccreacion = dr.GetDateTime(iPfrperfeccreacion);

            int iPfrperfecmodificacion = dr.GetOrdinal(this.Pfrperfecmodificacion);
            if (!dr.IsDBNull(iPfrperfecmodificacion)) entity.Pfrperfecmodificacion = dr.GetDateTime(iPfrperfecmodificacion);

            int iPfrperusumodificacion = dr.GetOrdinal(this.Pfrperusumodificacion);
            if (!dr.IsDBNull(iPfrperusumodificacion)) entity.Pfrperusumodificacion = dr.GetString(iPfrperusumodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrpercodi = "PFRPERCODI";
        public string Pfrpernombre = "PFRPERNOMBRE";
        public string Pfrperanio = "PFRPERANIO";
        public string Pfrpermes = "PFRPERMES";
        public string Pfrperaniomes = "PFRPERANIOMES";
        public string Pfrperusucreacion = "PFRPERUSUCREACION";
        public string Pfrperfeccreacion = "PFRPERFECCREACION";
        public string Pfrperfecmodificacion = "PFRPERFECMODIFICACION";
        public string Pfrperusumodificacion = "PFRPERUSUMODIFICACION";

        #endregion
    }
}
