using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_INSUMO_MES
    /// </summary>
    public class CpaInsumoMesHelper : HelperBase
    {
        public CpaInsumoMesHelper() : base(Consultas.CpaInsumoMesSql)
        {
        }

        public CpaInsumoMesDTO Create(IDataReader dr)
        {
            CpaInsumoMesDTO entity = new CpaInsumoMesDTO();

            int iCpainmcodi = dr.GetOrdinal(Cpainmcodi);
            if (!dr.IsDBNull(iCpainmcodi)) entity.Cpainmcodi = dr.GetInt32(iCpainmcodi);

            int iCpainscodi = dr.GetOrdinal(Cpainscodi);
            if (!dr.IsDBNull(iCpainscodi)) entity.Cpainscodi = dr.GetInt32(iCpainscodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iEmprcodi = dr.GetOrdinal(Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iEquicodi = dr.GetOrdinal(Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

            int iCpainmtipinsumo = dr.GetOrdinal(Cpainmtipinsumo);
            if (!dr.IsDBNull(iCpainmtipinsumo)) entity.Cpainmtipinsumo = dr.GetString(iCpainmtipinsumo);

            int iCpainmtipproceso = dr.GetOrdinal(Cpainmtipproceso);
            if (!dr.IsDBNull(iCpainmtipproceso)) entity.Cpainmtipproceso = dr.GetString(iCpainmtipproceso);

            int iCpainmmes = dr.GetOrdinal(Cpainmmes);
            if (!dr.IsDBNull(iCpainmmes)) entity.Cpainmmes = dr.GetInt32(iCpainmmes);

            int iCpainmtotal = dr.GetOrdinal(Cpainmtotal);
            if (!dr.IsDBNull(iCpainmtotal)) entity.Cpainmtotal = dr.GetDecimal(iCpainmtotal);

            int iCpainmusucreacion = dr.GetOrdinal(Cpainmusucreacion);
            if (!dr.IsDBNull(iCpainmusucreacion)) entity.Cpainmusucreacion = dr.GetString(iCpainmusucreacion);

            int iCpainmfeccreacion = dr.GetOrdinal(Cpainmfeccreacion);
            if (!dr.IsDBNull(iCpainmfeccreacion)) entity.Cpainmfeccreacion = dr.GetDateTime(iCpainmfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpainmcodi = "CPAINMCODI";
        public string Cpainscodi = "CPAINSCODI";
        public string Cparcodi = "CPARCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Cpainmtipinsumo = "CPAINMTIPINSUMO";
        public string Cpainmtipproceso = "CPAINMTIPPROCESO";
        public string Cpainmmes = "CPAINMMES";
        public string Cpainmtotal = "CPAINMTOTAL";
        public string Cpainmusucreacion = "CPAINMUSUCREACION";
        public string Cpainmfeccreacion = "CPAINMFECCREACION";
        #endregion

        //Metodos complementarios:
        public string SqlDeleteByCentral
        {
            get { return base.GetSqlXml("DeleteByCentral"); }
        }
        public string SqlDeleteByRevision
        {
            get { return base.GetSqlXml("DeleteByRevision"); }
        }
        public string SqlUpdateInsumoMesTotal {
            get { return base.GetSqlXml("UpdateInsumoMesTotal"); }
        }
    }
}
