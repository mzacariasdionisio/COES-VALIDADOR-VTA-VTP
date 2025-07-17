using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_PROCESO_LOG
    /// </summary>
    public class SiProcesoLogHelper : HelperBase
    {
        public SiProcesoLogHelper(): base(Consultas.SiProcesoLogSql)
        {
        }

        public SiProcesoLogDTO Create(IDataReader dr)
        {
            SiProcesoLogDTO entity = new SiProcesoLogDTO();

            int iPrcslgcodi = dr.GetOrdinal(this.Prcslgcodi);
            if (!dr.IsDBNull(iPrcslgcodi)) entity.Prcslgcodi = Convert.ToInt32(dr.GetValue(iPrcslgcodi));

            int iPrcscodi = dr.GetOrdinal(this.Prcscodi);
            if (!dr.IsDBNull(iPrcscodi)) entity.Prcscodi = Convert.ToInt32(dr.GetValue(iPrcscodi));

            int iPrcslgfecha = dr.GetOrdinal(this.Prcslgfecha);
            if (!dr.IsDBNull(iPrcslgfecha)) entity.Prcslgfecha = dr.GetDateTime(iPrcslgfecha);

            int iPrcslginicio = dr.GetOrdinal(this.Prcslginicio);
            if (!dr.IsDBNull(iPrcslginicio)) entity.Prcslginicio = dr.GetDateTime(iPrcslginicio);

            int iPrcslgfin = dr.GetOrdinal(this.Prcslgfin);
            if (!dr.IsDBNull(iPrcslgfin)) entity.Prcslgfin = dr.GetDateTime(iPrcslgfin);

            int iPrcslgestado = dr.GetOrdinal(this.Prcslgestado);
            if (!dr.IsDBNull(iPrcslgestado)) entity.Prcslgestado = dr.GetString(iPrcslgestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Prcslgcodi = "PRCSLGCODI";
        public string Prcscodi = "PRCSCODI";
        public string Prcslgfecha = "PRCSLGFECHA";
        public string Prcslginicio = "PRCSLGINICIO";
        public string Prcslgfin = "PRCSLGFIN";
        public string Prcslgestado = "PRCSLGESTADO";

        #endregion
    }
}
