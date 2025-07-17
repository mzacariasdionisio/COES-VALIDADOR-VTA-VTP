using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_RESUMENMDDETALLE
    /// </summary>
    public class WbResumenmddetalleHelper : HelperBase
    {
        public WbResumenmddetalleHelper() : base(Consultas.WbResumenmddetalleSql)
        {
        }

        public WbResumenmddetalleDTO Create(IDataReader dr)
        {
            WbResumenmddetalleDTO entity = new WbResumenmddetalleDTO();

            int iResmddcodi = dr.GetOrdinal(this.Resmddcodi);
            if (!dr.IsDBNull(iResmddcodi)) entity.Resmddcodi = Convert.ToInt32(dr.GetValue(iResmddcodi));

            int iResmdcodi = dr.GetOrdinal(this.Resmdcodi);
            if (!dr.IsDBNull(iResmdcodi)) entity.Resmdcodi = Convert.ToInt32(dr.GetValue(iResmdcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iResmddfecha = dr.GetOrdinal(this.Resmddfecha);
            if (!dr.IsDBNull(iResmddfecha)) entity.Resmddfecha = dr.GetDateTime(iResmddfecha);

            int iResmddvalor = dr.GetOrdinal(this.Resmddvalor);
            if (!dr.IsDBNull(iResmddvalor)) entity.Resmddvalor = dr.GetDecimal(iResmddvalor);

            int iResmddmes = dr.GetOrdinal(this.Resmddmes);
            if (!dr.IsDBNull(iResmddmes)) entity.Resmddmes = dr.GetDateTime(iResmddmes);

            int iResmddtipogenerrer = dr.GetOrdinal(this.Resmddtipogenerrer);
            if (!dr.IsDBNull(iResmddtipogenerrer)) entity.Resmddtipogenerrer = dr.GetString(iResmddtipogenerrer);

            int iResmddusumodificacion = dr.GetOrdinal(this.Resmddusumodificacion);
            if (!dr.IsDBNull(iResmddusumodificacion)) entity.Resmddusumodificacion = dr.GetString(iResmddusumodificacion);

            int iResmddfecmodificacion = dr.GetOrdinal(this.Resmddfecmodificacion);
            if (!dr.IsDBNull(iResmddfecmodificacion)) entity.Resmddfecmodificacion = dr.GetDateTime(iResmddfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Resmddcodi = "RESMDDCODI";
        public string Resmdcodi = "RESMDCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Fenergcodi = "FENERGCODI";
        public string Resmddfecha = "RESMDDFECHA";
        public string Resmddvalor = "RESMDDVALOR";
        public string Resmddmes = "RESMDDMES";
        public string Tgenercodi = "TGENERCODI";
        public string Tgenernomb = "TGENERNOMB";
        public string Tgenercolor = "TGENERCOLOR";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Resmddtipogenerrer = "RESMDDTIPOGENERRER";
        public string Resmddusumodificacion = "RESMDDUSUMODIFICACION";
        public string Resmddfecmodificacion = "RESMDDFECMODIFICACION";

        #endregion

        public string SqlDeleteByMes
        {
            get { return base.GetSqlXml("DeleteByMes"); }
        }

        public string SqlGetByIdMd
        {
            get { return base.GetSqlXml("GetByIdMd"); }
        }
    }
}
