using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_POTLIM_UNIDAD
    /// </summary>
    public class IndPotlimUnidadHelper : HelperBase
    {
        public IndPotlimUnidadHelper(): base(Consultas.IndPotlimUnidadSql)
        {
        }

        public IndPotlimUnidadDTO Create(IDataReader dr)
        {
            IndPotlimUnidadDTO entity = new IndPotlimUnidadDTO();

            int iEqulimcodi = dr.GetOrdinal(this.Equlimcodi);
            if (!dr.IsDBNull(iEqulimcodi)) entity.Equlimcodi = Convert.ToInt32(dr.GetValue(iEqulimcodi));

            int iEqulimpotefectiva = dr.GetOrdinal(this.Equlimpotefectiva);
            if (!dr.IsDBNull(iEqulimpotefectiva)) entity.Equlimpotefectiva = dr.GetDecimal(iEqulimpotefectiva);

            int iEqulimusumodificacion = dr.GetOrdinal(this.Equlimusumodificacion);
            if (!dr.IsDBNull(iEqulimusumodificacion)) entity.Equlimusumodificacion = dr.GetString(iEqulimusumodificacion);

            int iEqulimfecmodificacion = dr.GetOrdinal(this.Equlimfecmodificacion);
            if (!dr.IsDBNull(iEqulimfecmodificacion)) entity.Equlimfecmodificacion = dr.GetDateTime(iEqulimfecmodificacion);

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iPotlimcodi = dr.GetOrdinal(this.Potlimcodi);
            if (!dr.IsDBNull(iPotlimcodi)) entity.Potlimcodi = Convert.ToInt32(dr.GetValue(iPotlimcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Equlimcodi = "EQULIMCODI";
        public string Equlimpotefectiva = "EQULIMPOTEFECTIVA";
        public string Equlimusumodificacion = "EQULIMUSUMODIFICACION";
        public string Equlimfecmodificacion = "EQULIMFECMODIFICACION";
        public string Equipadre = "EQUIPADRE";
        public string Potlimcodi = "POTLIMCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";

        #endregion
    }
}
