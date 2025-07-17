using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FW_AREA
    /// </summary>
    public class FwAreaHelper : HelperBase
    {
        public FwAreaHelper(): base(Consultas.FwAreaSql)
        {
        }

        public FwAreaDTO Create(IDataReader dr)
        {
            FwAreaDTO entity = new FwAreaDTO();

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iAreaname = dr.GetOrdinal(this.Areaname);
            if (!dr.IsDBNull(iAreaname)) entity.Areaname = dr.GetString(iAreaname);

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));

            int iCompcode = dr.GetOrdinal(this.Compcode);
            if (!dr.IsDBNull(iCompcode)) entity.Compcode = Convert.ToInt32(dr.GetValue(iCompcode));

            int iFlagreclamos = dr.GetOrdinal(this.Flagreclamos);
            if (!dr.IsDBNull(iFlagreclamos)) entity.Flagreclamos = dr.GetString(iFlagreclamos);

            int iAreapadre = dr.GetOrdinal(this.Areapadre);
            if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = Convert.ToInt32(dr.GetValue(iAreapadre));

            int iAreaorder = dr.GetOrdinal(this.Areaorder);
            if (!dr.IsDBNull(iAreaorder)) entity.Areaorder = Convert.ToInt32(dr.GetValue(iAreaorder));

            return entity;
        }


        #region Mapeo de Campos

        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Areaabrev = "AREAABREV";
        public string Areaname = "AREANAME";
        public string Areacode = "AREACODE";
        public string Compcode = "COMPCODE";
        public string Flagreclamos = "FLAGRECLAMOS";
        public string Areapadre = "AREAPADRE";
        public string Areaorder = "AREAORDER";

        #endregion

        public string SqlListAreaXFormato
        {
            get { return base.GetSqlXml("ListAreaXFormato"); }
        }

        public string SqlGetDirResp
        {
            get { return base.GetSqlXml("GetDirResp"); }
        }

        public string SqlListarArea
        {
            get { return base.GetSqlXml("ListarArea"); }
        }
    }
}
