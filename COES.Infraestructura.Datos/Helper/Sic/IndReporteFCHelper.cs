using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_REPORTE_FC
    /// </summary>
    public class IndReporteFCHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Irptfccodi = "IRPTFCCODI";
        public string Itotcodi = "ITOTCODI";
        public string Irptfctipcombustible = "IRPTFCTIPCOMBUSTIBLE";
        public string Irptfcnomcombustible = "IRPTFCNOMCOMBUSTIBLE";
        public string Irptfcmw = "IRPTFCMW";
        public string Irptfcm3h = "IRPTFCM3H";
        public string Irptfc1000m3h = "IRPTFC1000M3H";
        public string Irptfckpch = "IRPTFCKPCH";
        public string Irptfcmmpch = "IRPTFCMMPCH";
        public string Irptfclh = "IRPTFCLH";
        public string Irptfcgalh = "IRPTFCGALH";
        public string Irptfccmtr = "IRPTFCCMTR";
        public string Irptfcnumdias = "IRPTFCNUMDIAS";
        public string Irptfcrngdias = "IRPTFCRNGDIAS";
        public string Irptfcsec = "IRPTFCSEC";
        public string Irptfcusucreacion = "IRPTFCUSUCREACION";
        public string Irptfcfeccreacion = "IRPTFCFECCREACION";
        #endregion

        #region Constructor
        public IndReporteFCHelper() : base(Consultas.IndReporteFCSql)
        {
        }
        #endregion

        #region Create
        public IndReporteFCDTO Create(IDataReader dr)
        {
            IndReporteFCDTO entity = new IndReporteFCDTO();

            int iIrptfccodi = dr.GetOrdinal(this.Irptfccodi);
            if (!dr.IsDBNull(iIrptfccodi)) entity.Irptfccodi = Convert.ToInt32(dr.GetValue(iIrptfccodi));

            int iItotcodi = dr.GetOrdinal(this.Itotcodi);
            if (!dr.IsDBNull(iItotcodi)) entity.Itotcodi = Convert.ToInt32(dr.GetValue(iItotcodi));

            int iIrptfctipcombustible = dr.GetOrdinal(this.Irptfctipcombustible);
            if (!dr.IsDBNull(iIrptfctipcombustible)) entity.Irptfctipcombustible = Convert.ToInt32(dr.GetValue(iIrptfctipcombustible));

            int iIrptfcnomcombustible = dr.GetOrdinal(this.Irptfcnomcombustible);
            if (!dr.IsDBNull(iIrptfcnomcombustible)) entity.Irptfcnomcombustible = dr.GetString(iIrptfcnomcombustible);

            int iIrptfcmw = dr.GetOrdinal(this.Irptfcmw);
            if (!dr.IsDBNull(iIrptfcmw)) entity.Irptfcmw = Convert.ToDecimal(dr.GetValue(iIrptfcmw));

            int iIrptfcm3h = dr.GetOrdinal(this.Irptfcm3h);
            if (!dr.IsDBNull(iIrptfcm3h)) entity.Irptfcm3h = Convert.ToDecimal(dr.GetValue(iIrptfcm3h));

            int iIrptfc1000m3h = dr.GetOrdinal(this.Irptfc1000m3h);
            if (!dr.IsDBNull(iIrptfc1000m3h)) entity.Irptfc1000m3h = Convert.ToDecimal(dr.GetValue(iIrptfc1000m3h));

            int iIrptfckpch = dr.GetOrdinal(this.Irptfckpch);
            if (!dr.IsDBNull(iIrptfckpch)) entity.Irptfckpch = Convert.ToDecimal(dr.GetValue(iIrptfckpch));

            int iIrptfcmmpch = dr.GetOrdinal(this.Irptfcmmpch);
            if (!dr.IsDBNull(iIrptfcmmpch)) entity.Irptfcmmpch = Convert.ToDecimal(dr.GetValue(iIrptfcmmpch));

            int iIrptfclh = dr.GetOrdinal(this.Irptfclh);
            if (!dr.IsDBNull(iIrptfclh)) entity.Irptfclh = Convert.ToDecimal(dr.GetValue(iIrptfclh));

            int iIrptfcgalh = dr.GetOrdinal(this.Irptfcgalh);
            if (!dr.IsDBNull(iIrptfcgalh)) entity.Irptfcgalh = Convert.ToDecimal(dr.GetValue(iIrptfcgalh));

            int iIrptfccmtr = dr.GetOrdinal(this.Irptfccmtr);
            if (!dr.IsDBNull(iIrptfccmtr)) entity.Irptfccmtr = Convert.ToDecimal(dr.GetValue(iIrptfccmtr));

            int iIrptfcnumdias = dr.GetOrdinal(this.Irptfcnumdias);
            if (!dr.IsDBNull(iIrptfcnumdias)) entity.Irptfcnumdias = Convert.ToInt32(dr.GetValue(iIrptfcnumdias));

            int iIrptfcrngdias = dr.GetOrdinal(this.Irptfcrngdias);
            if (!dr.IsDBNull(iIrptfcrngdias)) entity.Irptfcrngdias = dr.GetString(iIrptfcrngdias);

            int iIrptfcsec = dr.GetOrdinal(this.Irptfcsec);
            if (!dr.IsDBNull(iIrptfcsec)) entity.Irptfcsec = Convert.ToInt32(dr.GetValue(iIrptfcsec));

            int iIrptfcusucreacion = dr.GetOrdinal(this.Irptfcusucreacion);
            if (!dr.IsDBNull(iIrptfcusucreacion)) entity.Irptfcusucreacion = dr.GetString(iIrptfcusucreacion);

            int iIrptfcfeccreacion = dr.GetOrdinal(this.Irptfcfeccreacion);
            if (!dr.IsDBNull(iIrptfcfeccreacion)) entity.Irptfcfeccreacion = dr.GetDateTime(iIrptfcfeccreacion);

            return entity;
        }
        #endregion

        #region Consultas

        #endregion
    }
}
