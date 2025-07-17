using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_REPORTEDETALLE
    /// </summary>
    public class CmReportedetalleHelper : HelperBase
    {
        public CmReportedetalleHelper(): base(Consultas.CmReportedetalleSql)
        {
        }

        public CmReportedetalleDTO Create(IDataReader dr)
        {
            CmReportedetalleDTO entity = new CmReportedetalleDTO();

            int iCmrepcodi = dr.GetOrdinal(this.Cmrepcodi);
            if (!dr.IsDBNull(iCmrepcodi)) entity.Cmrepcodi = Convert.ToInt32(dr.GetValue(iCmrepcodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCmredefecha = dr.GetOrdinal(this.Cmredefecha);
            if (!dr.IsDBNull(iCmredefecha)) entity.Cmredefecha = dr.GetDateTime(iCmredefecha);

            int iCmredeperiodo = dr.GetOrdinal(this.Cmredeperiodo);
            if (!dr.IsDBNull(iCmredeperiodo)) entity.Cmredeperiodo = Convert.ToInt32(dr.GetValue(iCmredeperiodo));

            int iCmredecmtotal = dr.GetOrdinal(this.Cmredecmtotal);
            if (!dr.IsDBNull(iCmredecmtotal)) entity.Cmredecmtotal = dr.GetDecimal(iCmredecmtotal);

            int iCmredecmenergia = dr.GetOrdinal(this.Cmredecmenergia);
            if (!dr.IsDBNull(iCmredecmenergia)) entity.Cmredecmenergia = dr.GetDecimal(iCmredecmenergia);

            int iCmredecongestion = dr.GetOrdinal(this.Cmredecongestion);
            if (!dr.IsDBNull(iCmredecongestion)) entity.Cmredecongestion = dr.GetDecimal(iCmredecongestion);

            int iCmredevaltotal = dr.GetOrdinal(this.Cmredevaltotal);
            if (!dr.IsDBNull(iCmredevaltotal)) entity.Cmredevaltotal = Convert.ToInt32(dr.GetValue(iCmredevaltotal));

            int iCmredevalenergia = dr.GetOrdinal(this.Cmredevalenergia);
            if (!dr.IsDBNull(iCmredevalenergia)) entity.Cmredevalenergia = Convert.ToInt32(dr.GetValue(iCmredevalenergia));

            int iCmredevalcongestion = dr.GetOrdinal(this.Cmredevalcongestion);
            if (!dr.IsDBNull(iCmredevalcongestion)) entity.Cmredevalcongestion = Convert.ToInt32(dr.GetValue(iCmredevalcongestion));

            int iCmredetiporeporte = dr.GetOrdinal(this.Cmredetiporeporte);
            if (!dr.IsDBNull(iCmredetiporeporte)) entity.Cmredetiporeporte = dr.GetString(iCmredetiporeporte);

            int iCmredecodi = dr.GetOrdinal(this.Cmredecodi);
            if (!dr.IsDBNull(iCmredecodi)) entity.Cmredecodi = Convert.ToInt32(dr.GetValue(iCmredecodi));

            int iBarrcodi2 = dr.GetOrdinal(this.Barrcodi2);
            if (!dr.IsDBNull(iBarrcodi2)) entity.Barrcodi2 = Convert.ToInt32(dr.GetValue(iBarrcodi2));

            return entity;
        }


        #region Mapeo de Campos

        public string Cmrepcodi = "CMREPCODI";
        public string Barrcodi = "BARRCODI";
        public string Cmredefecha = "CMREDEFECHA";
        public string Cmredeperiodo = "CMREDEPERIODO";
        public string Cmredecmtotal = "CMREDECMTOTAL";
        public string Cmredecmenergia = "CMREDECMENERGIA";
        public string Cmredecongestion = "CMREDECONGESTION";
        public string Cmredevaltotal = "CMREDEVALTOTAL";
        public string Cmredevalenergia = "CMREDEVALENERGIA";
        public string Cmredevalcongestion = "CMREDEVALCONGESTION";
        public string Cmredetiporeporte = "CMREDETIPOREPORTE";
        public string Cmredecodi = "CMREDECODI";
        public string Barrnombre = "BARRNOMBRE";
        public string NombreTabla = "CM_REPORTEDETALLE";
        public string Barrcodi2 = "BARRCODI2";
        public string Barrnombre2 = "BARRNOMBRE2";
        #endregion

        public string SqlObtenerReporte
        {
            get { return base.GetSqlXml("ObtenerReporte"); }
        }
    }
}
