using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MP_REL_RECURSO_PTO
    /// </summary>
    public class MpRelRecursoPtoHelper : HelperBase
    {
        public MpRelRecursoPtoHelper(): base(Consultas.MpRelRecursoPtoSql)
        {
        }

        public MpRelRecursoPtoDTO Create(IDataReader dr)
        {
            MpRelRecursoPtoDTO entity = new MpRelRecursoPtoDTO();

            int iMtopcodi = dr.GetOrdinal(this.Mtopcodi);
            if (!dr.IsDBNull(iMtopcodi)) entity.Mtopcodi = Convert.ToInt32(dr.GetValue(iMtopcodi));

            int iMrecurcodi = dr.GetOrdinal(this.Mrecurcodi);
            if (!dr.IsDBNull(iMrecurcodi)) entity.Mrecurcodi = Convert.ToInt32(dr.GetValue(iMrecurcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iMrptohorizonte = dr.GetOrdinal(this.Mrptohorizonte);
            if (!dr.IsDBNull(iMrptohorizonte)) entity.Mrptohorizonte = dr.GetString(iMrptohorizonte);

            int iTptomedicodi = dr.GetOrdinal(this.Tptomedicodi);
            if (!dr.IsDBNull(iTptomedicodi)) entity.Tptomedicodi = Convert.ToInt32(dr.GetValue(iTptomedicodi));

            int iMrptofactor = dr.GetOrdinal(this.Mrptofactor);
            if (!dr.IsDBNull(iMrptofactor)) entity.Mrptofactor = dr.GetDecimal(iMrptofactor);

            int iMrptoformato = dr.GetOrdinal(this.Mrptoformato);
            if (!dr.IsDBNull(iMrptoformato)) entity.Mrptoformato = Convert.ToInt32(dr.GetValue(iMrptoformato));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int IMrptovolumen = dr.GetOrdinal(this.Mrptovolumen);
            if (!dr.IsDBNull(IMrptovolumen)) entity.Mrptovolumen = Convert.ToDecimal(dr.GetValue(IMrptovolumen));

            return entity;
        }

        #region Mapeo de Campos

        public string Mtopcodi = "MTOPCODI";
        public string Mrecurcodi = "MRECURCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Lectcodi = "LECTCODI";
        public string Mrptohorizonte = "MRPTOHORIZONTE";        
        public string Tptomedicodi = "TPTOMEDICODI";
        public string Mrptofactor = "MRPTOFACTOR";
        public string Mrptoformato = "MRPTOFORMATO";
        public string Mrptovolumen = "Mrptovolumen";
        public string Equicodi = "EQUICODI";

        public string Equinomb = "EQUINOMB";
        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string SqlListarByTopologia
        {
            get { return base.GetSqlXml("ListarByTopologia"); }
        }

        public string SqlListarByTopologiaYRecurso
        {
            get { return base.GetSqlXml("ListarByTopologiaYRecurso"); }
        }
        
    }
}
