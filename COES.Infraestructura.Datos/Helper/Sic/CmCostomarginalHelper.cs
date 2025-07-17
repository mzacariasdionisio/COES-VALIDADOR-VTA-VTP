using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_COSTOMARGINAL
    /// </summary>
    public class CmCostomarginalHelper : HelperBase
    {
        public CmCostomarginalHelper(): base(Consultas.CmCostomarginalSql)
        {
        }

        public CmCostomarginalDTO Create(IDataReader dr)
        {
            CmCostomarginalDTO entity = new CmCostomarginalDTO();

            int iCmgncodi = dr.GetOrdinal(this.Cmgncodi);
            if (!dr.IsDBNull(iCmgncodi)) entity.Cmgncodi = Convert.ToInt32(dr.GetValue(iCmgncodi));

            int iCnfbarcodi = dr.GetOrdinal(this.Cnfbarcodi);
            if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));

            int iCmgnenergia = dr.GetOrdinal(this.Cmgnenergia);
            if (!dr.IsDBNull(iCmgnenergia)) entity.Cmgnenergia = dr.GetDecimal(iCmgnenergia);

            int iCmgncongestion = dr.GetOrdinal(this.Cmgncongestion);
            if (!dr.IsDBNull(iCmgncongestion)) entity.Cmgncongestion = dr.GetDecimal(iCmgncongestion);

            int iCmgntotal = dr.GetOrdinal(this.Cmgntotal);
            if (!dr.IsDBNull(iCmgntotal)) entity.Cmgntotal = dr.GetDecimal(iCmgntotal);

            int iCmgncorrelativo = dr.GetOrdinal(this.Cmgncorrelativo);
            if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

            int iCmgnfecha = dr.GetOrdinal(this.Cmgnfecha);
            if (!dr.IsDBNull(iCmgnfecha)) entity.Cmgnfecha = dr.GetDateTime(iCmgnfecha);

            int iLastuser = dr.GetOrdinal(this.Cmgnusucreacion);
            if (!dr.IsDBNull(iLastuser)) entity.Cmgnusucreacion = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Cmgnfeccreacion);
            if (!dr.IsDBNull(iLastdate)) entity.Cmgnfeccreacion= dr.GetDateTime(iLastdate);

            int iCmgnDemanda = dr.GetOrdinal(this.Cmgndemanda);
            if (!dr.IsDBNull(iCmgnDemanda)) entity.Cmgndemanda = dr.GetDecimal(iCmgnDemanda);

            int iCmgnoperativo = dr.GetOrdinal(this.Cmgnoperativo);
            if (!dr.IsDBNull(iCmgnoperativo)) entity.Cmgnoperativo = Convert.ToInt32(dr.GetValue(iCmgnoperativo));

            return entity;
        }


        #region Mapeo de Campos

        public string Cmgncodi = "CMGNCODI";
        public string Cnfbarcodi = "CNFBARCODI";
        public string Cmgnenergia = "CMGNENERGIA";
        public string Cmgncongestion = "CMGNCONGESTION";
        public string Cmgntotal = "CMGNTOTAL";
        public string Cmgncorrelativo = "CMGNCORRELATIVO";
        public string Cmgnfecha = "CMGNFECHA";
        public string Cmgnusucreacion = "CMGNUSUCREACION";
        public string Cmgnfeccreacion = "CMGNFECCREACION";
        public string Cnfbarnodo = "CNFBARNODO";
        public string Cnfbarnombre = "CNFBARNOMBRE";
        public string Cnfbarcoorx = "CNFBARCOORX";
        public string Cnfbarcoory = "CNFBARCOORY";
        public string Cnfbarindpublicacion = "CNFBARINDPUBLICACION";
        public string Cnfbardefecto = "CNFBARDEFECTO";
        public string Cmgndemanda = "CMGNDEMANDA";
        public string Cmgnreproceso = "CMGNREPROCESO";
        public string Cmgnoperativo = "CMGNOPERATIVO";
        public string Tipoestimador = "TIPOESTIMADOR";
        public string Tipoproceso = "TIPOPROCESO";
        public string Versionpdo = "VERSIONPDO";
        public string Topcodi = "TOPCODI";
        public string Topnombre = "TOPNOMBRE";
        public string Cmveprversion = "CMVEPRVERSION";

        public string SqlObtenerCorrelativo
        {
            get { return base.GetSqlXml("ObtenerCorrelativo"); }
        }

        public string SqlObtenerResultadoCostoMarginal
        {
            get { return base.GetSqlXml("ObtenerResultadoCostoMarginal"); }
        }

        public string SqlObtenerResultadoCostoMarginalExtranet
        {
            get { return base.GetSqlXml("ObtenerResultadoCostoMarginalExtranet"); }
        }

        public string SqlObtenerResultadoCostoMarginalWeb
        {
            get { return base.GetSqlXml("ObtenerResultadoCostoMarginalWeb"); }
        }

        public string SqlObtenerDatosCostoMarginalCorrida
        {
            get { return base.GetSqlXml("ObtenerDatosCostoMarginalCorrida"); }
        }

        public string SqlObtenerReporteCostosMarginales
        {
            get { return base.GetSqlXml("ObtenerReporteCostosMarginales"); }
        }

        public string SqlObtenerReporteCostosMarginalesTR
        {
            get { return base.GetSqlXml("ObtenerReporteCostosMarginalesTR"); }
        }

        public string SqlActualizarCorrelativo
        {
            get { return base.GetSqlXml("ActualizarCorrelativo"); }
        }

        public string SqlObtenerReporteCostosMarginalesWeb
        {
            get { return base.GetSqlXml("ObtenerReporteCostosMarginalesWeb"); }
        }

        public string SqlObtenerIndicadorHora
        {
            get { return base.GetSqlXml("ObtenerIndicadorHora"); }
        }

        public string SqlGrabarRepresentativo
        {
            get { return base.GetSqlXml("GrabarRepresentativo"); }
        }

        public string SqlObtenerResumenCM
        {
            get { return base.GetSqlXml("ObtenerResumenCM"); }
        }

        public string SqlEliminarCorridaCostoMarginal
        {
            get { return base.GetSqlXml("EliminarCorridaCostoMarginal"); }
        }

        public string SqlObtenerDatosCostoMarginalXPeriodos
        {
            get { return base.GetSqlXml("ObtenerDatosCostoMarginalXPeriodos"); }
        }

        #endregion

        #region Mejoras CMgN - Movisoft

        public string SqlObtenerComparativoCM
        {
            get { return base.GetSqlXml("ObtenerComparativoCM"); }
        }

        public string SqlObtenerUltimosProcesosCM
        {
            get { return base.GetSqlXml("ObtenerUltimosProcesosCM"); }
        }

        public string SqlObtenerUltimosProcesosCMPorVersion
        {
            get { return base.GetSqlXml("ObtenerUltimosProcesosCMPorVersion"); }
        }

        #endregion
    }
}
