using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;
using COES.Dominio.DTO.Sp7;

namespace COES.Infraestructura.Datos.Helper.Sp7
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_REPORTEVERSION_SP7
    /// </summary>
    public class TrReporteversionSp7Helper : HelperBase
    {
        public TrReporteversionSp7Helper(): base(Consultas.TrReporteversionSp7Sql)
        {
        }

        public TrReporteversionSp7DTO Create(IDataReader dr)
        {
            TrReporteversionSp7DTO entity = new TrReporteversionSp7DTO();

            int iRevcodi = dr.GetOrdinal(this.Revcodi);
            if (!dr.IsDBNull(iRevcodi)) entity.Revcodi = Convert.ToInt32(dr.GetValue(iRevcodi));

            int iVercodi = dr.GetOrdinal(this.Vercodi);
            if (!dr.IsDBNull(iVercodi)) entity.Vercodi = Convert.ToInt32(dr.GetValue(iVercodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iRevfecha = dr.GetOrdinal(this.Revfecha);
            if (!dr.IsDBNull(iRevfecha)) entity.Revfecha = dr.GetDateTime(iRevfecha);

            int iRevsumaciccpsmed = dr.GetOrdinal(this.Revsumaciccpsmed);
            if (!dr.IsDBNull(iRevsumaciccpsmed)) entity.Revsumaciccpsmed = dr.GetDecimal(iRevsumaciccpsmed);

            int iRevsumaciccpsest = dr.GetOrdinal(this.Revsumaciccpsest);
            if (!dr.IsDBNull(iRevsumaciccpsest)) entity.Revsumaciccpsest = dr.GetDecimal(iRevsumaciccpsest);

            int iRevsumaciccpsestnoalm = dr.GetOrdinal(this.Revsumaciccpsestnoalm);
            if (!dr.IsDBNull(iRevsumaciccpsestnoalm)) entity.Revsumaciccpsestnoalm = dr.GetDecimal(iRevsumaciccpsestnoalm);

            int iRevsumaciccpsalm = dr.GetOrdinal(this.Revsumaciccpsalm);
            if (!dr.IsDBNull(iRevsumaciccpsalm)) entity.Revsumaciccpsalm = dr.GetDecimal(iRevsumaciccpsalm);

            int iRevsumaciccpsmedc = dr.GetOrdinal(this.Revsumaciccpsmedc);
            if (!dr.IsDBNull(iRevsumaciccpsmedc)) entity.Revsumaciccpsmedc = dr.GetDecimal(iRevsumaciccpsmedc);

            int iRevsumaciccpsestc = dr.GetOrdinal(this.Revsumaciccpsestc);
            if (!dr.IsDBNull(iRevsumaciccpsestc)) entity.Revsumaciccpsestc = dr.GetDecimal(iRevsumaciccpsestc);

            int iRevsumaciccpsestnoalmc = dr.GetOrdinal(this.revsumaciccpsestnoalmc);
            if (!dr.IsDBNull(iRevsumaciccpsestnoalmc)) entity.Revsumaciccpsestnoalmc = dr.GetDecimal(iRevsumaciccpsestnoalmc);

            int iRevsumaciccpsalmc = dr.GetOrdinal(this.Revsumaciccpsalmc);
            if (!dr.IsDBNull(iRevsumaciccpsalmc)) entity.Revsumaciccpsalmc = dr.GetDecimal(iRevsumaciccpsalmc);

            int iRevnummed = dr.GetOrdinal(this.Revnummed);
            if (!dr.IsDBNull(iRevnummed)) entity.Revnummed = Convert.ToInt32(dr.GetValue(iRevnummed));

            int iRevnumest = dr.GetOrdinal(this.Revnumest);
            if (!dr.IsDBNull(iRevnumest)) entity.Revnumest = Convert.ToInt32(dr.GetValue(iRevnumest));

            int iRevnumestnoalm = dr.GetOrdinal(this.Revnumestnoalm);
            if (!dr.IsDBNull(iRevnumestnoalm)) entity.Revnumestnoalm = Convert.ToInt32(dr.GetValue(iRevnumestnoalm));
            
            int iRevnumalm = dr.GetOrdinal(this.Revnumalm);
            if (!dr.IsDBNull(iRevnumalm)) entity.Revnumalm = Convert.ToInt32(dr.GetValue(iRevnumalm));

            int iRevnummedc = dr.GetOrdinal(this.Revnummedc);
            if (!dr.IsDBNull(iRevnummedc)) entity.Revnummedc = Convert.ToInt32(dr.GetValue(iRevnummedc));

            int iRevnumestc = dr.GetOrdinal(this.Revnumestc);
            if (!dr.IsDBNull(iRevnumestc)) entity.Revnumestc = Convert.ToInt32(dr.GetValue(iRevnumestc));

            int iRevnumestnoalmc = dr.GetOrdinal(this.Revnumestnoalmc);
            if (!dr.IsDBNull(iRevnumestnoalmc)) entity.Revnumestnoalmc = Convert.ToInt32(dr.GetValue(iRevnumestnoalmc));

            int iRevnumalmc = dr.GetOrdinal(this.Revnumalmc);
            if (!dr.IsDBNull(iRevnumalmc)) entity.Revnumalmc = Convert.ToInt32(dr.GetValue(iRevnumalmc));

            int iRevnummedcsindef = dr.GetOrdinal(this.Revnummedcsindef);
            if (!dr.IsDBNull(iRevnummedcsindef)) entity.Revnummedcsindef = Convert.ToInt32(dr.GetValue(iRevnummedcsindef));

            int iRevnumestcsindef = dr.GetOrdinal(this.Revnumestcsindef);
            if (!dr.IsDBNull(iRevnumestcsindef)) entity.Revnumestcsindef = Convert.ToInt32(dr.GetValue(iRevnumestcsindef));

            int iRevnumalmcsindef = dr.GetOrdinal(this.Revnumalmcsindef);
            if (!dr.IsDBNull(iRevnumalmcsindef)) entity.Revnumalmcsindef = Convert.ToInt32(dr.GetValue(iRevnumalmcsindef));

            int iRevtfse = dr.GetOrdinal(this.Revtfse);
            if (!dr.IsDBNull(iRevtfse)) entity.Revtfse = Convert.ToInt32(dr.GetValue(iRevtfse));

            int iRevtfss = dr.GetOrdinal(this.Revtfss);
            if (!dr.IsDBNull(iRevtfss)) entity.Revtfss = Convert.ToInt32(dr.GetValue(iRevtfss));

            int iRevttotal = dr.GetOrdinal(this.Revttotal);
            if (!dr.IsDBNull(iRevttotal)) entity.Revttotal = Convert.ToInt32(dr.GetValue(iRevttotal));

            int iRevfactdisp = dr.GetOrdinal(this.Revfactdisp);
            if (!dr.IsDBNull(iRevfactdisp)) entity.Revfactdisp = dr.GetDecimal(iRevfactdisp);

            int iRevciccpe = dr.GetOrdinal(this.Revciccpe);
            if (!dr.IsDBNull(iRevciccpe)) entity.Revciccpe = dr.GetDecimal(iRevciccpe);

            int iRevciccpemedest = dr.GetOrdinal(this.Revciccpemedest);
            if (!dr.IsDBNull(iRevciccpemedest)) entity.Revciccpemedest = dr.GetDecimal(iRevciccpemedest);

            int iRevciccpecrit = dr.GetOrdinal(this.Revciccpecrit);
            if (!dr.IsDBNull(iRevciccpecrit)) entity.Revciccpecrit = dr.GetDecimal(iRevciccpecrit);

            int iRevttng = dr.GetOrdinal(this.Revttng);
            if (!dr.IsDBNull(iRevttng)) entity.Revttng = Convert.ToInt32(dr.GetValue(iRevttng));

            int iRevusucreacion = dr.GetOrdinal(this.Revusucreacion);
            if (!dr.IsDBNull(iRevusucreacion)) entity.Revusucreacion = dr.GetString(iRevusucreacion);

            int iRevfeccreacion = dr.GetOrdinal(this.Revfeccreacion);
            if (!dr.IsDBNull(iRevfeccreacion)) entity.Revfeccreacion = dr.GetDateTime(iRevfeccreacion);

            int iRevusumodificacion = dr.GetOrdinal(this.Revusumodificacion);
            if (!dr.IsDBNull(iRevusumodificacion)) entity.Revusumodificacion = dr.GetString(iRevusumodificacion);

            int iRevfecmodificacion = dr.GetOrdinal(this.Revfecmodificacion);
            if (!dr.IsDBNull(iRevfecmodificacion)) entity.Revfecmodificacion = dr.GetDateTime(iRevfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Revcodi = "REVCODI";
        public string Vercodi = "VERCODI";
        public string Emprcodi = "EMPRCODI";
        public string Revfecha = "REVFECHA";
        public string Revsumaciccpsmed = "REVSUMACICCPSMED";
        public string Revsumaciccpsest = "REVSUMACICCPSEST";
        public string Revsumaciccpsestnoalm = "REVSUMACICCPSESTNOALM";
        public string Revsumaciccpsalm = "REVSUMACICCPSALM";
        public string Revsumaciccpsmedc = "REVSUMACICCPSMEDC";
        public string Revsumaciccpsestc = "REVSUMACICCPSESTC";
        public string revsumaciccpsestnoalmc = "REVSUMACICCPSESTNOALMC";
        public string Revsumaciccpsalmc = "REVSUMACICCPSALMC";
        public string Revnummed = "REVNUMMED";
        public string Revnumest = "REVNUMEST";
        public string Revnumestnoalm = "REVNUMESTNOALM";
        public string Revnumalm = "REVNUMALM";
        public string Revnummedc = "REVNUMMEDC";
        public string Revnumestc = "REVNUMESTC";
        public string Revnumestnoalmc = "REVNUMESTNOALMC";
        public string Revnumalmc = "REVNUMALMC";
        public string Revnummedcsindef = "REVNUMMEDCSINDEF";
        public string Revnumestcsindef = "REVNUMESTCSINDEF";
        public string Revnumalmcsindef = "REVNUMALMCSINDEF";
        public string Revtfse = "REVTFSE";
        public string Revtfss = "REVTFSS";
        public string Revttotal = "REVTTOTAL";
        public string Revfactdisp = "REVFACTDISP";
        public string Revciccpe = "REVCICCPE";
        public string Revciccpemedest = "REVCICCPEMEDEST";
        public string Revciccpecrit = "REVCICCPECRIT";
        public string Revttng = "REVTTNG";
        public string Revusucreacion = "REVUSUCREACION";
        public string Revfeccreacion = "REVFECCREACION";
        public string Revusumodificacion = "REVUSUMODIFICACION";
        public string Revfecmodificacion = "REVFECMODIFICACION";

        public string Verfechaini = "VERFECHAINI";
        public string Verfechafin = "VERFECHAFIN";
        public string Emprenomb = "EMPRENOMB";
        public string CanGeneral = "CANGENERAL";
        public string CanMedEst = "CANMEDEST";
        public string CanCritico = "CANCRITICO";
        public string VerNumero = "VERNUMERO";


        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string ObtenerListadoResumen
        {
            get { return base.GetSqlXml("ObtenerListadoResumen"); }
        }


        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlDeleteVersion
        {
            get { return base.GetSqlXml("DeleteVersion"); }
        }

        public string SqlListVersion
        {
            get { return base.GetSqlXml("ListVersion"); }
        }

        public string SqlListAgrupada
        {
            get { return base.GetSqlXml("ListAgrupada"); }
        }

        public string SqlGetMinId
        {
            get { return base.GetSqlXml("GetMinId"); }
        }

        public string SqlGetDispMensualVersion
        {
            get { return base.GetSqlXml("GetDispMensualVersion"); }
        }


        #region FIT Señales no disponibles

        public string SqlGetEmpresasDiasVersion
        {
            get { return base.GetSqlXml("GetEmpresasDiasVersion"); }
        }

        public string SqlObtenerCongelamientoSenales
        {
            get { return base.GetSqlXml("ObtenerCongelamientoSenales"); }
        }

        public string SqlObtenerCaidaEnlace
        {
            get { return base.GetSqlXml("ObtenerCaidaEnlace"); }
        }

        public string SqlObtenerIndicadorCaidaEnlaceHora
        {
            get { return base.GetSqlXml("ObtenerIndicadorCaidaEnlaceHora"); }
        }

        public string SqlObtenerIndicadorCaidaEnlace
        {
            get { return base.GetSqlXml("ObtenerIndicadorCaidaEnlace"); }
        }


        #endregion


        #endregion
    }
}
