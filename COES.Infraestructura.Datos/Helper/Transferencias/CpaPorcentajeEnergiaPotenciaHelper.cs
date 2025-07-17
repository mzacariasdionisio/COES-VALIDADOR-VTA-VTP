using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_PORCENTAJE_ENERGIAPOTENCIA
    /// </summary>
    public class CpaPorcentajeEnergiaPotenciaHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Cpapepcodi = "CPAPEPCODI";
        public string Cpapcodi = "CPAPCODI";
        public string Cparcodi = "CPARCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cpapepenemes01 = "CPAPEPENEMES01";
        public string Cpapepenemes02 = "CPAPEPENEMES02";
        public string Cpapepenemes03 = "CPAPEPENEMES03";
        public string Cpapepenemes04 = "CPAPEPENEMES04";
        public string Cpapepenemes05 = "CPAPEPENEMES05";
        public string Cpapepenemes06 = "CPAPEPENEMES06";
        public string Cpapepenemes07 = "CPAPEPENEMES07";
        public string Cpapepenemes08 = "CPAPEPENEMES08";
        public string Cpapepenemes09 = "CPAPEPENEMES09";
        public string Cpapepenemes10 = "CPAPEPENEMES10";
        public string Cpapepenemes11 = "CPAPEPENEMES11";
        public string Cpapepenemes12 = "CPAPEPENEMES12";
        public string Cpapepenetotal = "CPAPEPENETOTAL";
        public string Cpapeppotmes01 = "CPAPEPPOTMES01";
        public string Cpapeppotmes02 = "CPAPEPPOTMES02";
        public string Cpapeppotmes03 = "CPAPEPPOTMES03";
        public string Cpapeppotmes04 = "CPAPEPPOTMES04";
        public string Cpapeppotmes05 = "CPAPEPPOTMES05";
        public string Cpapeppotmes06 = "CPAPEPPOTMES06";
        public string Cpapeppotmes07 = "CPAPEPPOTMES07";
        public string Cpapeppotmes08 = "CPAPEPPOTMES08";
        public string Cpapeppotmes09 = "CPAPEPPOTMES09";
        public string Cpapeppotmes10 = "CPAPEPPOTMES10";
        public string Cpapeppotmes11 = "CPAPEPPOTMES11";
        public string Cpapeppotmes12 = "CPAPEPPOTMES12";
        public string Cpapeppottotal = "CPAPEPPOTTOTAL";
        public string Cpapepusucreacion = "CPAPEPUSUCREACION";
        public string Cpapepfeccreacion = "CPAPEPFECCREACION";

        //Additional
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Emprnomb = "EMPRNOMB";
        public string Emprruc = "EMPRRUC";
        #endregion

        public CpaPorcentajeEnergiaPotenciaHelper() : base(Consultas.CpaPorcentajeEnergiaPotenciaSql)
        {
        }

        public CpaPorcentajeEnergiaPotenciaDTO Create(IDataReader dr)
        {
            CpaPorcentajeEnergiaPotenciaDTO entity = new CpaPorcentajeEnergiaPotenciaDTO();

            int iCpapepcodi = dr.GetOrdinal(Cpapepcodi);
            if (!dr.IsDBNull(iCpapepcodi)) entity.Cpapepcodi = dr.GetInt32(iCpapepcodi);

            int iCpapcodi = dr.GetOrdinal(Cpapcodi);
            if (!dr.IsDBNull(iCpapcodi)) entity.Cpapcodi = dr.GetInt32(iCpapcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iEmprcodi = dr.GetOrdinal(Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iCpapepenemes01 = dr.GetOrdinal(Cpapepenemes01);
            if (!dr.IsDBNull(iCpapepenemes01)) entity.Cpapepenemes01 = dr.GetDecimal(iCpapepenemes01);

            int iCpapepenemes02 = dr.GetOrdinal(Cpapepenemes02);
            if (!dr.IsDBNull(iCpapepenemes02)) entity.Cpapepenemes02 = dr.GetDecimal(iCpapepenemes02);

            int iCpapepenemes03 = dr.GetOrdinal(Cpapepenemes03);
            if (!dr.IsDBNull(iCpapepenemes03)) entity.Cpapepenemes03 = dr.GetDecimal(iCpapepenemes03);

            int iCpapepenemes04 = dr.GetOrdinal(Cpapepenemes04);
            if (!dr.IsDBNull(iCpapepenemes04)) entity.Cpapepenemes04 = dr.GetDecimal(iCpapepenemes04);

            int iCpapepenemes05 = dr.GetOrdinal(Cpapepenemes05);
            if (!dr.IsDBNull(iCpapepenemes05)) entity.Cpapepenemes05 = dr.GetDecimal(iCpapepenemes05);

            int iCpapepenemes06 = dr.GetOrdinal(Cpapepenemes06);
            if (!dr.IsDBNull(iCpapepenemes06)) entity.Cpapepenemes06 = dr.GetDecimal(iCpapepenemes06);

            int iCpapepenemes07 = dr.GetOrdinal(Cpapepenemes07);
            if (!dr.IsDBNull(iCpapepenemes07)) entity.Cpapepenemes07 = dr.GetDecimal(iCpapepenemes07);

            int iCpapepenemes08 = dr.GetOrdinal(Cpapepenemes08);
            if (!dr.IsDBNull(iCpapepenemes08)) entity.Cpapepenemes08 = dr.GetDecimal(iCpapepenemes08);

            int iCpapepenemes09 = dr.GetOrdinal(Cpapepenemes09);
            if (!dr.IsDBNull(iCpapepenemes09)) entity.Cpapepenemes09 = dr.GetDecimal(iCpapepenemes09);

            int iCpapepenemes10 = dr.GetOrdinal(Cpapepenemes10);
            if (!dr.IsDBNull(iCpapepenemes10)) entity.Cpapepenemes10 = dr.GetDecimal(iCpapepenemes10);

            int iCpapepenemes11 = dr.GetOrdinal(Cpapepenemes11);
            if (!dr.IsDBNull(iCpapepenemes11)) entity.Cpapepenemes11 = dr.GetDecimal(iCpapepenemes11);

            int iCpapepenemes12 = dr.GetOrdinal(Cpapepenemes12);
            if (!dr.IsDBNull(iCpapepenemes12)) entity.Cpapepenemes12 = dr.GetDecimal(iCpapepenemes12);

            int iCpapepenetotal = dr.GetOrdinal(Cpapepenetotal);
            if (!dr.IsDBNull(iCpapepenetotal)) entity.Cpapepenetotal = dr.GetDecimal(iCpapepenetotal);

            int iCpapeppotmes01 = dr.GetOrdinal(Cpapeppotmes01);
            if (!dr.IsDBNull(iCpapeppotmes01)) entity.Cpapeppotmes01 = dr.GetDecimal(iCpapeppotmes01);

            int iCpapeppotmes02 = dr.GetOrdinal(Cpapeppotmes02);
            if (!dr.IsDBNull(iCpapeppotmes02)) entity.Cpapeppotmes02 = dr.GetDecimal(iCpapeppotmes02);

            int iCpapeppotmes03 = dr.GetOrdinal(Cpapeppotmes03);
            if (!dr.IsDBNull(iCpapeppotmes03)) entity.Cpapeppotmes03 = dr.GetDecimal(iCpapeppotmes03);

            int iCpapeppotmes04 = dr.GetOrdinal(Cpapeppotmes04);
            if (!dr.IsDBNull(iCpapeppotmes04)) entity.Cpapeppotmes04 = dr.GetDecimal(iCpapeppotmes04);

            int iCpapeppotmes05 = dr.GetOrdinal(Cpapeppotmes05);
            if (!dr.IsDBNull(iCpapeppotmes05)) entity.Cpapeppotmes05 = dr.GetDecimal(iCpapeppotmes05);

            int iCpapeppotmes06 = dr.GetOrdinal(Cpapeppotmes06);
            if (!dr.IsDBNull(iCpapeppotmes06)) entity.Cpapeppotmes06 = dr.GetDecimal(iCpapeppotmes06);

            int iCpapeppotmes07 = dr.GetOrdinal(Cpapeppotmes07);
            if (!dr.IsDBNull(iCpapeppotmes07)) entity.Cpapeppotmes07 = dr.GetDecimal(iCpapeppotmes07);

            int iCpapeppotmes08 = dr.GetOrdinal(Cpapeppotmes08);
            if (!dr.IsDBNull(iCpapeppotmes08)) entity.Cpapeppotmes08 = dr.GetDecimal(iCpapeppotmes08);

            int iCpapeppotmes09 = dr.GetOrdinal(Cpapeppotmes09);
            if (!dr.IsDBNull(iCpapeppotmes09)) entity.Cpapeppotmes09 = dr.GetDecimal(iCpapeppotmes09);

            int iCpapeppotmes10 = dr.GetOrdinal(Cpapeppotmes10);
            if (!dr.IsDBNull(iCpapeppotmes10)) entity.Cpapeppotmes10 = dr.GetDecimal(iCpapeppotmes10);

            int iCpapeppotmes11 = dr.GetOrdinal(Cpapeppotmes11);
            if (!dr.IsDBNull(iCpapeppotmes11)) entity.Cpapeppotmes11 = dr.GetDecimal(iCpapeppotmes11);

            int iCpapeppotmes12 = dr.GetOrdinal(Cpapeppotmes12);
            if (!dr.IsDBNull(iCpapeppotmes12)) entity.Cpapeppotmes12 = dr.GetDecimal(iCpapeppotmes12);

            int iCpapeppottotal = dr.GetOrdinal(Cpapeppottotal);
            if (!dr.IsDBNull(iCpapeppottotal)) entity.Cpapeppottotal = dr.GetDecimal(iCpapeppottotal);

            int iCpapepusucreacion = dr.GetOrdinal(Cpapepusucreacion);
            if (!dr.IsDBNull(iCpapepusucreacion)) entity.Cpapepusucreacion = dr.GetString(iCpapepusucreacion);

            int iCpapepfeccreacion = dr.GetOrdinal(Cpapepfeccreacion);
            if (!dr.IsDBNull(iCpapepfeccreacion)) entity.Cpapepfeccreacion = dr.GetDateTime(iCpapepfeccreacion);

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

