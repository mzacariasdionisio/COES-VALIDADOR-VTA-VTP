using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_REGISTRO_MODPLAN
    /// </summary>
    public class WbRegistroModplanHelper : HelperBase
    {
        public WbRegistroModplanHelper(): base(Consultas.WbRegistroModplanSql)
        {
        }

        public WbRegistroModplanDTO Create(IDataReader dr)
        {
            WbRegistroModplanDTO entity = new WbRegistroModplanDTO();

            int iRegmodcodi = dr.GetOrdinal(this.Regmodcodi);
            if (!dr.IsDBNull(iRegmodcodi)) entity.Regmodcodi = Convert.ToInt32(dr.GetValue(iRegmodcodi));

            int iRegmodplan = dr.GetOrdinal(this.Regmodplan);
            if (!dr.IsDBNull(iRegmodplan)) entity.Regmodplan = dr.GetString(iRegmodplan);

            int iRegmodversion = dr.GetOrdinal(this.Regmodversion);
            if (!dr.IsDBNull(iRegmodversion)) entity.Regmodversion = dr.GetString(iRegmodversion);

            int iRegmodusuario = dr.GetOrdinal(this.Regmodusuario);
            if (!dr.IsDBNull(iRegmodusuario)) entity.Regmodusuario = dr.GetString(iRegmodusuario);

            int iRegmodfecha = dr.GetOrdinal(this.Regmodfecha);
            if (!dr.IsDBNull(iRegmodfecha)) entity.Regmodfecha = dr.GetDateTime(iRegmodfecha);

            int iVermplcodi = dr.GetOrdinal(this.Vermplcodi);
            if (!dr.IsDBNull(iVermplcodi)) entity.Vermplcodi = Convert.ToInt32(dr.GetValue(iVermplcodi));

            int iRegmodtipo = dr.GetOrdinal(this.Regmodtipo);
            if (!dr.IsDBNull(iRegmodtipo)) entity.Regmodtipo = Convert.ToInt32(dr.GetValue(iRegmodtipo));

            return entity;
        }


        #region Mapeo de Campos

        public string Regmodcodi = "REGMODCODI";
        public string Regmodplan = "REGMODPLAN";
        public string Regmodversion = "REGMODVERSION";
        public string Regmodusuario = "REGMODUSUARIO";
        public string Regmodfecha = "REGMODFECHA";
        public string Emprnomb = "EMPRNOMB";
        public string Vermplcodi = "VERMPLCODI";
        public string Emprcodi = "EMPRCODI";
        public string Regmodtipo = "REGMODTIPO";
        public string Arcmplcodi = "ARCMPLCODI";

        #endregion

        public string SqlObtenerReporte
        {
            get { return base.GetSqlXml("ObtenerReporte"); }
        }
    }
}
