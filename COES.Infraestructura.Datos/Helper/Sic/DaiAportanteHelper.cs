using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla DAI_APORTANTE
    /// </summary>
    public class DaiAportanteHelper : HelperBase
    {
        public DaiAportanteHelper(): base(Consultas.DaiAportanteSql)
        {
        }

        public string SqlGetByCriteriaAportanteCronograma
        {
            get { return GetSqlXml("GetByCriteriaAportanteCronograma"); }
        }

        public string SqlGetByCriteriaAportanteLiquidacion
        {
            get { return GetSqlXml("GetByCriteriaAportanteLiquidacion"); }
        }
        
        public string SqlDeleteByPresupuesto
        {
            get { return GetSqlXml("DeleteByPresupuesto"); }
        }

        public string SqlGetByCriteriaAniosCronograma
        {
            get { return GetSqlXml("GetByCriteriaAniosCronograma"); }
        }

        public string SqlGetByCriteriaFinalizar
        {
            get { return GetSqlXml("GetByCriteriaFinalizar"); }
        }
        

        public string SqlListCuadroDevolucion
        {
            get { return GetSqlXml("ListCuadroDevolucion"); }
        }
        

        public DaiAportanteDTO Create(IDataReader dr)
        {
            DaiAportanteDTO entity = new DaiAportanteDTO();

            int iAporcodi = dr.GetOrdinal(this.Aporcodi);
            if (!dr.IsDBNull(iAporcodi)) entity.Aporcodi = Convert.ToInt32(dr.GetValue(iAporcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPrescodi = dr.GetOrdinal(this.Prescodi);
            if (!dr.IsDBNull(iPrescodi)) entity.Prescodi = Convert.ToInt32(dr.GetValue(iPrescodi));

            int iTabcdcodiestado = dr.GetOrdinal(this.Tabcdcodiestado);
            if (!dr.IsDBNull(iTabcdcodiestado)) entity.Tabcdcodiestado = Convert.ToInt32(dr.GetValue(iTabcdcodiestado));

            int iAporporcentajeparticipacion = dr.GetOrdinal(this.Aporporcentajeparticipacion);
            if (!dr.IsDBNull(iAporporcentajeparticipacion)) entity.Aporporcentajeparticipacion = dr.GetDecimal(iAporporcentajeparticipacion);

            int iApormontoparticipacion = dr.GetOrdinal(this.Apormontoparticipacion);
            if (!dr.IsDBNull(iApormontoparticipacion)) entity.Apormontoparticipacion = dr.GetDecimal(iApormontoparticipacion);

            int iAporaniosinaporte = dr.GetOrdinal(this.Aporaniosinaporte);
            if (!dr.IsDBNull(iAporaniosinaporte)) entity.Aporaniosinaporte = Convert.ToInt32(dr.GetValue(iAporaniosinaporte));

            int iAporliquidado = dr.GetOrdinal(this.Aporliquidado);
            if (!dr.IsDBNull(iAporliquidado)) entity.Aporliquidado = dr.GetString(iAporliquidado);

            int iAporusuliquidacion = dr.GetOrdinal(this.Aporusuliquidacion);
            if (!dr.IsDBNull(iAporusuliquidacion)) entity.Aporusuliquidacion = dr.GetString(iAporusuliquidacion);

            int iAporfecliquidacion = dr.GetOrdinal(this.Aporfecliquidacion);
            if (!dr.IsDBNull(iAporfecliquidacion)) entity.Aporfecliquidacion = dr.GetDateTime(iAporfecliquidacion);

            int iAporactivo = dr.GetOrdinal(this.Aporactivo);
            if (!dr.IsDBNull(iAporactivo)) entity.Aporactivo = dr.GetString(iAporactivo);

            int iAporusucreacion = dr.GetOrdinal(this.Aporusucreacion);
            if (!dr.IsDBNull(iAporusucreacion)) entity.Aporusucreacion = dr.GetString(iAporusucreacion);

            int iAprofeccreacion = dr.GetOrdinal(this.Aprofeccreacion);
            if (!dr.IsDBNull(iAprofeccreacion)) entity.Aprofeccreacion = dr.GetDateTime(iAprofeccreacion);

            int iAporusumodificacion = dr.GetOrdinal(this.Aporusumodificacion);
            if (!dr.IsDBNull(iAporusumodificacion)) entity.Aporusumodificacion = dr.GetString(iAporusumodificacion);

            int iAporfecmodificacion = dr.GetOrdinal(this.Aporfecmodificacion);
            if (!dr.IsDBNull(iAporfecmodificacion)) entity.Aporfecmodificacion = dr.GetDateTime(iAporfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Aporcodi = "APORCODI";
        public string Emprcodi = "EMPRCODI";
        public string Prescodi = "PRESCODI";
        public string Tabcdcodiestado = "TABCDCODIESTADO";
        public string Aporporcentajeparticipacion = "APORPORCENTAJEPARTICIPACION";
        public string Apormontoparticipacion = "APORMONTOPARTICIPACION";
        public string Aporaniosinaporte = "APORANIOSINAPORTE";
        public string Aporliquidado = "APORLIQUIDADO";
        public string Aporusuliquidacion = "APORUSULIQUIDACION";
        public string Aporfecliquidacion = "APORFECLIQUIDACION";
        public string Aporactivo = "APORACTIVO";
        public string Aporusucreacion = "APORUSUCREACION";
        public string Aprofeccreacion = "APROFECCREACION";
        public string Aporusumodificacion = "APORUSUMODIFICACION";
        public string Aporfecmodificacion = "APORFECMODIFICACION";

        public string Emprnomb = "EMPRNOMB";
        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string Emprruc = "EMPRRUC";
        public string Tipoempresa = "TIPOEMPRESA";
        public string Estadoaportante = "ESTADOAPORTANTE";

        public string Estado = "ESTADO";
        public string Caleanio = "caleanio";

        public string Caleinteres = "caleinteres";
        public string Caleinteresigv = "caleinteresigv";
        public string Totalinteresesigv = "totalinteresesigv";
        public string Amortizacion = "amortizacion";
        public string Total = "total";

        #endregion
    }
}
