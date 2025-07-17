using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MMM_CAMBIOVERSION
    /// </summary>
    public class MmmCambioversionHelper : HelperBase
    {
        public MmmCambioversionHelper()
             : base(Consultas.MmmCambioversionSql)
        {
        }

        public MmmCambioversionDTO Create(IDataReader dr)
        {
            MmmCambioversionDTO entity = new MmmCambioversionDTO();

            int iMmmdatocodi = dr.GetOrdinal(this.Mmmdatcodi);
            if (!dr.IsDBNull(iMmmdatocodi)) entity.Mmmdatcodi = Convert.ToInt32(dr.GetValue(iMmmdatocodi));

            int iVermmcodi = dr.GetOrdinal(this.Vermmcodi);
            if (!dr.IsDBNull(iVermmcodi)) entity.Vermmcodi = Convert.ToInt32(dr.GetValue(iVermmcodi));

            int iCamvercodi = dr.GetOrdinal(this.Camvercodi);
            if (!dr.IsDBNull(iCamvercodi)) entity.Camvercodi = Convert.ToInt32(dr.GetValue(iCamvercodi));

            int iCamvertipo = dr.GetOrdinal(this.Camvertipo);
            if (!dr.IsDBNull(iCamvertipo)) entity.Camvertipo = Convert.ToInt32(dr.GetValue(iCamvertipo));

            int iCamverfeccreacion = dr.GetOrdinal(this.Camverfeccreacion);
            if (!dr.IsDBNull(iCamverfeccreacion)) entity.Camverfeccreacion = dr.GetDateTime(iCamverfeccreacion);

            int iCamverusucreacion = dr.GetOrdinal(this.Camverusucreacion);
            if (!dr.IsDBNull(iCamverusucreacion)) entity.Camverusucreacion = dr.GetString(iCamverusucreacion);

            int iCamvervalor = dr.GetOrdinal(this.Camvervalor);
            if (!dr.IsDBNull(iCamvervalor)) entity.Camvervalor = dr.GetDecimal(iCamvervalor);

            return entity;
        }

        #region Mapeo de Campos

        public string Vermmcodi = "VERMMCODI";
        public string Camvercodi = "CAMVERCODI";
        public string Camvertipo = "CAMVERTIPO";
        public string Camverfeccreacion = "CAMVERFECCREACION";
        public string Camverusucreacion = "CAMVERUSUCREACION";
        public string Camvervalor = "CAMVERVALOR";
        public string Mmmdatcodi = "Mmmdatcodi";
        public string Mmmdatfecha = "Mmmdatfecha";

        #endregion

        public string SqlListByPeriodo
        {
            get { return base.GetSqlXml("ListByPeriodo"); }
        }
    }
}
