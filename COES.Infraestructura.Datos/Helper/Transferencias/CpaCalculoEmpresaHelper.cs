using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;
using System.Web;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_CALCULO_EMPRESA
    /// </summary>
    public class CpaCalculoEmpresaHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Cpacecodi = "CPACECODI";
        public string Cpaccodi = "CPACCODI";
        public string Cparcodi = "CPARCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cpacetipo = "CPACETIPO";
        public string Cpacemes = "CPACEMES";
        public string Cpacetotenemwh = "CPACETOTENEMWH";
        public string Cpacetotenesoles = "CPACETOTENESOLES";
        public string Cpacetotpotmwh = "CPACETOTPOTMWH";
        public string Cpacetotpotsoles = "CPACETOTPOTSOLES";
        public string Cpaceusucreacion = "CPACEUSUCREACION";
        public string Cpacefeccreacion = "CPACEFECCREACION";

        //Additional
        public string Emprnomb = "EMPRNOMB";
        #endregion

        public CpaCalculoEmpresaHelper() : base(Consultas.CpaCalculoEmpresaSql)
        {
        }

        public CpaCalculoEmpresaDTO Create(IDataReader dr)
        {
            CpaCalculoEmpresaDTO entity = new CpaCalculoEmpresaDTO();

            int iCpacecodi = dr.GetOrdinal(Cpacecodi);
            if (!dr.IsDBNull(iCpacecodi)) entity.Cpacecodi = dr.GetInt32(iCpacecodi);

            int iCpaccodi = dr.GetOrdinal(Cpaccodi);
            if (!dr.IsDBNull(iCpaccodi)) entity.Cpaccodi = dr.GetInt32(iCpaccodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iEmprcodi = dr.GetOrdinal(Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iCpacetipo = dr.GetOrdinal(Cpacetipo);
            if (!dr.IsDBNull(iCpacetipo)) entity.Cpacetipo = dr.GetString(iCpacetipo);

            int iCpacemes = dr.GetOrdinal(Cpacemes);
            if (!dr.IsDBNull(iCpacemes)) entity.Cpacemes = dr.GetInt32(iCpacemes);

            int iCpacetotenemwh = dr.GetOrdinal(Cpacetotenemwh);
            if (!dr.IsDBNull(iCpacetotenemwh)) entity.Cpacetotenemwh = dr.GetDecimal(iCpacetotenemwh);

            int iCpacetotenesoles = dr.GetOrdinal(Cpacetotenesoles);
            if (!dr.IsDBNull(iCpacetotenesoles)) entity.Cpacetotenesoles = dr.GetDecimal(iCpacetotenesoles);

            int iCpacetotpotmwh = dr.GetOrdinal(Cpacetotpotmwh);
            if (!dr.IsDBNull(iCpacetotpotmwh)) entity.Cpacetotpotmwh = dr.GetDecimal(iCpacetotpotmwh);

            int iCpacetotpotsoles = dr.GetOrdinal(Cpacetotpotsoles);
            if (!dr.IsDBNull(iCpacetotpotsoles)) entity.Cpacetotpotsoles = dr.GetDecimal(iCpacetotpotsoles);

            int iCpaceusucreacion = dr.GetOrdinal(Cpaceusucreacion);
            if (!dr.IsDBNull(iCpaceusucreacion)) entity.Cpaceusucreacion = dr.GetString(iCpaceusucreacion);

            int iCpacefeccreacion = dr.GetOrdinal(Cpacefeccreacion);
            if (!dr.IsDBNull(iCpacefeccreacion)) entity.Cpacefeccreacion = dr.GetDateTime(iCpacefeccreacion);

            return entity;
        }

        public string SqlDeleteByRevision
        {
            get { return base.GetSqlXml("DeleteByRevision"); }
        }
    }
}
