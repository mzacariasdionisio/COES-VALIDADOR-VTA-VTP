using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_CALCULO
    /// </summary>
    public class CpaCalculoHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Cpaccodi = "CPACCODI";
        public string Cparcodi = "CPARCODI";
        public string Cpaclog = "CPACLOG";
        public string Cpacusucreacion = "CPACUSUCREACION";
        public string Cpacfeccreacion = "CPACFECCREACION";
        #endregion

        public CpaCalculoHelper() : base(Consultas.CpaCalculoSql)
        {
        }

        public CpaCalculoDTO Create(IDataReader dr)
        {
            CpaCalculoDTO entity = new CpaCalculoDTO();

            int iCpaccodi = dr.GetOrdinal(Cpaccodi);
            if (!dr.IsDBNull(iCpaccodi)) entity.Cpaccodi = dr.GetInt32(iCpaccodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iCpaclog = dr.GetOrdinal(Cpaclog);
            if (!dr.IsDBNull(iCpaclog)) entity.Cpaclog = dr.GetString(iCpaclog);

            int iCpacusucreacion = dr.GetOrdinal(Cpacusucreacion);
            if (!dr.IsDBNull(iCpacusucreacion)) entity.Cpacusucreacion = dr.GetString(iCpacusucreacion);

            int iCpacfeccreacion = dr.GetOrdinal(Cpacfeccreacion);
            if (!dr.IsDBNull(iCpacfeccreacion)) entity.Cpacfeccreacion = dr.GetDateTime(iCpacfeccreacion);

            return entity;
        }

        public string SqlDeleteByRevision
        {
            get { return base.GetSqlXml("DeleteByRevision"); }
        }
        
    }
}
