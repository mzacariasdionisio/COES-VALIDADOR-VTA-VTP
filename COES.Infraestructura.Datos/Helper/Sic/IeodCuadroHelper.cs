using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class IeodCuadroHelper : HelperBase
    {
        public IeodCuadroHelper()
            : base(Consultas.IeodCuadroSql)
        {

        }

        public IeodCuadroDTO Create(IDataReader dr)
        {
            IeodCuadroDTO entity = new IeodCuadroDTO();

            int iICCODI = dr.GetOrdinal(this.ICCODI);
            int iEQUICODI = dr.GetOrdinal(this.EQUICODI);
            int iSUBCAUSACODI = dr.GetOrdinal(this.SUBCAUSACODI);
            int iICHORINI = dr.GetOrdinal(this.ICHORINI);
            int iICHORFIN = dr.GetOrdinal(this.ICHORFIN);
            int iICDESCRIP1 = dr.GetOrdinal(this.ICDESCRIP1);
            int iICDESCRIP2 = dr.GetOrdinal(this.ICDESCRIP2);
            int iICDESCRIP3 = dr.GetOrdinal(this.ICDESCRIP3);
            int iICCHECK1 = dr.GetOrdinal(this.ICCHECK1);
            int iICVALOR1 = dr.GetOrdinal(this.ICVALOR1);
            int iLASTUSER = dr.GetOrdinal(this.LASTUSER);
            int iLASTDATE = dr.GetOrdinal(this.LASTDATE);
            int iNUMTRSGSUBIT = dr.GetOrdinal(this.NUMTRSGSUBIT);
            int iNUMTRSGSOSTN = dr.GetOrdinal(this.NUMTRSGSOSTN);
            int iICCHECK2 = dr.GetOrdinal(this.ICCHECK2);
            int iEVENCLASECODI = dr.GetOrdinal(this.EVENCLASECODI);
            int iICHOR3 = dr.GetOrdinal(this.ICHOR3);


            if (!dr.IsDBNull(iICCODI)) entity.ICCODI = Convert.ToInt32(dr.GetValue(iICCODI));
            if (!dr.IsDBNull(iEQUICODI)) entity.EQUICODI = Convert.ToInt32(dr.GetValue(iEQUICODI));
            if (!dr.IsDBNull(iSUBCAUSACODI)) entity.SUBCAUSACODI = Convert.ToInt32(dr.GetValue(iSUBCAUSACODI));
            if (!dr.IsDBNull(iICHORINI)) entity.ICHORINI = dr.GetDateTime(iICHORINI);
            if (!dr.IsDBNull(iICHORFIN)) entity.ICHORFIN = dr.GetDateTime(iICHORFIN);
            if (!dr.IsDBNull(iICDESCRIP1)) entity.ICDESCRIP1 = dr.GetString(iICDESCRIP1);
            if (!dr.IsDBNull(iICDESCRIP2)) entity.ICDESCRIP2 = dr.GetString(iICDESCRIP2);
            if (!dr.IsDBNull(iICDESCRIP3)) entity.ICDESCRIP3 = dr.GetString(iICDESCRIP3);
            if (!dr.IsDBNull(iICCHECK1)) entity.ICCHECK1 = dr.GetString(iICCHECK1);
            if (!dr.IsDBNull(iICVALOR1)) entity.ICVALOR1 = dr.GetDecimal(iICVALOR1);
            if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
            if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
            if (!dr.IsDBNull(iNUMTRSGSUBIT)) entity.NUMTRSGSUBIT = Convert.ToInt32(dr.GetValue(iNUMTRSGSUBIT));
            if (!dr.IsDBNull(iNUMTRSGSOSTN)) entity.NUMTRSGSOSTN = Convert.ToInt32(dr.GetValue(iNUMTRSGSOSTN));
            if (!dr.IsDBNull(iICCHECK2)) entity.ICCHECK2 = dr.GetString(iICCHECK2);
            if (!dr.IsDBNull(iEVENCLASECODI)) entity.EVENCLASECODI = Convert.ToInt32(dr.GetValue(iEVENCLASECODI));
            if (!dr.IsDBNull(iICHOR3)) entity.ICHOR3 = dr.GetDateTime(iICHOR3);


            return entity;
        }


        public string ICCODI = "ICCODI";
        public string EQUICODI = "EQUICODI";
        public string SUBCAUSACODI = "SUBCAUSACODI";
        public string SUBCAUSADESC = "SUBCAUSADESC";
        public string ICHORINI = "ICHORINI";
        public string ICHORFIN = "ICHORFIN";
        public string ICDESCRIP1 = "ICDESCRIP1";
        public string ICDESCRIP2 = "ICDESCRIP2";
        public string ICDESCRIP3 = "ICDESCRIP3";
        public string ICCHECK1 = "ICCHECK1";
        public string ICVALOR1 = "ICVALOR1";
        public string LASTUSER = "LASTUSER";
        public string LASTDATE = "LASTDATE";
        public string NUMTRSGSUBIT = "NUMTRSGSUBIT";
        public string NUMTRSGSOSTN = "NUMTRSGSOSTN";
        public string ICCHECK2 = "ICCHECK2";
        public string EVENCLASECODI = "EVENCLASECODI";
        public string ICHOR3 = "ICHOR3";
        public string FAMABREV = "FAMABREV";
        public string TAREAABREV = "TAREAABREV";
        public string EMPRNOMB = "EMPRNOMB";
        public string AREANOMB = "AREANOMB";
        public string EQUIABREV = "EQUIABREV";
        public string EQUINOMB = "EQUINOMB";
        public string EMPRCODI = "EMPRCODI";
        public string MAXCOUNT = "MAXCOUNT";
        public string RUS = "RUS";
        public string HORA = "HORA";
        public string Icnombarchenvio = "ICNOMBARCHENVIO";
        public string Icnombarchfisico = "ICNOMBARCHFISICO";
        public string Icestado = "ICESTADO";

        public string SqlGetMaxId
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlUpdateCounter
        {
            get { return base.GetSqlXml("UpdateCounter"); }
        }

        public string SqlConfiguracionEquipo
        {
            get { return base.GetSqlXml("ConfiguracionEquipo"); }
        }

        public string SqlObtenerReporte
        {
            get { return base.GetSqlXml("SqlObtenerReporte"); }
        }

        public string SqlValidarExistencia
        {
            get { return base.GetSqlXml("ValidarExistencia"); }
        }

        public string SqlGetFallaAcumuladaSein
        {
            get { return base.GetSqlXml("GetFallaAcumuladaSein"); }
        }

        public string SqlListarIeodCuadroxEmpresa
        {
            get { return base.GetSqlXml("ListarIeodCuadroxEmpresa"); }
        }

        public string SqlDelete2
        {
            get { return base.GetSqlXml("DeleteById"); }
        }

        public string SqlGetCriteriaxPKCodis
        {
            get { return base.GetSqlXml("GetCriteriaxPKCodis"); }
        }

        public string SqlBorradoLogico
        {
            get { return base.GetSqlXml("BorradoLogico"); }
        }

    }

}
