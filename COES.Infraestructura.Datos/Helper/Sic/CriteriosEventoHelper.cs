using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class CriteriosEventoHelper : HelperBase
    {
        public CriteriosEventoHelper()
            : base(Consultas.CriteriosEventoSql)
        {

        }
        public string SqlObtenerComentarioXEventoyEtapa
        {
            get { return base.GetSqlXml("SqlObtenerComentarioXEventoyEtapa"); }
        }

        public string SqlConsultarCriterioEvento
        {
            get { return base.GetSqlXml("SqlConsultarCriterioEvento"); }
        }
        public string SqlConsultarCriterioEvento2
        {
            get { return base.GetSqlXml("SqlConsultarCriterioEvento2"); }
        }
        public string SqlTraerEtapaxEvento
        {
            get { return base.GetSqlXml("SqlTraerEtapaxEvento"); }
        }
        public string SqlObtenerEmpresaResponsable
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaResponsable"); }
        }
        public string SqlObtenerEmpresaSolicitante
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaSolicitante"); }
        }
        public string SqlObtenerCasosEspeciales
        {
            get { return base.GetSqlXml("SqlObtenerCasosEspeciales"); }
        }

        public string SqlObtenerCriterios
        {
            get { return base.GetSqlXml("SqlObtenerCriterios"); }
        }

        public string SqlSaveCasosEspeciales
        {
            get { return base.GetSqlXml("SqlSaveCasosEspeciales"); }
        }

        public string SqlUpdateCasosEspeciales
        {
            get { return base.GetSqlXml("SqlUpdateCasosEspeciales"); }
        }
        public string SqlDeleteCasosEspeciales
        {
            get { return base.GetSqlXml("SqlDeleteCasosEspeciales"); }
        }
        public string SqlGetByIdCasosEspeciales
        {
            get { return base.GetSqlXml("SqlGetByIdCasosEspeciales"); }
        }
        public string SqlListCasosEspeciales
        {
            get { return base.GetSqlXml("SqlListCasosEspeciales"); }
        }

        public string SqlValidarCasosEspeciales
        {
            get { return base.GetSqlXml("ValidarCasosEspeciales"); }
        }

        public string SqlSaveCriterios
        {
            get { return base.GetSqlXml("SqlSaveCriterios"); }
        }
        public string SqlValidarCriterios
        {
            get { return base.GetSqlXml("ValidarCriterios"); }
        }
        public string SqlGetMaxIdCasosEspeciales
        {
            get { return base.GetSqlXml("SqlGetMaxIdCasosEspeciales"); }
        }
        public string SqlGetMaxIdCriterios
        {
            get { return base.GetSqlXml("SqlGetMaxIdCriterios"); }
        }
        public string SqlUpdateCriterios
        {
            get { return base.GetSqlXml("SqlUpdateCriterios"); }
        }
        
        public string SqlGetByIdCriterios
        {
            get { return base.GetSqlXml("SqlGetByIdCriterios"); }
        }
        public string SqlDeleteCriterios
        {
            get { return base.GetSqlXml("SqlDeleteCriterios"); }
        }
        public string SqlListCriterios
        {
            get { return base.GetSqlXml("SqlListCriterios"); }
        }
        
        
        public CrCasosEspecialesDTO CreateCasosEspeciales(IDataReader dr)
        {
            CrCasosEspecialesDTO entity = new CrCasosEspecialesDTO();

            int iCRESPECIALCODI = dr.GetOrdinal(this.CRESPECIALCODI);
            if (!dr.IsDBNull(iCRESPECIALCODI)) entity.CRESPECIALCODI = Convert.ToInt32(dr.GetValue(iCRESPECIALCODI));

            int iCREDESCRIPCION = dr.GetOrdinal(this.CREDESCRIPCION);
            if (!dr.IsDBNull(iCREDESCRIPCION)) entity.CREDESCRIPCION = dr.GetString(iCREDESCRIPCION);

            int iCREESTADO = dr.GetOrdinal(this.CREESTADO);
            if (!dr.IsDBNull(iCREESTADO)) entity.CREESTADO = dr.GetString(iCREESTADO);

            int iLASTDATE = dr.GetOrdinal(this.LASTDATE);
            if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);

            int iLASTUSER = dr.GetOrdinal(this.LASTUSER);
            if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);

            return entity;
        }

        public CrCriteriosDTO CreateCriterios(IDataReader dr)
        {
            CrCriteriosDTO entity = new CrCriteriosDTO();

            int iCRCRITERIOCODI = dr.GetOrdinal(this.CRCRITERIOCODI);
            if (!dr.IsDBNull(iCRCRITERIOCODI)) entity.CRCRITERIOCODI = Convert.ToInt32(dr.GetValue(iCRCRITERIOCODI));

            int iCREDESCRIPCION = dr.GetOrdinal(this.CREDESCRIPCIONC);
            if (!dr.IsDBNull(iCREDESCRIPCION)) entity.CREDESCRIPCION = dr.GetString(iCREDESCRIPCION);

            int iCREESTADO = dr.GetOrdinal(this.CREESTADOC);
            if (!dr.IsDBNull(iCREESTADO)) entity.CREESTADO = dr.GetString(iCREESTADO);

            int iLASTDATE = dr.GetOrdinal(this.LASTDATEC);
            if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);

            int iLASTUSER = dr.GetOrdinal(this.LASTUSERC);
            if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);

            return entity;
        }


        #region Mapeo de Campos Casos Especiales
        public string CRESPECIALCODI = "CRESPECIALCODI";
        public string CREDESCRIPCION = "CREDESCRIPCION";
        public string CREESTADO = "CREESTADO";
        public string LASTDATE = "LASTDATE";
        public string LASTUSER = "LASTUSER";
        #endregion

        #region Mapeo de Campos Casos Especiales
        public string CRCRITERIOCODI = "CRCRITERIOCODI";
        public string CREDESCRIPCIONC = "CREDESCRIPCION";
        public string CREESTADOC = "CREESTADO";
        public string LASTDATEC = "LASTDATE";
        public string LASTUSERC = "LASTUSER";
        #endregion
    }
}
