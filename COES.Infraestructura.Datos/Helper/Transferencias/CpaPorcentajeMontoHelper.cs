using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_PORCENTAJE_MONTO
    /// </summary>
    public class CpaPorcentajeMontoHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Cpapmtcodi = "CPAPMTCODI";
        public string Cpapcodi = "CPAPCODI";
        public string Cparcodi = "CPARCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cpapmtenemes01 = "CPAPMTENEMES01";
        public string Cpapmtenemes02 = "CPAPMTENEMES02";
        public string Cpapmtenemes03 = "CPAPMTENEMES03";
        public string Cpapmtenemes04 = "CPAPMTENEMES04";
        public string Cpapmtenemes05 = "CPAPMTENEMES05";
        public string Cpapmtenemes06 = "CPAPMTENEMES06";
        public string Cpapmtenemes07 = "CPAPMTENEMES07";
        public string Cpapmtenemes08 = "CPAPMTENEMES08";
        public string Cpapmtenemes09 = "CPAPMTENEMES09";
        public string Cpapmtenemes10 = "CPAPMTENEMES10";
        public string Cpapmtenemes11 = "CPAPMTENEMES11";
        public string Cpapmtenemes12 = "CPAPMTENEMES12";
        public string Cpapmtenetotal = "CPAPMTENETOTAL";
        public string Cpapmtpotmes01 = "CPAPMTPOTMES01";
        public string Cpapmtpotmes02 = "CPAPMTPOTMES02";
        public string Cpapmtpotmes03 = "CPAPMTPOTMES03";
        public string Cpapmtpotmes04 = "CPAPMTPOTMES04";
        public string Cpapmtpotmes05 = "CPAPMTPOTMES05";
        public string Cpapmtpotmes06 = "CPAPMTPOTMES06";
        public string Cpapmtpotmes07 = "CPAPMTPOTMES07";
        public string Cpapmtpotmes08 = "CPAPMTPOTMES08";
        public string Cpapmtpotmes09 = "CPAPMTPOTMES09";
        public string Cpapmtpotmes10 = "CPAPMTPOTMES10";
        public string Cpapmtpotmes11 = "CPAPMTPOTMES11";
        public string Cpapmtpotmes12 = "CPAPMTPOTMES12";
        public string Cpapmtpottotal = "CPAPMTPOTTOTAL";
        public string Cpapmttrames01 = "CPAPMTTRAMES01";
        public string Cpapmttrames02 = "CPAPMTTRAMES02";
        public string Cpapmttrames03 = "CPAPMTTRAMES03";
        public string Cpapmttrames04 = "CPAPMTTRAMES04";
        public string Cpapmttrames05 = "CPAPMTTRAMES05";
        public string Cpapmttrames06 = "CPAPMTTRAMES06";
        public string Cpapmttrames07 = "CPAPMTTRAMES07";
        public string Cpapmttrames08 = "CPAPMTTRAMES08";
        public string Cpapmttrames09 = "CPAPMTTRAMES09";
        public string Cpapmttrames10 = "CPAPMTTRAMES10";
        public string Cpapmttrames11 = "CPAPMTTRAMES11";
        public string Cpapmttrames12 = "CPAPMTTRAMES12";
        public string Cpapmttratotal = "CPAPMTTRATOTAL";
        public string Cpapmtusucreacion = "CPAPMTUSUCREACION";
        public string Cpapmtfeccreacion = "CPAPMTFECCREACION";

        //Additional
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Emprnomb = "EMPRNOMB";
        public string Emprruc = "EMPRRUC";
        #endregion

        public CpaPorcentajeMontoHelper() : base(Consultas.CpaPorcentajeMontoSql)
        {
        }

        public CpaPorcentajeMontoDTO Create(IDataReader dr)
        {
            CpaPorcentajeMontoDTO entity = new CpaPorcentajeMontoDTO();

            int iCpapmtcodi = dr.GetOrdinal(Cpapmtcodi);
            if (!dr.IsDBNull(iCpapmtcodi)) entity.Cpapmtcodi = dr.GetInt32(iCpapmtcodi);

            int iCpapcodi = dr.GetOrdinal(Cpapcodi);
            if (!dr.IsDBNull(iCpapcodi)) entity.Cpapcodi = dr.GetInt32(iCpapcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iEmprcodi = dr.GetOrdinal(Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iCpapmtenemes01 = dr.GetOrdinal(Cpapmtenemes01);
            if (!dr.IsDBNull(iCpapmtenemes01)) entity.Cpapmtenemes01 = dr.GetDecimal(iCpapmtenemes01);

            int iCpapmtenemes02 = dr.GetOrdinal(Cpapmtenemes02);
            if (!dr.IsDBNull(iCpapmtenemes02)) entity.Cpapmtenemes02 = dr.GetDecimal(iCpapmtenemes02);

            int iCpapmtenemes03 = dr.GetOrdinal(Cpapmtenemes03);
            if (!dr.IsDBNull(iCpapmtenemes03)) entity.Cpapmtenemes03 = dr.GetDecimal(iCpapmtenemes03);

            int iCpapmtenemes04 = dr.GetOrdinal(Cpapmtenemes04);
            if (!dr.IsDBNull(iCpapmtenemes04)) entity.Cpapmtenemes04 = dr.GetDecimal(iCpapmtenemes04);

            int iCpapmtenemes05 = dr.GetOrdinal(Cpapmtenemes05);
            if (!dr.IsDBNull(iCpapmtenemes05)) entity.Cpapmtenemes05 = dr.GetDecimal(iCpapmtenemes05);

            int iCpapmtenemes06 = dr.GetOrdinal(Cpapmtenemes06);
            if (!dr.IsDBNull(iCpapmtenemes06)) entity.Cpapmtenemes06 = dr.GetDecimal(iCpapmtenemes06);

            int iCpapmtenemes07 = dr.GetOrdinal(Cpapmtenemes07);
            if (!dr.IsDBNull(iCpapmtenemes07)) entity.Cpapmtenemes07 = dr.GetDecimal(iCpapmtenemes07);

            int iCpapmtenemes08 = dr.GetOrdinal(Cpapmtenemes08);
            if (!dr.IsDBNull(iCpapmtenemes08)) entity.Cpapmtenemes08 = dr.GetDecimal(iCpapmtenemes08);

            int iCpapmtenemes09 = dr.GetOrdinal(Cpapmtenemes09);
            if (!dr.IsDBNull(iCpapmtenemes09)) entity.Cpapmtenemes09 = dr.GetDecimal(iCpapmtenemes09);

            int iCpapmtenemes10 = dr.GetOrdinal(Cpapmtenemes10);
            if (!dr.IsDBNull(iCpapmtenemes10)) entity.Cpapmtenemes10 = dr.GetDecimal(iCpapmtenemes10);

            int iCpapmtenemes11 = dr.GetOrdinal(Cpapmtenemes11);
            if (!dr.IsDBNull(iCpapmtenemes11)) entity.Cpapmtenemes11 = dr.GetDecimal(iCpapmtenemes11);

            int iCpapmtenemes12 = dr.GetOrdinal(Cpapmtenemes12);
            if (!dr.IsDBNull(iCpapmtenemes12)) entity.Cpapmtenemes12 = dr.GetDecimal(iCpapmtenemes12);

            int iCpapmtenetotal = dr.GetOrdinal(Cpapmtenetotal);
            if (!dr.IsDBNull(iCpapmtenetotal)) entity.Cpapmtenetotal = dr.GetDecimal(iCpapmtenetotal);

            int iCpapmtpotmes01 = dr.GetOrdinal(Cpapmtpotmes01);
            if (!dr.IsDBNull(iCpapmtpotmes01)) entity.Cpapmtpotmes01 = dr.GetDecimal(iCpapmtpotmes01);

            int iCpapmtpotmes02 = dr.GetOrdinal(Cpapmtpotmes02);
            if (!dr.IsDBNull(iCpapmtpotmes02)) entity.Cpapmtpotmes02 = dr.GetDecimal(iCpapmtpotmes02);

            int iCpapmtpotmes03 = dr.GetOrdinal(Cpapmtpotmes03);
            if (!dr.IsDBNull(iCpapmtpotmes03)) entity.Cpapmtpotmes03 = dr.GetDecimal(iCpapmtpotmes03);

            int iCpapmtpotmes04 = dr.GetOrdinal(Cpapmtpotmes04);
            if (!dr.IsDBNull(iCpapmtpotmes04)) entity.Cpapmtpotmes04 = dr.GetDecimal(iCpapmtpotmes04);

            int iCpapmtpotmes05 = dr.GetOrdinal(Cpapmtpotmes05);
            if (!dr.IsDBNull(iCpapmtpotmes05)) entity.Cpapmtpotmes05 = dr.GetDecimal(iCpapmtpotmes05);

            int iCpapmtpotmes06 = dr.GetOrdinal(Cpapmtpotmes06);
            if (!dr.IsDBNull(iCpapmtpotmes06)) entity.Cpapmtpotmes06 = dr.GetDecimal(iCpapmtpotmes06);

            int iCpapmtpotmes07 = dr.GetOrdinal(Cpapmtpotmes07);
            if (!dr.IsDBNull(iCpapmtpotmes07)) entity.Cpapmtpotmes07 = dr.GetDecimal(iCpapmtpotmes07);

            int iCpapmtpotmes08 = dr.GetOrdinal(Cpapmtpotmes08);
            if (!dr.IsDBNull(iCpapmtpotmes08)) entity.Cpapmtpotmes08 = dr.GetDecimal(iCpapmtpotmes08);

            int iCpapmtpotmes09 = dr.GetOrdinal(Cpapmtpotmes09);
            if (!dr.IsDBNull(iCpapmtpotmes09)) entity.Cpapmtpotmes09 = dr.GetDecimal(iCpapmtpotmes09);

            int iCpapmtpotmes10 = dr.GetOrdinal(Cpapmtpotmes10);
            if (!dr.IsDBNull(iCpapmtpotmes10)) entity.Cpapmtpotmes10 = dr.GetDecimal(iCpapmtpotmes10);

            int iCpapmtpotmes11 = dr.GetOrdinal(Cpapmtpotmes11);
            if (!dr.IsDBNull(iCpapmtpotmes11)) entity.Cpapmtpotmes11 = dr.GetDecimal(iCpapmtpotmes11);

            int iCpapmtpotmes12 = dr.GetOrdinal(Cpapmtpotmes12);
            if (!dr.IsDBNull(iCpapmtpotmes12)) entity.Cpapmtpotmes12 = dr.GetDecimal(iCpapmtpotmes12);

            int iCpapmtpottotal = dr.GetOrdinal(Cpapmtpottotal);
            if (!dr.IsDBNull(iCpapmtpottotal)) entity.Cpapmtpottotal = dr.GetDecimal(iCpapmtpottotal);

            int iCpapmttrames01 = dr.GetOrdinal(Cpapmttrames01);
            if (!dr.IsDBNull(iCpapmttrames01)) entity.Cpapmttrames01 = dr.GetDecimal(iCpapmttrames01);

            int iCpapmttrames02 = dr.GetOrdinal(Cpapmttrames02);
            if (!dr.IsDBNull(iCpapmttrames02)) entity.Cpapmttrames02 = dr.GetDecimal(iCpapmttrames02);

            int iCpapmttrames03 = dr.GetOrdinal(Cpapmttrames03);
            if (!dr.IsDBNull(iCpapmttrames03)) entity.Cpapmttrames03 = dr.GetDecimal(iCpapmttrames03);

            int iCpapmttrames04 = dr.GetOrdinal(Cpapmttrames04);
            if (!dr.IsDBNull(iCpapmttrames04)) entity.Cpapmttrames04 = dr.GetDecimal(iCpapmttrames04);

            int iCpapmttrames05 = dr.GetOrdinal(Cpapmttrames05);
            if (!dr.IsDBNull(iCpapmttrames05)) entity.Cpapmttrames05 = dr.GetDecimal(iCpapmttrames05);

            int iCpapmttrames06 = dr.GetOrdinal(Cpapmttrames06);
            if (!dr.IsDBNull(iCpapmttrames06)) entity.Cpapmttrames06 = dr.GetDecimal(iCpapmttrames06);

            int iCpapmttrames07 = dr.GetOrdinal(Cpapmttrames07);
            if (!dr.IsDBNull(iCpapmttrames07)) entity.Cpapmttrames07 = dr.GetDecimal(iCpapmttrames07);

            int iCpapmttrames08 = dr.GetOrdinal(Cpapmttrames08);
            if (!dr.IsDBNull(iCpapmttrames08)) entity.Cpapmttrames08 = dr.GetDecimal(iCpapmttrames08);

            int iCpapmttrames09 = dr.GetOrdinal(Cpapmttrames09);
            if (!dr.IsDBNull(iCpapmttrames09)) entity.Cpapmttrames09 = dr.GetDecimal(iCpapmttrames09);

            int iCpapmttrames10 = dr.GetOrdinal(Cpapmttrames10);
            if (!dr.IsDBNull(iCpapmttrames10)) entity.Cpapmttrames10 = dr.GetDecimal(iCpapmttrames10);

            int iCpapmttrames11 = dr.GetOrdinal(Cpapmttrames11);
            if (!dr.IsDBNull(iCpapmttrames11)) entity.Cpapmttrames11 = dr.GetDecimal(iCpapmttrames11);

            int iCpapmttrames12 = dr.GetOrdinal(Cpapmttrames12);
            if (!dr.IsDBNull(iCpapmttrames12)) entity.Cpapmttrames12 = dr.GetDecimal(iCpapmttrames12);

            int iCpapmttratotal = dr.GetOrdinal(Cpapmttratotal);
            if (!dr.IsDBNull(iCpapmttratotal)) entity.Cpapmttratotal = dr.GetDecimal(iCpapmttratotal);

            int iCpapmtusucreacion = dr.GetOrdinal(Cpapmtusucreacion);
            if (!dr.IsDBNull(iCpapmtusucreacion)) entity.Cpapmtusucreacion = dr.GetString(iCpapmtusucreacion);

            int iCpapmtfeccreacion = dr.GetOrdinal(Cpapmtfeccreacion);
            if (!dr.IsDBNull(iCpapmtfeccreacion)) entity.Cpapmtfeccreacion = dr.GetDateTime(iCpapmtfeccreacion);

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

