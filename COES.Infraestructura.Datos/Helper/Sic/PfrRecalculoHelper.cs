using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_RECALCULO
    /// </summary>
    public class PfrRecalculoHelper : HelperBase
    {
        public PfrRecalculoHelper(): base(Consultas.PfrRecalculoSql)
        {
        }

        public PfrRecalculoDTO Create(IDataReader dr)
        {
            PfrRecalculoDTO entity = new PfrRecalculoDTO();

            int iPfrrecfecmodificacion = dr.GetOrdinal(this.Pfrrecfecmodificacion);
            if (!dr.IsDBNull(iPfrrecfecmodificacion)) entity.Pfrrecfecmodificacion = dr.GetDateTime(iPfrrecfecmodificacion);

            int iPfrpercodi = dr.GetOrdinal(this.Pfrpercodi);
            if (!dr.IsDBNull(iPfrpercodi)) entity.Pfrpercodi = Convert.ToInt32(dr.GetValue(iPfrpercodi));

            int iPfrreccodi = dr.GetOrdinal(this.Pfrreccodi);
            if (!dr.IsDBNull(iPfrreccodi)) entity.Pfrreccodi = Convert.ToInt32(dr.GetValue(iPfrreccodi));

            int iPfrrecnombre = dr.GetOrdinal(this.Pfrrecnombre);
            if (!dr.IsDBNull(iPfrrecnombre)) entity.Pfrrecnombre = dr.GetString(iPfrrecnombre);

            int iPfrrecdescripcion = dr.GetOrdinal(this.Pfrrecdescripcion);
            if (!dr.IsDBNull(iPfrrecdescripcion)) entity.Pfrrecdescripcion = dr.GetString(iPfrrecdescripcion);

            int iPfrrecinforme = dr.GetOrdinal(this.Pfrrecinforme);
            if (!dr.IsDBNull(iPfrrecinforme)) entity.Pfrrecinforme = dr.GetString(iPfrrecinforme);

            int iPfrrectipo = dr.GetOrdinal(this.Pfrrectipo);
            if (!dr.IsDBNull(iPfrrectipo)) entity.Pfrrectipo = dr.GetString(iPfrrectipo);

            int iPfrrecfechalimite = dr.GetOrdinal(this.Pfrrecfechalimite);
            if (!dr.IsDBNull(iPfrrecfechalimite)) entity.Pfrrecfechalimite = dr.GetDateTime(iPfrrecfechalimite);

            int iPfrrecusucreacion = dr.GetOrdinal(this.Pfrrecusucreacion);
            if (!dr.IsDBNull(iPfrrecusucreacion)) entity.Pfrrecusucreacion = dr.GetString(iPfrrecusucreacion);

            int iPfrrecfeccreacion = dr.GetOrdinal(this.Pfrrecfeccreacion);
            if (!dr.IsDBNull(iPfrrecfeccreacion)) entity.Pfrrecfeccreacion = dr.GetDateTime(iPfrrecfeccreacion);

            int iPfrrecusumodificacion = dr.GetOrdinal(this.Pfrrecusumodificacion);
            if (!dr.IsDBNull(iPfrrecusumodificacion)) entity.Pfrrecusumodificacion = dr.GetString(iPfrrecusumodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrrecfecmodificacion = "PFRRECFECMODIFICACION";
        public string Pfrpercodi = "PFRPERCODI";
        public string Pfrreccodi = "PFRRECCODI";
        public string Pfrrecnombre = "PFRRECNOMBRE";
        public string Pfrrecdescripcion = "PFRRECDESCRIPCION";
        public string Pfrrecinforme = "PFRRECINFORME";
        public string Pfrrectipo = "PFRRECTIPO";
        public string Pfrrecfechalimite = "PFRRECFECHALIMITE";
        public string Pfrrecusucreacion = "PFRRECUSUCREACION";
        public string Pfrrecfeccreacion = "PFRRECFECCREACION";
        public string Pfrrecusumodificacion = "PFRRECUSUMODIFICACION";

        #endregion
    }
}
