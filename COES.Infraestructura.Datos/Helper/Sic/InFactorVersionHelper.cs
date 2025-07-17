using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_FACTOR_VERSION
    /// </summary>
    public class InFactorVersionHelper : HelperBase
    {
        public InFactorVersionHelper() : base(Consultas.InFactorVersionSql)
        {
        }

        public InFactorVersionDTO Create(IDataReader dr)
        {
            InFactorVersionDTO entity = new InFactorVersionDTO();

            int iInfvercodi = dr.GetOrdinal(this.Infvercodi);
            if (!dr.IsDBNull(iInfvercodi)) entity.Infvercodi = Convert.ToInt32(dr.GetValue(iInfvercodi));

            int iInfverfechaperiodo = dr.GetOrdinal(this.Infverfechaperiodo);
            if (!dr.IsDBNull(iInfverfechaperiodo)) entity.Infverfechaperiodo = dr.GetDateTime(iInfverfechaperiodo);

            int iInfvertipoeq = dr.GetOrdinal(this.Infvertipoeq);
            if (!dr.IsDBNull(iInfvertipoeq)) entity.Infvertipoeq = dr.GetString(iInfvertipoeq);

            int iInfverdisp = dr.GetOrdinal(this.Infverdisp);
            if (!dr.IsDBNull(iInfverdisp)) entity.Infverdisp = dr.GetString(iInfverdisp);

            int iInfverflagfinal = dr.GetOrdinal(this.Infverflagfinal);
            if (!dr.IsDBNull(iInfverflagfinal)) entity.Infverflagfinal = dr.GetString(iInfverflagfinal);

            int iInfverflagdefinitivo = dr.GetOrdinal(this.Infverflagdefinitivo);
            if (!dr.IsDBNull(iInfverflagdefinitivo)) entity.Infverflagdefinitivo = dr.GetString(iInfverflagdefinitivo);

            int iInfverf1 = dr.GetOrdinal(this.Infverf1);
            if (!dr.IsDBNull(iInfverf1)) entity.Infverf1 = dr.GetDecimal(iInfverf1);

            int iInfverf2 = dr.GetOrdinal(this.Infverf2);
            if (!dr.IsDBNull(iInfverf2)) entity.Infverf2 = dr.GetDecimal(iInfverf2);

            int iInfverusucreacion = dr.GetOrdinal(this.Infverusucreacion);
            if (!dr.IsDBNull(iInfverusucreacion)) entity.Infverusucreacion = dr.GetString(iInfverusucreacion);

            int iInfverfeccreacion = dr.GetOrdinal(this.Infverfeccreacion);
            if (!dr.IsDBNull(iInfverfeccreacion)) entity.Infverfeccreacion = dr.GetDateTime(iInfverfeccreacion);

            int iInfvernro = dr.GetOrdinal(this.Infvernro);
            if (!dr.IsDBNull(iInfvernro)) entity.Infvernro = Convert.ToInt32(dr.GetValue(iInfvernro));

            int iInfvercumpl = dr.GetOrdinal(this.Infvercumpl);
            if (!dr.IsDBNull(iInfvercumpl)) entity.Infvercumpl = dr.GetDecimal(iInfvercumpl);

            int iInfverhorizonte = dr.GetOrdinal(this.Infverhorizonte);
            if (!dr.IsDBNull(iInfverhorizonte)) entity.Infverhorizonte = Convert.ToInt32(dr.GetValue(iInfverhorizonte));

            int iInfvermodulo = dr.GetOrdinal(this.Infvermodulo);
            if (!dr.IsDBNull(iInfvermodulo)) entity.Infvermodulo = Convert.ToInt32(dr.GetValue(iInfvermodulo));

            return entity;
        }

        #region Mapeo de Campos

        public string Infvercodi = "INFVERCODI";
        public string Infverfechaperiodo = "INFVERFECHAPERIODO";
        public string Infvertipoeq = "INFVERTIPOEQ";
        public string Infverdisp = "INFVERDISP";
        public string Infverflagfinal = "INFVERFLAGFINAL";
        public string Infverflagdefinitivo = "INFVERFLAGDEFINITIVO";
        public string Infverf1 = "INFVERF1";
        public string Infverf2 = "INFVERF2";
        public string Infverusucreacion = "INFVERUSUCREACION";
        public string Infverfeccreacion = "INFVERFECCREACION";
        public string Infvernro = "INFVERNRO";
        public string Infvercumpl = "INFVERCUMPL";
        public string Infverhorizonte = "INFVERHORIZONTE";
        public string Infvermodulo = "INFVERMODULO";

        #endregion

        public string SqlGetByFecha
        {
            get { return GetSqlXml("GetByFecha"); }
        }

        public string SqlUpdateByFecha
        {
            get { return GetSqlXml("UpdateByFecha"); }
        }

    }
}
