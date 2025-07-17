using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_LOGDMP_SP7
    /// </summary>
    public class TrLogdmpSp7Helper : HelperBase
    {
        public TrLogdmpSp7Helper()
            : base(Consultas.TrLogdmpSp7Sql)
        {
        }

        public TrLogdmpSp7DTO Create(IDataReader dr)
        {
            TrLogdmpSp7DTO entity = new TrLogdmpSp7DTO();

            int iLdmcodi = dr.GetOrdinal(this.Ldmcodi);
            if (!dr.IsDBNull(iLdmcodi)) entity.Ldmcodi = Convert.ToInt32(dr.GetValue(iLdmcodi));

            int iLdmfecha = dr.GetOrdinal(this.Ldmfecha);
            if (!dr.IsDBNull(iLdmfecha)) entity.Ldmfecha = dr.GetDateTime(iLdmfecha);

            int iVercodi = dr.GetOrdinal(this.Vercodi);
            if (!dr.IsDBNull(iVercodi)) entity.Vercodi = Convert.ToInt32(dr.GetValue(iVercodi));

            int iLdmfechapub = dr.GetOrdinal(this.Ldmfechapub);
            if (!dr.IsDBNull(iLdmfechapub)) entity.Ldmfechapub = dr.GetDateTime(iLdmfechapub);

            int iLdmfechaimp = dr.GetOrdinal(this.Ldmfechaimp);
            if (!dr.IsDBNull(iLdmfechaimp)) entity.Ldmfechaimp = dr.GetDateTime(iLdmfechaimp);

            int iLdmnomb = dr.GetOrdinal(this.Ldmnomb);
            if (!dr.IsDBNull(iLdmnomb)) entity.Ldmnomb = dr.GetString(iLdmnomb);

            int iLdmtipo = dr.GetOrdinal(this.Ldmtipo);
            if (!dr.IsDBNull(iLdmtipo)) entity.Ldmtipo = dr.GetString(iLdmtipo);

            int iLdmestadoserv = dr.GetOrdinal(this.Ldmestadoserv);
            if (!dr.IsDBNull(iLdmestadoserv)) entity.Ldmestadoserv = dr.GetString(iLdmestadoserv);

            int iLdmestadocli = dr.GetOrdinal(this.Ldmestadocli);
            if (!dr.IsDBNull(iLdmestadocli)) entity.Ldmestadocli = dr.GetString(iLdmestadocli);

            int iLdmnotaexp = dr.GetOrdinal(this.Ldmnotaexp);
            if (!dr.IsDBNull(iLdmnotaexp)) entity.Ldmnotaexp = dr.GetString(iLdmnotaexp);

            int iLdmnotaimp = dr.GetOrdinal(this.Ldmnotaimp);
            if (!dr.IsDBNull(iLdmnotaimp)) entity.Ldmnotaimp = dr.GetString(iLdmnotaimp);

            int iLdmmedioimp = dr.GetOrdinal(this.Ldmmedioimp);
            if (!dr.IsDBNull(iLdmmedioimp)) entity.Ldmmedioimp = dr.GetString(iLdmmedioimp);

            int iLdmcomandoexp = dr.GetOrdinal(this.Ldmcomandoexp);
            if (!dr.IsDBNull(iLdmcomandoexp)) entity.Ldmcomandoexp = dr.GetString(iLdmcomandoexp);

            int iLdmcomandoimp = dr.GetOrdinal(this.Ldmcomandoimp);
            if (!dr.IsDBNull(iLdmcomandoimp)) entity.Ldmcomandoimp = dr.GetString(iLdmcomandoimp);

            int iLdmenlace = dr.GetOrdinal(this.Ldmenlace);
            if (!dr.IsDBNull(iLdmenlace)) entity.Ldmenlace = dr.GetString(iLdmenlace);

            int iLdmusucreacion = dr.GetOrdinal(this.Ldmusucreacion);
            if (!dr.IsDBNull(iLdmusucreacion)) entity.Ldmusucreacion = dr.GetString(iLdmusucreacion);

            int iLdmfeccreacion = dr.GetOrdinal(this.Ldmfeccreacion);
            if (!dr.IsDBNull(iLdmfeccreacion)) entity.Ldmfeccreacion = dr.GetDateTime(iLdmfeccreacion);

            int iLdmusumodificacion = dr.GetOrdinal(this.Ldmusumodificacion);
            if (!dr.IsDBNull(iLdmusumodificacion)) entity.Ldmusumodificacion = dr.GetString(iLdmusumodificacion);

            int iLdmfecmodificacion = dr.GetOrdinal(this.Ldmfecmodificacion);
            if (!dr.IsDBNull(iLdmfecmodificacion)) entity.Ldmfecmodificacion = dr.GetDateTime(iLdmfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ldmcodi = "LDMCODI";
        public string Ldmfecha = "LDMFECHA";
        public string Vercodi = "VERCODI";
        public string Ldmfechapub = "LDMFECHAPUB";
        public string Ldmfechaimp = "LDMFECHAIMP";
        public string Ldmnomb = "LDMNOMB";
        public string Ldmtipo = "LDMTIPO";
        public string Ldmestadoserv = "LDMESTADOSERV";
        public string Ldmestadocli = "LDMESTADOCLI";
        public string Ldmnotaexp = "LDMNOTAEXP";
        public string Ldmnotaimp = "LDMNOTAIMP";
        public string Ldmmedioimp = "LDMMEDIOIMP";
        public string Ldmcomandoexp = "LDMCOMANDOEXP";
        public string Ldmcomandoimp = "LDMCOMANDOIMP";
        public string Ldmenlace = "LDMENLACE";
        public string Ldmusucreacion = "LDMUSUCREACION";
        public string Ldmfeccreacion = "LDMFECCREACION";
        public string Ldmusumodificacion = "LDMUSUMODIFICACION";
        public string Ldmfecmodificacion = "LDMFECMODIFICACION";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlListExportacion
        {
            get { return base.GetSqlXml("ListExportacion"); }
        }

        public string SqlListImportacion
        {
            get { return base.GetSqlXml("ListImportacion"); }
        }

        public string SqlGetMinId
        {
            get { return base.GetSqlXml("GetMinId"); }
        }        

        #endregion
    }
}
