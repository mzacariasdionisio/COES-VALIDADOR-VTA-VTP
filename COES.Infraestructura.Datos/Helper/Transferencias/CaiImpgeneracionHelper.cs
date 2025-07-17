using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_IMPGENERACION
    /// </summary>
    public class CaiImpgeneracionHelper : HelperBase
    {
        public CaiImpgeneracionHelper(): base(Consultas.CaiImpgeneracionSql)
        {
        }

        public CaiImpgeneracionDTO Create(IDataReader dr)
        {
            CaiImpgeneracionDTO entity = new CaiImpgeneracionDTO();

            int iCaimpgcodi = dr.GetOrdinal(this.Caimpgcodi);
            if (!dr.IsDBNull(iCaimpgcodi)) entity.Caimpgcodi = Convert.ToInt32(dr.GetValue(iCaimpgcodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iCaimpgfuentedat = dr.GetOrdinal(this.Caimpgfuentedat);
            if (!dr.IsDBNull(iCaimpgfuentedat)) entity.Caimpgfuentedat = dr.GetString(iCaimpgfuentedat);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iCaimpgmes = dr.GetOrdinal(this.Caimpgmes);
            if (!dr.IsDBNull(iCaimpgmes)) entity.Caimpgmes = Convert.ToInt32(dr.GetValue(iCaimpgmes));

            int iCaimpgtotenergia = dr.GetOrdinal(this.Caimpgtotenergia);
            if (!dr.IsDBNull(iCaimpgtotenergia)) entity.Caimpgtotenergia = dr.GetDecimal(iCaimpgtotenergia);

            int iCaimpgimpenergia = dr.GetOrdinal(this.Caimpgimpenergia);
            if (!dr.IsDBNull(iCaimpgimpenergia)) entity.Caimpgimpenergia = dr.GetDecimal(iCaimpgimpenergia);

            int iCaimpgtotpotencia = dr.GetOrdinal(this.Caimpgtotpotencia);
            if (!dr.IsDBNull(iCaimpgtotpotencia)) entity.Caimpgtotpotencia = dr.GetDecimal(iCaimpgtotpotencia);

            int iCaimpgimppotencia = dr.GetOrdinal(this.Caimpgimppotencia);
            if (!dr.IsDBNull(iCaimpgimppotencia)) entity.Caimpgimppotencia = dr.GetDecimal(iCaimpgimppotencia);

            int iCaimpgusucreacion = dr.GetOrdinal(this.Caimpgusucreacion);
            if (!dr.IsDBNull(iCaimpgusucreacion)) entity.Caimpgusucreacion = dr.GetString(iCaimpgusucreacion);

            int iCaimpgfeccreacion = dr.GetOrdinal(this.Caimpgfeccreacion);
            if (!dr.IsDBNull(iCaimpgfeccreacion)) entity.Caimpgfeccreacion = dr.GetDateTime(iCaimpgfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Caimpgcodi = "CAIMPGCODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Caimpgfuentedat = "CAIMPGFUENTEDAT";
        public string Emprcodi = "EMPRCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Caimpgmes = "CAIMPGMES";
        public string Caimpgtotenergia = "CAIMPGTOTENERGIA";
        public string Caimpgimpenergia = "CAIMPGIMPENERGIA";
        public string Caimpgtotpotencia = "CAIMPGTOTPOTENCIA";
        public string Caimpgimppotencia = "CAIMPGIMPPOTENCIA";
        public string Caimpgusucreacion = "CAIMPGUSUCREACION";
        public string Caimpgfeccreacion = "CAIMPGFECCREACION";

        #endregion

        
    }
}
