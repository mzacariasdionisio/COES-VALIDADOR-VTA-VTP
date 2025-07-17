using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_INSUMO
    /// </summary>
    public class CpaInsumoHelper : HelperBase
    {
        public CpaInsumoHelper() : base(Consultas.CpaInsumoSql)
        {
        }

        public CpaInsumoDTO Create(IDataReader dr)
        {
            CpaInsumoDTO entity = new CpaInsumoDTO();

            int iCpainscodi = dr.GetOrdinal(Cpainscodi);
            if (!dr.IsDBNull(iCpainscodi)) entity.Cpainscodi = dr.GetInt32(iCpainscodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iCpainstipinsumo = dr.GetOrdinal(Cpainstipinsumo);
            if (!dr.IsDBNull(iCpainstipinsumo)) entity.Cpainstipinsumo = dr.GetString(iCpainstipinsumo);

            int iCpainstipproceso = dr.GetOrdinal(Cpainstipproceso);
            if (!dr.IsDBNull(iCpainstipproceso)) entity.Cpainstipproceso = dr.GetString(iCpainstipproceso);

            int iCpainslog = dr.GetOrdinal(Cpainslog);
            if (!dr.IsDBNull(iCpainslog)) entity.Cpainslog = dr.GetString(iCpainslog);

            int iCpainsusucreacion = dr.GetOrdinal(Cpainsusucreacion);
            if (!dr.IsDBNull(iCpainsusucreacion)) entity.Cpainsusucreacion = dr.GetString(iCpainsusucreacion);

            int iCpainsfeccreacion = dr.GetOrdinal(Cpainsfeccreacion);
            if (!dr.IsDBNull(iCpainsfeccreacion)) entity.Cpainsfeccreacion = dr.GetDateTime(iCpainsfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpainscodi = "CPAINSCODI";
        public string Cparcodi = "CPARCODI";
        public string Cpainstipinsumo = "CPAINSTIPINSUMO";
        public string Cpainstipproceso = "CPAINSTIPPROCESO";
        public string Cpainslog = "CPAINSLOG";
        public string Cpainsusucreacion = "CPAINSUSUCREACION";
        public string Cpainsfeccreacion = "CPAINSFECCREACION";
        #endregion

        #region Querys
        public string SqlGetByCparcodiByCpainstipinsumo
        {
            get { return base.GetSqlXml("GetByCparcodiByCpainstipinsumo"); }
        }

        #endregion
    }
}
