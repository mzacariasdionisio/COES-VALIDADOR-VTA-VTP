using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_GERCSV
    /// </summary>
    public class RerGerCsvHelper : HelperBase
    {
        public RerGerCsvHelper() : base(Consultas.RerGerCsvSql)
        {
        }

        public RerGerCsvDTO Create(IDataReader dr)
        {
            RerGerCsvDTO entity = new RerGerCsvDTO();

            int iRegercodi = dr.GetOrdinal(this.Regercodi);
            if (!dr.IsDBNull(iRegercodi)) entity.Regercodi = Convert.ToInt32(dr.GetValue(iRegercodi));

            int iResddpcodi = dr.GetOrdinal(this.Resddpcodi);
            if (!dr.IsDBNull(iResddpcodi)) entity.Resddpcodi = Convert.ToInt32(dr.GetValue(iResddpcodi));

            int iRegergndarchivo = dr.GetOrdinal(this.Regergndarchivo);
            if (!dr.IsDBNull(iRegergndarchivo)) entity.Regergndarchivo = dr.GetString(iRegergndarchivo);

            int iRegerhidarchivo = dr.GetOrdinal(this.Regerhidarchivo);
            if (!dr.IsDBNull(iRegerhidarchivo)) entity.Regerhidarchivo = dr.GetString(iRegerhidarchivo);

            int iRegerusucreacion = dr.GetOrdinal(this.Regerusucreacion);
            if (!dr.IsDBNull(iRegerusucreacion)) entity.Regerusucreacion = dr.GetString(iRegerusucreacion);

            int iRegerfeccreacion = dr.GetOrdinal(this.Regerfeccreacion);
            if (!dr.IsDBNull(iRegerfeccreacion)) entity.Regerfeccreacion = dr.GetDateTime(iRegerfeccreacion);

            return entity;
        }

        #region Mapeo de Campos RER_GERCSV
        public string Regercodi = "REGERCODI";
        public string Resddpcodi = "RESDDPCODI";
        public string Regergndarchivo = "REGERGNDARCHIVO";
        public string Regerhidarchivo = "REGERHIDARCHIVO";
        public string Regerusucreacion = "REGERUSUCREACION";
        public string Regerfeccreacion = "REGERFECCREACION";
        #endregion

        #region Mapeo de Campos RER_LECCSV_TEMP
        public string Rerfecinicio = "RERFECINICIO";
        public string Reretapa = "RERETAPA";
        public string Rerserie = "RERSERIE";
        public string Rerbloque = "RERBLOQUE";
        public string Rercentrsddp = "RERCENTRSDDP";
        public string Rervalor = "RERVALOR";
        public string Rertipcsv = "RERTIPCSV";
        #endregion

        #region Mapeo de Campos RER_CENTRAL
        public string Rercencodi = "RERCENCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        #endregion

        #region Mapeo de Campos RER_CENTRAL_PMPO
        public string Ptomedicodi = "PTOMEDICODI";
        public string Ptomedidesc = "PTOMEDIDESC";
        #endregion

        #region Mapeo de Campos PMO_SDDP_CODIGO
        public string Sddpnomb = "SDDPNOMB";
        #endregion

        #region Mapeo de Campos RER_INSUMO_DIA_TEMP
        public string Rerinddiafecdia = "RERINDDIAFECDIA";
        public string Rerinddiah1 = "RERINDDIAH1";
        public string Rerinddiah2 = "RERINDDIAH2";
        public string Rerinddiah3 = "RERINDDIAH3";
        public string Rerinddiah4 = "RERINDDIAH4";
        public string Rerinddiah5 = "RERINDDIAH5";
        public string Rerinddiah6 = "RERINDDIAH6";
        public string Rerinddiah7 = "RERINDDIAH7";
        public string Rerinddiah8 = "RERINDDIAH8";
        public string Rerinddiah9 = "RERINDDIAH9";
        public string Rerinddiah10 = "RERINDDIAH10";
        public string Rerinddiah11 = "RERINDDIAH11";
        public string Rerinddiah12 = "RERINDDIAH12";
        public string Rerinddiah13 = "RERINDDIAH13";
        public string Rerinddiah14 = "RERINDDIAH14";
        public string Rerinddiah15 = "RERINDDIAH15";
        public string Rerinddiah16 = "RERINDDIAH16";
        public string Rerinddiah17 = "RERINDDIAH17";
        public string Rerinddiah18 = "RERINDDIAH18";
        public string Rerinddiah19 = "RERINDDIAH19";
        public string Rerinddiah20 = "RERINDDIAH20";
        public string Rerinddiah21 = "RERINDDIAH21";
        public string Rerinddiah22 = "RERINDDIAH22";
        public string Rerinddiah23 = "RERINDDIAH23";
        public string Rerinddiah24 = "RERINDDIAH24";
        public string Rerinddiah25 = "RERINDDIAH25";
        public string Rerinddiah26 = "RERINDDIAH26";
        public string Rerinddiah27 = "RERINDDIAH27";
        public string Rerinddiah28 = "RERINDDIAH28";
        public string Rerinddiah29 = "RERINDDIAH29";
        public string Rerinddiah30 = "RERINDDIAH30";
        public string Rerinddiah31 = "RERINDDIAH31";
        public string Rerinddiah32 = "RERINDDIAH32";
        public string Rerinddiah33 = "RERINDDIAH33";
        public string Rerinddiah34 = "RERINDDIAH34";
        public string Rerinddiah35 = "RERINDDIAH35";
        public string Rerinddiah36 = "RERINDDIAH36";
        public string Rerinddiah37 = "RERINDDIAH37";
        public string Rerinddiah38 = "RERINDDIAH38";
        public string Rerinddiah39 = "RERINDDIAH39";
        public string Rerinddiah40 = "RERINDDIAH40";
        public string Rerinddiah41 = "RERINDDIAH41";
        public string Rerinddiah42 = "RERINDDIAH42";
        public string Rerinddiah43 = "RERINDDIAH43";
        public string Rerinddiah44 = "RERINDDIAH44";
        public string Rerinddiah45 = "RERINDDIAH45";
        public string Rerinddiah46 = "RERINDDIAH46";
        public string Rerinddiah47 = "RERINDDIAH47";
        public string Rerinddiah48 = "RERINDDIAH48";
        public string Rerinddiah49 = "RERINDDIAH49";
        public string Rerinddiah50 = "RERINDDIAH50";
        public string Rerinddiah51 = "RERINDDIAH51";
        public string Rerinddiah52 = "RERINDDIAH52";
        public string Rerinddiah53 = "RERINDDIAH53";
        public string Rerinddiah54 = "RERINDDIAH54";
        public string Rerinddiah55 = "RERINDDIAH55";
        public string Rerinddiah56 = "RERINDDIAH56";
        public string Rerinddiah57 = "RERINDDIAH57";
        public string Rerinddiah58 = "RERINDDIAH58";
        public string Rerinddiah59 = "RERINDDIAH59";
        public string Rerinddiah60 = "RERINDDIAH60";
        public string Rerinddiah61 = "RERINDDIAH61";
        public string Rerinddiah62 = "RERINDDIAH62";
        public string Rerinddiah63 = "RERINDDIAH63";
        public string Rerinddiah64 = "RERINDDIAH64";
        public string Rerinddiah65 = "RERINDDIAH65";
        public string Rerinddiah66 = "RERINDDIAH66";
        public string Rerinddiah67 = "RERINDDIAH67";
        public string Rerinddiah68 = "RERINDDIAH68";
        public string Rerinddiah69 = "RERINDDIAH69";
        public string Rerinddiah70 = "RERINDDIAH70";
        public string Rerinddiah71 = "RERINDDIAH71";
        public string Rerinddiah72 = "RERINDDIAH72";
        public string Rerinddiah73 = "RERINDDIAH73";
        public string Rerinddiah74 = "RERINDDIAH74";
        public string Rerinddiah75 = "RERINDDIAH75";
        public string Rerinddiah76 = "RERINDDIAH76";
        public string Rerinddiah77 = "RERINDDIAH77";
        public string Rerinddiah78 = "RERINDDIAH78";
        public string Rerinddiah79 = "RERINDDIAH79";
        public string Rerinddiah80 = "RERINDDIAH80";
        public string Rerinddiah81 = "RERINDDIAH81";
        public string Rerinddiah82 = "RERINDDIAH82";
        public string Rerinddiah83 = "RERINDDIAH83";
        public string Rerinddiah84 = "RERINDDIAH84";
        public string Rerinddiah85 = "RERINDDIAH85";
        public string Rerinddiah86 = "RERINDDIAH86";
        public string Rerinddiah87 = "RERINDDIAH87";
        public string Rerinddiah88 = "RERINDDIAH88";
        public string Rerinddiah89 = "RERINDDIAH89";
        public string Rerinddiah90 = "RERINDDIAH90";
        public string Rerinddiah91 = "RERINDDIAH91";
        public string Rerinddiah92 = "RERINDDIAH92";
        public string Rerinddiah93 = "RERINDDIAH93";
        public string Rerinddiah94 = "RERINDDIAH94";
        public string Rerinddiah95 = "RERINDDIAH95";
        public string Rerinddiah96 = "RERINDDIAH96";
        public string Rerinddiatotal = "RERINDDIATOTAL";

        #endregion

        #region Querys
        public string SqlTruncateTablaTemporal
        {
            get { return base.GetSqlXml("TruncateTablaTemporal"); }
        }

        public string SqlInsertTablaTemporal
        {
            get { return base.GetSqlXml("InsertTablaTemporal"); }
        }

        public string SqlListTablaTemporal
        {
            get { return base.GetSqlXml("ListTablaTemporal"); }
        }

        public string SqlListEquiposEmpresasCentralesRer
        {
            get { return base.GetSqlXml("ListEquiposEmpresasCentralesRer"); }
        }

        public string SqlListPtosMedicionCentralesPmpo
        {
            get { return base.GetSqlXml("ListPtosMedicionCentralesPmpo"); }
        }

        public string SqlGetByCentralesSddp
        {
            get { return base.GetSqlXml("GetByCentralesSddp"); }
        }

        public string SqlListPtoMedicionCentralesRer
        {
            get { return base.GetSqlXml("ListPtoMedicionCentralesRer"); }
        }

        public string SqlListTablaCMTemporal
        {
            get { return base.GetSqlXml("ListTablaCMTemporal"); }
        }

        public string SqlListTablaCMTemporalDia
        {
            get { return base.GetSqlXml("ListTablaCMTemporalDia"); }
        }
        #endregion
    }
}

