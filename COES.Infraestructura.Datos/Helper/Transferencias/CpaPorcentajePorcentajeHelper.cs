using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_PORCENTAJE_PORCENTAJE
    /// </summary>
    public class CpaPorcentajePorcentajeHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Cpappcodi = "CPAPPCODI";
        public string Cpapcodi = "CPAPCODI";
        public string Cparcodi = "CPARCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cpappgentotene = "CPAPPGENTOTENE";
        public string Cpappgentotpot = "CPAPPGENTOTPOT";
        public string Cpappdistotene = "CPAPPDISTOTENE";
        public string Cpappdistotpot = "CPAPPDISTOTPOT";
        public string Cpappultotene = "CPAPPULTOTENE";
        public string Cpappultotpot = "CPAPPULTOTPOT";
        public string Cpapptratot = "CPAPPTRATOT";
        public string Cpapptotal = "CPAPPTOTAL";
        public string Cpappporcentaje = "CPAPPPORCENTAJE";
        public string Cpappusucreacion = "CPAPPUSUCREACION";
        public string Cpappfeccreacion = "CPAPPFECCREACION";

        //Additional
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Emprnomb = "EMPRNOMB";
        public string Emprruc = "EMPRRUC";
        #endregion

        public CpaPorcentajePorcentajeHelper() : base(Consultas.CpaPorcentajePorcentajeSql)
        {
        }

        public CpaPorcentajePorcentajeDTO Create(IDataReader dr)
        {
            CpaPorcentajePorcentajeDTO entity = new CpaPorcentajePorcentajeDTO();

            int iCpappcodi = dr.GetOrdinal(Cpappcodi);
            if (!dr.IsDBNull(iCpappcodi)) entity.Cpappcodi = dr.GetInt32(iCpappcodi);

            int iCpapcodi = dr.GetOrdinal(Cpapcodi);
            if (!dr.IsDBNull(iCpapcodi)) entity.Cpapcodi = dr.GetInt32(iCpapcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iEmprcodi = dr.GetOrdinal(Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iCpappgentotene = dr.GetOrdinal(Cpappgentotene);
            if (!dr.IsDBNull(iCpappgentotene)) entity.Cpappgentotene = dr.GetDecimal(iCpappgentotene);

            int iCpappgentotpot = dr.GetOrdinal(Cpappgentotpot);
            if (!dr.IsDBNull(iCpappgentotpot)) entity.Cpappgentotpot = dr.GetDecimal(iCpappgentotpot);

            int iCpappdistotene = dr.GetOrdinal(Cpappdistotene);
            if (!dr.IsDBNull(iCpappdistotene)) entity.Cpappdistotene = dr.GetDecimal(iCpappdistotene);

            int iCpappdistotpot = dr.GetOrdinal(Cpappdistotpot);
            if (!dr.IsDBNull(iCpappdistotpot)) entity.Cpappdistotpot = dr.GetDecimal(iCpappdistotpot);

            int iCpappultotene = dr.GetOrdinal(Cpappultotene);
            if (!dr.IsDBNull(iCpappultotene)) entity.Cpappultotene = dr.GetDecimal(iCpappultotene);

            int iCpappultotpot = dr.GetOrdinal(Cpappultotpot);
            if (!dr.IsDBNull(iCpappultotpot)) entity.Cpappultotpot = dr.GetDecimal(iCpappultotpot);

            int iCpapptratot = dr.GetOrdinal(Cpapptratot);
            if (!dr.IsDBNull(iCpapptratot)) entity.Cpapptratot = dr.GetDecimal(iCpapptratot);

            int iCpapptotal = dr.GetOrdinal(Cpapptotal);
            if (!dr.IsDBNull(iCpapptotal)) entity.Cpapptotal = dr.GetDecimal(iCpapptotal);

            int iCpappporcentaje = dr.GetOrdinal(Cpappporcentaje);
            if (!dr.IsDBNull(iCpappporcentaje)) entity.Cpappporcentaje = dr.GetDecimal(iCpappporcentaje);

            int iCpappusucreacion = dr.GetOrdinal(Cpappusucreacion);
            if (!dr.IsDBNull(iCpappusucreacion)) entity.Cpappusucreacion = dr.GetString(iCpappusucreacion);

            int iCpappfeccreacion = dr.GetOrdinal(Cpappfeccreacion);
            if (!dr.IsDBNull(iCpappfeccreacion)) entity.Cpappfeccreacion = dr.GetDateTime(iCpappfeccreacion);

            return entity;
        }

        public string SqlDeleteByRevision
        {
            get { return base.GetSqlXml("DeleteByRevision"); }
        }

        public string SqlListByRevision
        {
            get { return base.GetSqlXml("ListByRevision"); }
        }
    }
}

