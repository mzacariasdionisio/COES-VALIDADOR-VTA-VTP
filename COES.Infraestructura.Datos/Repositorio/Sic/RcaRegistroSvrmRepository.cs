using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RCA_REGISTRO_SVRM
    /// </summary>
    public class RcaRegistroSvrmRepository: RepositoryBase, IRcaRegistroSvrmRepository
    {
        public RcaRegistroSvrmRepository(string strConn): base(strConn)
        {
        }

        RcaRegistroSvrmHelper helper = new RcaRegistroSvrmHelper();

        public int Save(RcaRegistroSvrmDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rcsvrmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rcsvrmhperacmf, DbType.Decimal, entity.Rcsvrmhperacmf);
            dbProvider.AddInParameter(command, helper.Rcsvrmhperacmt, DbType.Decimal, entity.Rcsvrmhperacmt);
            dbProvider.AddInParameter(command, helper.Rcsvrmhfperacmf, DbType.Decimal, entity.Rcsvrmhfperacmf);
            dbProvider.AddInParameter(command, helper.Rcsvrmhfperacmt, DbType.Decimal, entity.Rcsvrmhfperacmt);
            dbProvider.AddInParameter(command, helper.Rcsvrmmaxdemcont, DbType.Decimal, entity.Rcsvrmmaxdemcont);
            dbProvider.AddInParameter(command, helper.Rcsvrmmaxdemdisp, DbType.Decimal, entity.Rcsvrmmaxdemdisp);
            dbProvider.AddInParameter(command, helper.Rcsvrmmaxdemcomp, DbType.Decimal, entity.Rcsvrmmaxdemcomp);
            dbProvider.AddInParameter(command, helper.Rcsvrmdocumento, DbType.String, entity.Rcsvrmdocumento);
            dbProvider.AddInParameter(command, helper.Rcsvrmfechavigencia, DbType.DateTime, entity.Rcsvrmfechavigencia);
            dbProvider.AddInParameter(command, helper.Rcsvrmestado, DbType.String, entity.Rcsvrmestado);
            dbProvider.AddInParameter(command, helper.Rcsvrmestregistro, DbType.String, entity.Rcsvrmestregistro);
            dbProvider.AddInParameter(command, helper.Rcsvrmusucreacion, DbType.String, entity.Rcsvrmusucreacion);
            dbProvider.AddInParameter(command, helper.Rcsvrmfeccreacion, DbType.DateTime, entity.Rcsvrmfeccreacion);
            dbProvider.AddInParameter(command, helper.Rcsvrmusumodificacion, DbType.String, entity.Rcsvrmusumodificacion);
            dbProvider.AddInParameter(command, helper.Rcsvrmfecmodificacion, DbType.DateTime, entity.Rcsvrmfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RcaRegistroSvrmDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rcsvrmhperacmf, DbType.Decimal, entity.Rcsvrmhperacmf);
            dbProvider.AddInParameter(command, helper.Rcsvrmhperacmt, DbType.Decimal, entity.Rcsvrmhperacmt);
            dbProvider.AddInParameter(command, helper.Rcsvrmhfperacmf, DbType.Decimal, entity.Rcsvrmhfperacmf);
            dbProvider.AddInParameter(command, helper.Rcsvrmhfperacmt, DbType.Decimal, entity.Rcsvrmhfperacmt);
            dbProvider.AddInParameter(command, helper.Rcsvrmmaxdemcont, DbType.Decimal, entity.Rcsvrmmaxdemcont);
            dbProvider.AddInParameter(command, helper.Rcsvrmmaxdemdisp, DbType.Decimal, entity.Rcsvrmmaxdemdisp);
            dbProvider.AddInParameter(command, helper.Rcsvrmmaxdemcomp, DbType.Decimal, entity.Rcsvrmmaxdemcomp);
            dbProvider.AddInParameter(command, helper.Rcsvrmdocumento, DbType.String, entity.Rcsvrmdocumento);
            dbProvider.AddInParameter(command, helper.Rcsvrmfechavigencia, DbType.DateTime, entity.Rcsvrmfechavigencia);
            dbProvider.AddInParameter(command, helper.Rcsvrmestado, DbType.String, entity.Rcsvrmestado);
            dbProvider.AddInParameter(command, helper.Rcsvrmestregistro, DbType.String, entity.Rcsvrmestregistro);
            dbProvider.AddInParameter(command, helper.Rcsvrmusucreacion, DbType.String, entity.Rcsvrmusucreacion);
            dbProvider.AddInParameter(command, helper.Rcsvrmfeccreacion, DbType.DateTime, entity.Rcsvrmfeccreacion);
            dbProvider.AddInParameter(command, helper.Rcsvrmusumodificacion, DbType.String, entity.Rcsvrmusumodificacion);
            dbProvider.AddInParameter(command, helper.Rcsvrmfecmodificacion, DbType.DateTime, entity.Rcsvrmfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rcsvrmcodi, DbType.Int32, entity.Rcsvrmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rcsvrmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rcsvrmcodi, DbType.Int32, rcsvrmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RcaRegistroSvrmDTO GetById(int rcsvrmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rcsvrmcodi, DbType.Int32, rcsvrmcodi);
            RcaRegistroSvrmDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RcaRegistroSvrmDTO> List()
        {
            List<RcaRegistroSvrmDTO> entitys = new List<RcaRegistroSvrmDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RcaRegistroSvrmDTO> GetByCriteria()
        {
            List<RcaRegistroSvrmDTO> entitys = new List<RcaRegistroSvrmDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }


        public List<RcaRegistroSvrmDTO> ListRcaRegistroSvrmsExcel(string empresa, string codigoSuministro, string fecIni, string fecFin, string maxDemComprometidaIni, string maxDemComprometidaFin, string estadoRegistro)
        {
            List<RcaRegistroSvrmDTO> entitys = new List<RcaRegistroSvrmDTO>();

            string condicion = "";

            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + " AND EMPR.EMPRRAZSOCIAL LIKE '%" + empresa.ToUpper() + "%'";
            }

            if (!string.IsNullOrEmpty(codigoSuministro))
            {
                condicion = condicion + " AND UPPER(EQU.OSINERGCODI) LIKE '%" + codigoSuministro.ToUpper() + "%'";
            }

            if (maxDemComprometidaIni != null && !maxDemComprometidaIni.Equals(""))
            {
                condicion = condicion + " AND RCSVRMMAXDEMCOMP >= " + maxDemComprometidaIni;
            }

            if (maxDemComprometidaFin != null && !maxDemComprometidaFin.Equals(""))
            {
                condicion = condicion + " AND RCSVRMMAXDEMCOMP <= " + maxDemComprometidaFin;
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(RCSVRMFECHAVIGENCIA) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(RCSVRMFECHAVIGENCIA) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            string queryString = string.Format(helper.SqlListFiltroExcel, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Rcsvrmestregistro, DbType.String, estadoRegistro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaRegistroSvrmDTO entity = new RcaRegistroSvrmDTO();

                    int iRcsvrmcodi = dr.GetOrdinal(helper.Rcsvrmcodi);
                    if (!dr.IsDBNull(iRcsvrmcodi)) entity.Rcsvrmcodi = Convert.ToInt32(dr.GetValue(iRcsvrmcodi));

                    //int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcsvrmhperacmf = dr.GetOrdinal(helper.Rcsvrmhperacmf);
                    if (!dr.IsDBNull(iRcsvrmhperacmf)) entity.Rcsvrmhperacmf = dr.GetDecimal(iRcsvrmhperacmf);

                    int iRcsvrmhperacmt = dr.GetOrdinal(helper.Rcsvrmhperacmt);
                    if (!dr.IsDBNull(iRcsvrmhperacmt)) entity.Rcsvrmhperacmt = dr.GetDecimal(iRcsvrmhperacmt);

                    int iRcsvrmhfperacmf = dr.GetOrdinal(helper.Rcsvrmhfperacmf);
                    if (!dr.IsDBNull(iRcsvrmhfperacmf)) entity.Rcsvrmhfperacmf = dr.GetDecimal(iRcsvrmhfperacmf);

                    int iRcsvrmhfperacmt = dr.GetOrdinal(helper.Rcsvrmhfperacmt);
                    if (!dr.IsDBNull(iRcsvrmhfperacmt)) entity.Rcsvrmhfperacmt = dr.GetDecimal(iRcsvrmhfperacmt);

                    int iRcsvrmmaxdemcont = dr.GetOrdinal(helper.Rcsvrmmaxdemcont);
                    if (!dr.IsDBNull(iRcsvrmmaxdemcont)) entity.Rcsvrmmaxdemcont = dr.GetDecimal(iRcsvrmmaxdemcont);

                    int iRcsvrmmaxdemdisp = dr.GetOrdinal(helper.Rcsvrmmaxdemdisp);
                    if (!dr.IsDBNull(iRcsvrmmaxdemdisp)) entity.Rcsvrmmaxdemdisp = dr.GetDecimal(iRcsvrmmaxdemdisp);

                    int iRcsvrmmaxdemcomp = dr.GetOrdinal(helper.Rcsvrmmaxdemcomp);
                    if (!dr.IsDBNull(iRcsvrmmaxdemcomp)) entity.Rcsvrmmaxdemcomp = dr.GetDecimal(iRcsvrmmaxdemcomp);

                    int iRcsvrmfechavigencia = dr.GetOrdinal(helper.Rcsvrmfechavigencia);
                    if (!dr.IsDBNull(iRcsvrmfechavigencia)) entity.Rcsvrmfechavigencia = dr.GetDateTime(iRcsvrmfechavigencia);

                    int iRcsvrmestado = dr.GetOrdinal(helper.Rcsvrmestado);
                    if (!dr.IsDBNull(iRcsvrmestado)) entity.Rcsvrmestado = dr.GetString(iRcsvrmestado);

                    int iRcsvrmdocumento = dr.GetOrdinal(helper.Rcsvrmdocumento);
                    if (!dr.IsDBNull(iRcsvrmdocumento)) entity.Rcsvrmdocumento = dr.GetString(iRcsvrmdocumento);

                    int iRcsvrmestregistro = dr.GetOrdinal(helper.Rcsvrmestregistro);
                    if (!dr.IsDBNull(iRcsvrmestregistro)) entity.Rcsvrmestregistro = dr.GetString(iRcsvrmestregistro);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);                    

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iEmprsum = dr.GetOrdinal(helper.Emprsum);
                    if (!dr.IsDBNull(iEmprsum)) entity.Emprsum = dr.GetString(iEmprsum);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public RcaRegistroSvrmDTO ObtenerPorCodigo(int rcsvrmcodi)
        {
            RcaRegistroSvrmDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorCodigo);
            dbProvider.AddInParameter(command, helper.Rcsvrmcodi, DbType.Int32, rcsvrmcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new RcaRegistroSvrmDTO();

                    int iRcsvrmcodi = dr.GetOrdinal(helper.Rcsvrmcodi);
                    if (!dr.IsDBNull(iRcsvrmcodi)) entity.Rcsvrmcodi = Convert.ToInt32(dr.GetValue(iRcsvrmcodi));

                    //int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcsvrmhperacmf = dr.GetOrdinal(helper.Rcsvrmhperacmf);
                    if (!dr.IsDBNull(iRcsvrmhperacmf)) entity.Rcsvrmhperacmf = dr.GetDecimal(iRcsvrmhperacmf);

                    int iRcsvrmhperacmt = dr.GetOrdinal(helper.Rcsvrmhperacmt);
                    if (!dr.IsDBNull(iRcsvrmhperacmt)) entity.Rcsvrmhperacmt = dr.GetDecimal(iRcsvrmhperacmt);

                    int iRcsvrmhfperacmf = dr.GetOrdinal(helper.Rcsvrmhfperacmf);
                    if (!dr.IsDBNull(iRcsvrmhfperacmf)) entity.Rcsvrmhfperacmf = dr.GetDecimal(iRcsvrmhfperacmf);

                    int iRcsvrmhfperacmt = dr.GetOrdinal(helper.Rcsvrmhfperacmt);
                    if (!dr.IsDBNull(iRcsvrmhfperacmt)) entity.Rcsvrmhfperacmt = dr.GetDecimal(iRcsvrmhfperacmt);

                    int iRcsvrmmaxdemcont = dr.GetOrdinal(helper.Rcsvrmmaxdemcont);
                    if (!dr.IsDBNull(iRcsvrmmaxdemcont)) entity.Rcsvrmmaxdemcont = dr.GetDecimal(iRcsvrmmaxdemcont);

                    int iRcsvrmmaxdemdisp = dr.GetOrdinal(helper.Rcsvrmmaxdemdisp);
                    if (!dr.IsDBNull(iRcsvrmmaxdemdisp)) entity.Rcsvrmmaxdemdisp = dr.GetDecimal(iRcsvrmmaxdemdisp);

                    int iRcsvrmmaxdemcomp = dr.GetOrdinal(helper.Rcsvrmmaxdemcomp);
                    if (!dr.IsDBNull(iRcsvrmmaxdemcomp)) entity.Rcsvrmmaxdemcomp = dr.GetDecimal(iRcsvrmmaxdemcomp);

                    int iRcsvrmfechavigencia = dr.GetOrdinal(helper.Rcsvrmfechavigencia);
                    if (!dr.IsDBNull(iRcsvrmfechavigencia)) entity.Rcsvrmfechavigencia = dr.GetDateTime(iRcsvrmfechavigencia);

                    int iRcsvrmestado = dr.GetOrdinal(helper.Rcsvrmestado);
                    if (!dr.IsDBNull(iRcsvrmestado)) entity.Rcsvrmestado = dr.GetString(iRcsvrmestado);

                    int iRcsvrmdocumento = dr.GetOrdinal(helper.Rcsvrmdocumento);
                    if (!dr.IsDBNull(iRcsvrmdocumento)) entity.Rcsvrmdocumento = dr.GetString(iRcsvrmdocumento);

                    int iRcsvrmestregistro = dr.GetOrdinal(helper.Rcsvrmestregistro);
                    if (!dr.IsDBNull(iRcsvrmestregistro)) entity.Rcsvrmestregistro = dr.GetString(iRcsvrmestregistro);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    
                }
            }

            return entity;

        }


        public List<RcaRegistroSvrmDTO> ListRcaRegistroSvrmsFiltro(string empresa, string codigoSuministro, string fecIni, string fecFin, string maxDemComprometidaIni, 
            string maxDemComprometidaFin, string estadoRegistro, int regIni, int regFin)
        {
            List<RcaRegistroSvrmDTO> entitys = new List<RcaRegistroSvrmDTO>();

            string condicion = "";

            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + " AND EMPR.EMPRRAZSOCIAL LIKE '%" + empresa.ToUpper() + "%'";
            }

            if (!string.IsNullOrEmpty(codigoSuministro))
            {
                condicion = condicion + " AND UPPER(EQU.OSINERGCODI) LIKE '%" + codigoSuministro.ToUpper() + "%'";
            }

            if (maxDemComprometidaIni != null && !maxDemComprometidaIni.Equals(""))
            {
                condicion = condicion + " AND RCSVRMMAXDEMCOMP >= " + maxDemComprometidaIni;
            }

            if (maxDemComprometidaFin != null && !maxDemComprometidaFin.Equals(""))
            {
                condicion = condicion + " AND RCSVRMMAXDEMCOMP <= " + maxDemComprometidaFin;
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(RCSVRMFECHAVIGENCIA) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(RCSVRMFECHAVIGENCIA) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            string queryString = string.Format(helper.SqlListFiltro, condicion, regFin, regIni);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Rcsvrmestregistro, DbType.String, estadoRegistro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaRegistroSvrmDTO entity = new RcaRegistroSvrmDTO();

                    int iRcsvrmcodi = dr.GetOrdinal(helper.Rcsvrmcodi);
                    if (!dr.IsDBNull(iRcsvrmcodi)) entity.Rcsvrmcodi = Convert.ToInt32(dr.GetValue(iRcsvrmcodi));

                    //int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcsvrmhperacmf = dr.GetOrdinal(helper.Rcsvrmhperacmf);
                    if (!dr.IsDBNull(iRcsvrmhperacmf)) entity.Rcsvrmhperacmf = dr.GetDecimal(iRcsvrmhperacmf);

                    int iRcsvrmhperacmt = dr.GetOrdinal(helper.Rcsvrmhperacmt);
                    if (!dr.IsDBNull(iRcsvrmhperacmt)) entity.Rcsvrmhperacmt = dr.GetDecimal(iRcsvrmhperacmt);

                    int iRcsvrmhfperacmf = dr.GetOrdinal(helper.Rcsvrmhfperacmf);
                    if (!dr.IsDBNull(iRcsvrmhfperacmf)) entity.Rcsvrmhfperacmf = dr.GetDecimal(iRcsvrmhfperacmf);

                    int iRcsvrmhfperacmt = dr.GetOrdinal(helper.Rcsvrmhfperacmt);
                    if (!dr.IsDBNull(iRcsvrmhfperacmt)) entity.Rcsvrmhfperacmt = dr.GetDecimal(iRcsvrmhfperacmt);

                    int iRcsvrmmaxdemcont = dr.GetOrdinal(helper.Rcsvrmmaxdemcont);
                    if (!dr.IsDBNull(iRcsvrmmaxdemcont)) entity.Rcsvrmmaxdemcont = dr.GetDecimal(iRcsvrmmaxdemcont);

                    int iRcsvrmmaxdemdisp = dr.GetOrdinal(helper.Rcsvrmmaxdemdisp);
                    if (!dr.IsDBNull(iRcsvrmmaxdemdisp)) entity.Rcsvrmmaxdemdisp = dr.GetDecimal(iRcsvrmmaxdemdisp);

                    int iRcsvrmmaxdemcomp = dr.GetOrdinal(helper.Rcsvrmmaxdemcomp);
                    if (!dr.IsDBNull(iRcsvrmmaxdemcomp)) entity.Rcsvrmmaxdemcomp = dr.GetDecimal(iRcsvrmmaxdemcomp);

                    int iRcsvrmfechavigencia = dr.GetOrdinal(helper.Rcsvrmfechavigencia);
                    if (!dr.IsDBNull(iRcsvrmfechavigencia)) entity.Rcsvrmfechavigencia = dr.GetDateTime(iRcsvrmfechavigencia);

                    int iRcsvrmestado = dr.GetOrdinal(helper.Rcsvrmestado);
                    if (!dr.IsDBNull(iRcsvrmestado)) entity.Rcsvrmestado = dr.GetString(iRcsvrmestado);

                    int iRcsvrmdocumento = dr.GetOrdinal(helper.Rcsvrmdocumento);
                    if (!dr.IsDBNull(iRcsvrmdocumento)) entity.Rcsvrmdocumento = dr.GetString(iRcsvrmdocumento);

                    int iRcsvrmestregistro = dr.GetOrdinal(helper.Rcsvrmestregistro);
                    if (!dr.IsDBNull(iRcsvrmestregistro)) entity.Rcsvrmestregistro = dr.GetString(iRcsvrmestregistro);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iEmprsum = dr.GetOrdinal(helper.Emprsum);
                    if (!dr.IsDBNull(iEmprsum)) entity.Emprsum = dr.GetString(iEmprsum);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public int ListRcaRegistroSvrmsCount(string empresa, string codigoSuministro, string fecIni, string fecFin, string maxDemComprometidaIni, string maxDemComprometidaFin, string estadoRegistro)
        {
            List<RcaRegistroSvrmDTO> entitys = new List<RcaRegistroSvrmDTO>();

            string condicion = "";

            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + " AND EMPR.EMPRRAZSOCIAL LIKE '%" + empresa.ToUpper() + "%'";
            }

            if (!string.IsNullOrEmpty(codigoSuministro))
            {
                condicion = condicion + " AND UPPER(EQU.OSINERGCODI) LIKE '%" + codigoSuministro.ToUpper() + "%'";
            }

            if (maxDemComprometidaIni != null && !maxDemComprometidaIni.Equals(""))
            {
                condicion = condicion + " AND RCSVRMMAXDEMCOMP >= " + maxDemComprometidaIni;
            }

            if (maxDemComprometidaFin != null && !maxDemComprometidaFin.Equals(""))
            {
                condicion = condicion + " AND RCSVRMMAXDEMCOMP <= " + maxDemComprometidaFin;
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(RCSVRMFECHAVIGENCIA) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(RCSVRMFECHAVIGENCIA) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            string queryString = string.Format(helper.SqlListFiltroCount, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Rcsvrmestregistro, DbType.String, estadoRegistro);

            int cant = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iQregistros = dr.GetOrdinal(helper.Qregistros);
                    cant = Convert.ToInt32(dr.GetValue(iQregistros));
                }
            }
            return cant;

        }
    }
}
