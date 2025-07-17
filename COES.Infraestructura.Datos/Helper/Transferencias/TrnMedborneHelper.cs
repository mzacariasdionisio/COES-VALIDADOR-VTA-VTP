using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_MEDBORNE
    /// </summary>
    public class TrnMedborneHelper : HelperBase
    {
        public TrnMedborneHelper(): base(Consultas.TrnMedborneSql)
        {
        }

        public TrnMedborneDTO Create(IDataReader dr)
        {
            TrnMedborneDTO entity = new TrnMedborneDTO();

            int iTrnmebcodi = dr.GetOrdinal(this.Trnmebcodi);
            if (!dr.IsDBNull(iTrnmebcodi)) entity.Trnmebcodi = Convert.ToInt32(dr.GetValue(iTrnmebcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iTrnmebversion = dr.GetOrdinal(this.Trnmebversion);
            if (!dr.IsDBNull(iTrnmebversion)) entity.Trnmebversion = Convert.ToInt32(dr.GetValue(iTrnmebversion));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iTrnmebfecha = dr.GetOrdinal(this.Trnmebfecha);
            if (!dr.IsDBNull(iTrnmebfecha)) entity.Trnmebfecha = dr.GetDateTime(iTrnmebfecha);

            int iTrnmebptomed = dr.GetOrdinal(this.Trnmebptomed);
            if (!dr.IsDBNull(iTrnmebptomed)) entity.Trnmebptomed = dr.GetString(iTrnmebptomed);

            int iTrnmebenergia = dr.GetOrdinal(this.Trnmebenergia);
            if (!dr.IsDBNull(iTrnmebenergia)) entity.Trnmebenergia = dr.GetDecimal(iTrnmebenergia);

            int iTrnmebusucreacion = dr.GetOrdinal(this.Trnmebusucreacion);
            if (!dr.IsDBNull(iTrnmebusucreacion)) entity.Trnmebusucreacion = dr.GetString(iTrnmebusucreacion);

            int iTrnmebfeccreacion = dr.GetOrdinal(this.Trnmebfeccreacion);
            if (!dr.IsDBNull(iTrnmebfeccreacion)) entity.Trnmebfeccreacion = dr.GetDateTime(iTrnmebfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Trnmebcodi = "TRNMEBCODI";
        public string Pericodi = "PERICODI";
        public string Trnmebversion = "TRNMEBVERSION";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Trnmebfecha = "TRNMEBFECHA";
        public string Trnmebptomed = "TRNMEBPTOMED";
        public string Trnmebenergia = "TRNMEBENERGIA";
        public string Trnmebusucreacion = "TRNMEBUSUCREACION";
        public string Trnmebfeccreacion = "TRNMEBFECCREACION";

        #endregion
    }
}
