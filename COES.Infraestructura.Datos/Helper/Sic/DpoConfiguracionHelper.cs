using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class DpoConfiguracionHelper : HelperBase
    {
        public DpoConfiguracionHelper() : base(Consultas.DpoConfiguracionSql)
        {
        }

        public DpoConfiguracionDTO Create(IDataReader dr)
        {
            DpoConfiguracionDTO entity = new DpoConfiguracionDTO();

            int iDpocngcodi = dr.GetOrdinal(this.Dpocngcodi);
            if (!dr.IsDBNull(iDpocngcodi)) entity.Dpocngcodi = Convert.ToInt32(dr.GetValue(iDpocngcodi));
            
            int iVergrpcodi = dr.GetOrdinal(this.Vergrpcodi);
            if (!dr.IsDBNull(iVergrpcodi)) entity.Vergrpcodi = Convert.ToInt32(dr.GetValue(iVergrpcodi));

            int iDpocngdias = dr.GetOrdinal(this.Dpocngdias);
            if (!dr.IsDBNull(iDpocngdias)) entity.Dpocngdias = Convert.ToInt32(dr.GetValue(iDpocngdias));

            int iDpocngpromedio = dr.GetOrdinal(this.Dpocngpromedio);
            if (!dr.IsDBNull(iDpocngpromedio)) entity.Dpocngpromedio = Convert.ToInt32(dr.GetValue(iDpocngpromedio));

            int iDpocngtendencia = dr.GetOrdinal(this.Dpocngtendencia);
            if (!dr.IsDBNull(iDpocngtendencia)) entity.Dpocngtendencia = dr.GetDecimal(iDpocngtendencia);

            int iDpocnggaussiano = dr.GetOrdinal(this.Dpocnggaussiano);
            if (!dr.IsDBNull(iDpocnggaussiano)) entity.Dpocnggaussiano = dr.GetDecimal(iDpocnggaussiano);
            
            int iDpocngumbral = dr.GetOrdinal(this.Dpocngumbral);
            if (!dr.IsDBNull(iDpocngumbral)) entity.Dpocngumbral = dr.GetDecimal(iDpocngumbral);

            int iDpocngvmg = dr.GetOrdinal(this.Dpocngvmg);
            if (!dr.IsDBNull(iDpocngvmg)) entity.Dpocngvmg = Convert.ToInt32(dr.GetValue(iDpocngvmg));

            int iDpocngstd = dr.GetOrdinal(this.Dpocngstd);
            if (!dr.IsDBNull(iDpocngstd)) entity.Dpocngstd = dr.GetDecimal(iDpocngstd);

            int iDpocngfechora = dr.GetOrdinal(this.Dpocngfechora);
            if (!dr.IsDBNull(iDpocngfechora)) entity.Dpocngfechora = dr.GetString(iDpocngfechora);

            int iDpocngusucreacion = dr.GetOrdinal(this.Dpocngusucreacion);
            if (!dr.IsDBNull(iDpocngusucreacion)) entity.Dpocngusucreacion = dr.GetString(iDpocngusucreacion);

            int iDpocngfeccreacion = dr.GetOrdinal(this.Dpocngfeccreacion);
            if (!dr.IsDBNull(iDpocngfeccreacion)) entity.Dpocngfeccreacion = dr.GetDateTime(iDpocngfeccreacion);

            int iDpocngusumodificacion = dr.GetOrdinal(this.Dpocngusumodificacion);
            if (!dr.IsDBNull(iDpocngusumodificacion)) entity.Dpocngusumodificacion = dr.GetString(iDpocngusumodificacion);

            int iDpocngfecmodificacion = dr.GetOrdinal(this.Dpocngfecmodificacion);
            if (!dr.IsDBNull(iDpocngfecmodificacion)) entity.Dpocngfecmodificacion = dr.GetDateTime(iDpocngfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Dpocngcodi = "DPOCNGCODI";
        public string Vergrpcodi = "VERGRPCODI";
        public string Dpocngdias = "DPOCNGDIAS";
        public string Dpocngpromedio = "DPOCNGPROMEDIO";
        public string Dpocngtendencia = "DPOCNGTENDENCIA";
        public string Dpocnggaussiano = "DPOCNGGAUSSIANO";
        public string Dpocngusucreacion = "DPOCNGUSUCREACION";
        public string Dpocngfeccreacion = "DPOCNGFECCREACION";
        public string Dpocngusumodificacion = "DPOCNGUSUMODIFICACION";
        public string Dpocngfecmodificacion = "DPOCNGFECMODIFICACION";

        public string Dpocngumbral = "DPOCNGUMBRAL";
        public string Dpocngvmg = "DPOCNGVMG";
        public string Dpocngstd = "DPOCNGSTD";
        public string Dpocngfechora = "DPOCNGFECHORA";

        #endregion

        #region Consultas DB
        public string SqlGetMaxId
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }
        public string SqlGetById
        {
            get { return base.GetSqlXml("GetById"); }
        }
        public string SqlGetByVersion
        {
            get { return base.GetSqlXml("GetByVersion"); }
        }
        #endregion
    }
}
