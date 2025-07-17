using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SH_CAUDAL
    /// </summary>
    public class SHCaudalHelper : HelperBase
    {
        public SHCaudalHelper()
            : base(Consultas.SHCaudalSql)
        {
        }

        public SHCaudalDTO Create(IDataReader dr)
        {
            SHCaudalDTO entity = new SHCaudalDTO();

            int iIdCaudal = dr.GetOrdinal(this.IdCaudal);
            if (!dr.IsDBNull(iIdCaudal)) entity.IdCaudal = Convert.ToInt32(dr.GetValue(iIdCaudal));

            int iEmpresaCodi = dr.GetOrdinal(this.EmpresaCodi);
            if (!dr.IsDBNull(iEmpresaCodi)) entity.EmpresaCodi = Convert.ToInt32(iEmpresaCodi);

            int iTipoSerieCodi = dr.GetOrdinal(this.TipoSerieCodi);
            if (!dr.IsDBNull(iTipoSerieCodi)) entity.TipoSerieCodi = Convert.ToInt32(iTipoSerieCodi);

            int iTPtoMediCodi = dr.GetOrdinal(this.TPtoMediCodi);
            if (!dr.IsDBNull(iTPtoMediCodi)) entity.TPtoMediCodi = Convert.ToInt32(iTPtoMediCodi);

            int iPtoMediCodi = dr.GetOrdinal(this.PtoMediCodi);
            if (!dr.IsDBNull(iPtoMediCodi)) entity.PtoMediCodi = Convert.ToInt32(iPtoMediCodi);

            int iFechaRegistro = dr.GetOrdinal(this.FechaRegistro);
            if (!dr.IsDBNull(iFechaRegistro)) entity.FechaRegistro = dr.GetDateTime(iFechaRegistro);

            int iUsuarioRegistro = dr.GetOrdinal(this.UsuarioRegistro);
            if (!dr.IsDBNull(iUsuarioRegistro)) entity.UsuarioRegistro = dr.GetString(iUsuarioRegistro);

            return entity;
        }

        public SHCaudalDetalleDTO CreateDetalle(IDataReader dr) 
        {
            SHCaudalDetalleDTO entity = new SHCaudalDetalleDTO();

            int iIdDet = dr.GetOrdinal(this.IdDet);
            if (!dr.IsDBNull(iIdDet)) entity.IdDet = Convert.ToInt32(dr.GetValue(iIdDet));

            int iIdCaudal = dr.GetOrdinal(this.IdCaudal);
            if (!dr.IsDBNull(iIdCaudal)) entity.IdCaudal = Convert.ToInt32(dr.GetValue(iIdCaudal));

            int iAnio = dr.GetOrdinal(this.Anio);
            if (!dr.IsDBNull(iAnio)) entity.Anio = Convert.ToInt32(dr.GetValue(iAnio));

            int iM1 = dr.GetOrdinal(this.M1);
            if (!dr.IsDBNull(iM1)) entity.M1 = Convert.ToDecimal(dr.GetValue(iM1));

            int iM2 = dr.GetOrdinal(this.M2);
            if (!dr.IsDBNull(iM2)) entity.M2 = Convert.ToDecimal(dr.GetValue(iM2));

            int iM3 = dr.GetOrdinal(this.M3);
            if (!dr.IsDBNull(iM3)) entity.M3 = Convert.ToDecimal(dr.GetValue(iM3));

            int iM4 = dr.GetOrdinal(this.M4);
            if (!dr.IsDBNull(iM4)) entity.M4 = Convert.ToDecimal(dr.GetValue(iM4));

            int iM5 = dr.GetOrdinal(this.M5);
            if (!dr.IsDBNull(iM5)) entity.M5 = Convert.ToDecimal(dr.GetValue(iM5));

            int iM6 = dr.GetOrdinal(this.M6);
            if (!dr.IsDBNull(iM6)) entity.M6 = Convert.ToDecimal(dr.GetValue(iM6));

            int iM7 = dr.GetOrdinal(this.M7);
            if (!dr.IsDBNull(iM7)) entity.M7 = Convert.ToDecimal(dr.GetValue(iM7));

            int iM8 = dr.GetOrdinal(this.M8);
            if (!dr.IsDBNull(iM8)) entity.M8 = Convert.ToDecimal(dr.GetValue(iM8));

            int iM9 = dr.GetOrdinal(this.M9);
            if (!dr.IsDBNull(iM9)) entity.M9 = Convert.ToDecimal(dr.GetValue(iM9));

            int iM10 = dr.GetOrdinal(this.M10);
            if (!dr.IsDBNull(iM10)) entity.M10 = Convert.ToDecimal(dr.GetValue(iM10));

            int iM11 = dr.GetOrdinal(this.M11);
            if (!dr.IsDBNull(iM11)) entity.M11 = Convert.ToDecimal(dr.GetValue(iM11));

            int iM12 = dr.GetOrdinal(this.M12);
            if (!dr.IsDBNull(iM12)) entity.M12 = Convert.ToDecimal(dr.GetValue(iM12));

            int iIndM1 = dr.GetOrdinal(this.INDM1);
            if (!dr.IsDBNull(iIndM1)) entity.IndM1 = dr.GetString(iIndM1);

            int iIndM2 = dr.GetOrdinal(this.INDM2);
            if (!dr.IsDBNull(iIndM2)) entity.IndM2 = dr.GetString(iIndM2);

            int iIndM3 = dr.GetOrdinal(this.INDM3);
            if (!dr.IsDBNull(iIndM3)) entity.IndM3 = dr.GetString(iIndM3);

            int iIndM4 = dr.GetOrdinal(this.INDM4);
            if (!dr.IsDBNull(iIndM4)) entity.IndM4 = dr.GetString(iIndM4);

            int iIndM5 = dr.GetOrdinal(this.INDM5);
            if (!dr.IsDBNull(iIndM5)) entity.IndM5 = dr.GetString(iIndM5);

            int iIndM6 = dr.GetOrdinal(this.INDM6);
            if (!dr.IsDBNull(iIndM6)) entity.IndM6 = dr.GetString(iIndM6);

            int iIndM7 = dr.GetOrdinal(this.INDM7);
            if (!dr.IsDBNull(iIndM7)) entity.IndM7 = dr.GetString(iIndM7);

            int iIndM8 = dr.GetOrdinal(this.INDM8);
            if (!dr.IsDBNull(iIndM8)) entity.IndM8 = dr.GetString(iIndM8);

            int iIndM9 = dr.GetOrdinal(this.INDM9);
            if (!dr.IsDBNull(iIndM9)) entity.IndM9 = dr.GetString(iIndM9);

            int iIndM10 = dr.GetOrdinal(this.INDM10);
            if (!dr.IsDBNull(iIndM10)) entity.IndM10 = dr.GetString(iIndM10);

            int iIndM11 = dr.GetOrdinal(this.INDM11);
            if (!dr.IsDBNull(iIndM11)) entity.IndM11 = dr.GetString(iIndM11);

            int iIndM12 = dr.GetOrdinal(this.INDM12);
            if (!dr.IsDBNull(iIndM12)) entity.IndM12 = dr.GetString(iIndM12);

            return entity;
        }


        #region Mapeo de Campos SH_CAUDAL

        public string IdCaudal = "IDCAUDAL";
        public string EmpresaCodi = "EMPRESACODI";
        public string TipoSerieCodi = "TIPOSERIECODI";
        public string TPtoMediCodi = "TPTOMEDICODI";
        public string PtoMediCodi = "PTOMEDICODI";
        public string FechaRegistro = "FECHAREGISTRO";
        public string UsuarioRegistro = "USUARIOREGISTRO";

        public string IdDet = "IDDET";
        public string Anio = "ANIO";
        public string M1 = "M1";
        public string M2 = "M2";
        public string M3 = "M3";
        public string M4 = "M4";
        public string M5 = "M5";
        public string M6 = "M6";
        public string M7 = "M7";
        public string M8 = "M8";
        public string M9 = "M9";
        public string M10 = "M10";
        public string M11 = "M11";
        public string M12 = "M12";
        public string INDM1 = "INDM1";
        public string INDM2 = "INDM2";
        public string INDM3 = "INDM3";
        public string INDM4 = "INDM4";
        public string INDM5 = "INDM5";
        public string INDM6 = "INDM6";
        public string INDM7 = "INDM7";
        public string INDM8 = "INDM8";
        public string INDM9 = "INDM9";
        public string INDM10 = "INDM10";
        public string INDM11 = "INDM11";
        public string INDM12 = "INDM12";

        #endregion

        public string SqlSaveDetalle
        {
            get { return base.GetSqlXml("SaveDetalle"); }
        }

        public string SqlGetMaxIdDetalle
        {
            get { return base.GetSqlXml("GetMaxIdDetalle"); }
        }

        public string SqlGetCaudalActivo
        {
            get { return base.GetSqlXml("GetCaudalActivo"); }
        }

        public string SqlGetCaudalDetalle
        {
            get { return base.GetSqlXml("GetCaudalDetalle"); }
        }



        /*public string SqlGetByCriteriaRango
        {
            get { return base.GetSqlXml("GetByCriteriaRango"); }
        }

        public string SqlGetListaMultiple
        {
            get { return base.GetSqlXml("GetListaMultiple"); }
        }

        public string SqlUpdate1
        {
            get { return base.GetSqlXml("Update1"); }
        }

        public string SqlGetListaMultipleXLS
        {
            get { return base.GetSqlXml("GetListaMultipleXLS"); }
        }

        public string SqlTotalListaMultiple
        {
            get { return base.GetSqlXml("TotalListaMultiple"); }
        }

        public string SqlObtenerReporteEnvioCumplimiento
        {
            get { return base.GetSqlXml("ObtenerReporteEnvioCumplimiento"); }
        }

        public string SqlGetByMaxEnvioFormato
        {
            get { return base.GetSqlXml("GetByMaxEnvioFormato"); }
        }

        public string SqlGetByCriteriaRangoFecha
        {
            get { return base.GetSqlXml("GetByCriteriaRangoFecha"); }
        }

        public string SqlObtenerReporteCumplimiento
        {
            get { return base.GetSqlXml("ObtenerReporteCumplimiento"); }
        }

        public string SqlObtenerReporteCumplimientoXBloqueHorario
        {
            get { return base.GetSqlXml("ObtenerReporteCumplimientoXBloqueHorario"); }
        }

        public string SqlGetByMaxEnvioFormatoPeriodo
        {
            get { return base.GetSqlXml("GetByMaxEnvioFormatoPeriodo"); }
        }
       
        public string SqlObtenerListaEnvioActual
        {
            get { return base.GetSqlXml("ObtenerListaEnvioActual"); }
        }

        public string SqlObtenerListaPeriodoReporte
        {
            get { return base.GetSqlXml("ObtenerListaPeriodoReporte"); }
        }       

        public string SqlListReporteCumplimiento
        {
            get { return base.GetSqlXml("ListReporteCumplimiento"); }
        }

        public string SqlObtenerEnvioXModulo
        {
            get { return base.GetSqlXml("ObtenerEnvioXModulo"); }
        }

        public string SqlListaReporteCumplimientoHidrologia
        {
            get { return base.GetSqlXml("ListaReporteCumplimientoDeExtranetHidrologia"); }
        }*/

        
    }
}
