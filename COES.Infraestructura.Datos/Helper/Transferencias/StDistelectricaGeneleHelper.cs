using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_DISTELECTRICA_GENELE
    /// </summary>
    public class StDistelectricaGeneleHelper : HelperBase
    {
        public StDistelectricaGeneleHelper(): base(Consultas.StDistelectricaGeneleSql)
        {
        }

        public StDistelectricaGeneleDTO Create(IDataReader dr)
        {
            StDistelectricaGeneleDTO entity = new StDistelectricaGeneleDTO();

            int iDegelecodi = dr.GetOrdinal(this.Degelecodi);
            if (!dr.IsDBNull(iDegelecodi)) entity.Degelecodi = Convert.ToInt32(dr.GetValue(iDegelecodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iStcntgcodi = dr.GetOrdinal(this.Stcntgcodi);
            if (!dr.IsDBNull(iStcntgcodi)) entity.Stcntgcodi = Convert.ToInt32(dr.GetValue(iStcntgcodi));

            int iBarrcodigw = dr.GetOrdinal(this.Barrcodigw);
            if (!dr.IsDBNull(iBarrcodigw)) entity.Barrcodigw = Convert.ToInt32(dr.GetValue(iBarrcodigw));

            int iStcompcodi = dr.GetOrdinal(this.Stcompcodi);
            if (!dr.IsDBNull(iStcompcodi)) entity.Stcompcodi = Convert.ToInt32(dr.GetValue(iStcompcodi));

            int iBarrcodilm = dr.GetOrdinal(this.Barrcodilm);
            if (!dr.IsDBNull(iBarrcodilm)) entity.Barrcodilm = Convert.ToInt32(dr.GetValue(iBarrcodilm));

            int iBarrcodiln = dr.GetOrdinal(this.Barrcodiln);
            if (!dr.IsDBNull(iBarrcodiln)) entity.Barrcodiln = Convert.ToInt32(dr.GetValue(iBarrcodiln));
                       
            int iDegeledistancia = dr.GetOrdinal(this.Degeledistancia);
            if (!dr.IsDBNull(iDegeledistancia)) entity.Degeledistancia = dr.GetDecimal(iDegeledistancia);

            int iDegeleusucreacion = dr.GetOrdinal(this.Degeleusucreacion);
            if (!dr.IsDBNull(iDegeleusucreacion)) entity.Degeleusucreacion = dr.GetString(iDegeleusucreacion);

            int iDegelefeccreacion = dr.GetOrdinal(this.Degelefeccreacion);
            if (!dr.IsDBNull(iDegelefeccreacion)) entity.Degelefeccreacion = dr.GetDateTime(iDegelefeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Degelecodi = "DEGELECODI";
        public string Strecacodi = "STRECACODI";
        public string Stcntgcodi = "STCNTGCODI";
        public string Barrcodigw = "BARRCODIGW";
        public string Stcompcodi = "STCOMPCODI";
        public string Barrcodilm = "BARRCODILM";
        public string Barrcodiln = "BARRCODILN";
        public string Degeledistancia = "DEGELEDISTANCIA";
        public string Degeleusucreacion = "DEGELEUSUCREACION";
        public string Degelefeccreacion = "DEGELEFECCREACION";
        //variables para Reportes
        public string Equinomb = "EQUINOMB";
        public string Cmpmelcodelemento = "CMPMELCODELEMENTO";

        #endregion

        public string SqlGetByIdCriteriaStDistGene
        {
            get { return base.GetSqlXml("GetByIdCriteriaStDistGene"); }
        }

        public string SqlGetByIdCriteriaStDistGeneReporte
        {
            get { return base.GetSqlXml("GetByIdCriteriaStDistGeneReporte"); }
        }
    }
}
