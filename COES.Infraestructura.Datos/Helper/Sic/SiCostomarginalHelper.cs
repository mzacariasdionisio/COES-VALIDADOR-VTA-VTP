using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_COSTOMARGINAL
    /// </summary>
    public class SiCostomarginalHelper : HelperBase
    {
        public SiCostomarginalHelper(): base(Consultas.SiCostomarginalSql)
        {
        }

        public SiCostomarginalDTO Create(IDataReader dr)
        {
            SiCostomarginalDTO entity = new SiCostomarginalDTO();

            int iCmgrcodi = dr.GetOrdinal(this.Cmgrcodi);
            if (!dr.IsDBNull(iCmgrcodi)) entity.Cmgrcodi = Convert.ToInt32(dr.GetValue(iCmgrcodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCmgrenergia = dr.GetOrdinal(this.Cmgrenergia);
            if (!dr.IsDBNull(iCmgrenergia)) entity.Cmgrenergia = dr.GetDecimal(iCmgrenergia);

            int iCmgrcongestion = dr.GetOrdinal(this.Cmgrcongestion);
            if (!dr.IsDBNull(iCmgrcongestion)) entity.Cmgrcongestion = dr.GetDecimal(iCmgrcongestion);

            int iCmgrtotal = dr.GetOrdinal(this.Cmgrtotal);
            if (!dr.IsDBNull(iCmgrtotal)) entity.Cmgrtotal = dr.GetDecimal(iCmgrtotal);

            int iCmgrcorrelativo = dr.GetOrdinal(this.Cmgrcorrelativo);
            if (!dr.IsDBNull(iCmgrcorrelativo)) entity.Cmgrcorrelativo = Convert.ToInt32(dr.GetValue(iCmgrcorrelativo));

            int iCmgrfecha = dr.GetOrdinal(this.Cmgrfecha);
            if (!dr.IsDBNull(iCmgrfecha)) entity.Cmgrfecha = dr.GetDateTime(iCmgrfecha);

            int iCmgrusucreacion = dr.GetOrdinal(this.Cmgrusucreacion);
            if (!dr.IsDBNull(iCmgrusucreacion)) entity.Cmgrusucreacion = dr.GetString(iCmgrusucreacion);

            int iCmgrfeccreacion = dr.GetOrdinal(this.Cmgrfeccreacion);
            if (!dr.IsDBNull(iCmgrfeccreacion)) entity.Cmgrfeccreacion = dr.GetDateTime(iCmgrfeccreacion);

            int iCmgrtcodi = dr.GetOrdinal(this.Cmgrtcodi);
            if (!dr.IsDBNull(iCmgrtcodi)) entity.Cmgrtcodi = dr.GetInt32(iCmgrtcodi);

            return entity;
        }

        #region Mapeo de Campos

        public string Cmgrcodi = "CMGRCODI";
        public string Barrcodi = "BARRCODI";
        public string Cmgrenergia = "CMGRENERGIA";
        public string Cmgrcongestion = "CMGRCONGESTION";
        public string Cmgrtotal = "CMGRTOTAL";
        public string Cmgrcorrelativo = "CMGRCORRELATIVO";
        public string Cmgrfecha = "CMGRFECHA";
        public string Cmgrusucreacion = "CMGRUSUCREACION";
        public string Cmgrfeccreacion = "CMGRFECCREACION";
        public string TableName = "SI_COSTOMARGINAL";
        public string Cmgrtcodi = "CMGRTCODI";
        public string Barrnomb = "BARRNOMBRE";
        public string Fechahoracm = "HORA";

        #region SIOSEIN
        public string Osinergcodi = "OSINERGCODI";
        #endregion

        #endregion

        public string SqlDeleteSiCostomarginalXFecha
        {
            get { return base.GetSqlXml("DeleteSiCostomarginalXFecha"); }
        }

        public string SqlGetByCriteriaSiCostomarginalDet
        {
            get { return base.GetSqlXml("GetByCriteriaSiCostomarginalDet"); }
        }

        public string SqlProcesarTempCostoMarginal
        {
            get { return base.GetSqlXml("ProcesarTempCostoMarginal"); }
        }

        public string SqlObtenerCmgPromedioDiarioDeBarras
        {
            get { return base.GetSqlXml("ObtenerCmgPromedioDiarioDeBarras"); }
        }

        public string SqlObtenerReporteValoresNulos
        {
            get { return base.GetSqlXml("ObtenerReporteValoresNulos"); }
        }

    }
}
