using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_FACTOR_VERSION_DET
    /// </summary>
    public class InFactorVersionDetHelper : HelperBase
    {
        public InFactorVersionDetHelper() : base(Consultas.InFactorVersionDetSql)
        {
        }

        public InFactorVersionDetDTO Create(IDataReader dr)
        {
            InFactorVersionDetDTO entity = new InFactorVersionDetDTO();

            int iInfvdtcodi = dr.GetOrdinal(this.Infvdtcodi);
            if (!dr.IsDBNull(iInfvdtcodi)) entity.Infvdtcodi = Convert.ToInt32(dr.GetValue(iInfvdtcodi));

            int iInfvdtintercodis = dr.GetOrdinal(this.Infvdtintercodis);
            if (!dr.IsDBNull(iInfvdtintercodis)) entity.Infvdtintercodis = dr.GetString(iInfvdtintercodis);

            int iInfvdthorizonte = dr.GetOrdinal(this.Infvdthorizonte);
            if (!dr.IsDBNull(iInfvdthorizonte)) entity.Infvdthorizonte = dr.GetString(iInfvdthorizonte);

            int iInfvercodi = dr.GetOrdinal(this.Infvercodi);
            if (!dr.IsDBNull(iInfvercodi)) entity.Infvercodi = Convert.ToInt32(dr.GetValue(iInfvercodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Infvdtcodi = "INFVDTCODI";
        public string Infvdtintercodis = "INFVDTINTERCODIS";
        public string Infvdthorizonte = "INFVDTHORIZONTE";
        public string Infvercodi = "INFVERCODI";

        #endregion

        public string SqlListxInfvercodi
        {
            get { return GetSqlXml("ListByIDinfvercodi"); }
        }
    }
}
