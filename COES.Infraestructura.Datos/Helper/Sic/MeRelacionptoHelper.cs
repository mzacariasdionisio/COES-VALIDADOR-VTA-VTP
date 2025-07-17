using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_RELACIONPTO
    /// </summary>
    public class MeRelacionptoHelper : HelperBase
    {
        public MeRelacionptoHelper()
            : base(Consultas.MeRelacionptoSql)
        {
        }

        public MeRelacionptoDTO Create(IDataReader dr)
        {
            MeRelacionptoDTO entity = new MeRelacionptoDTO();

            int iRelptocodi = dr.GetOrdinal(this.Relptocodi);
            if (!dr.IsDBNull(iRelptocodi)) entity.Relptocodi = Convert.ToInt32(dr.GetValue(iRelptocodi));

            int iPtomedicodi1 = dr.GetOrdinal(this.Ptomedicodi1);
            if (!dr.IsDBNull(iPtomedicodi1)) entity.Ptomedicodi1 = Convert.ToInt32(dr.GetValue(iPtomedicodi1));

            int iPtomedicodi2 = dr.GetOrdinal(this.Ptomedicodi2);
            if (!dr.IsDBNull(iPtomedicodi2)) entity.Ptomedicodi2 = Convert.ToInt32(dr.GetValue(iPtomedicodi2));

            int iTrptocodi = dr.GetOrdinal(this.Trptocodi);
            if (!dr.IsDBNull(iTrptocodi)) entity.Trptocodi = Convert.ToInt32(dr.GetValue(iTrptocodi));

            int iRelptofactor = dr.GetOrdinal(this.Relptofactor);
            if (!dr.IsDBNull(iRelptofactor)) entity.Relptofactor = dr.GetDecimal(iRelptofactor);

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iTptomedicodi = dr.GetOrdinal(this.Tptomedicodi);
            if (!dr.IsDBNull(iTptomedicodi)) entity.Tptomedicodi = Convert.ToInt32(dr.GetValue(iTptomedicodi));

            int iRelptotabmed = dr.GetOrdinal(this.Relptotabmed);
            if (!dr.IsDBNull(iRelptotabmed)) entity.Relptotabmed = dr.GetInt32(iRelptotabmed);

            int iFunptocodi = dr.GetOrdinal(this.Funptocodi);
            if (!dr.IsDBNull(iFunptocodi)) entity.Funptocodi = dr.GetInt32(iFunptocodi);

            int iRelptopotencia = dr.GetOrdinal(this.Relptopotencia);
            if (!dr.IsDBNull(iRelptopotencia)) entity.Relptopotencia = dr.GetDecimal(iRelptopotencia);

            return entity;
        }


        #region Mapeo de Campos

        public string Relptocodi = "RELPTOCODI";
        public string Ptomedicodi1 = "PTOMEDICODI1";
        public string Ptomedicodi2 = "PTOMEDICODI2";
        public string Trptocodi = "TRPTOCODI";
        public string Relptofactor = "RELPTOFACTOR";
        public string Relptopotencia = "RELPTOPOTENCIA";

        #endregion

        public string Lectcodi = "Lectcodi";
        public string Tipoinfocodi = "Tipoinfocodi";
        public string Tptomedicodi = "Tptomedicodi";
        public string Ptomedinomb = "Ptomedielenomb";
        public string Equicodi = "Equicodi";
        public string Grupocodi = "GRUPOCODI";

        public string Relptotabmed = "Relptotabmed";

        #region SIOSEIN
        public string Funptocodi = "FUNPTOCODI";
        public string Funptofuncion = "FUNPTOFUNCION";
        #endregion
    }
}
