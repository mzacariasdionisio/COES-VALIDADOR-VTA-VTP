using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RCA_CARGA_ESENCIAL
    /// </summary>
    public class RcaCargaEsencialRepository: RepositoryBase, IRcaCargaEsencialRepository
    {
        public RcaCargaEsencialRepository(string strConn): base(strConn)
        {
        }

        RcaCargaEsencialHelper helper = new RcaCargaEsencialHelper();

        public int Save(RcaCargaEsencialDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rccarecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi > 0 ? entity.Equicodi : (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Rccarecarga, DbType.Decimal, entity.Rccarecarga);
            dbProvider.AddInParameter(command, helper.Rccaredocumento, DbType.String, entity.Rccaredocumento);
            dbProvider.AddInParameter(command, helper.Rccarefecharecepcion, DbType.DateTime, entity.Rccarefecharecepcion);
            dbProvider.AddInParameter(command, helper.Rccareestado, DbType.String, entity.Rccareestado);
            dbProvider.AddInParameter(command, helper.Rccarenombarchivo, DbType.String, entity.Rccarenombarchivo);
            dbProvider.AddInParameter(command, helper.Rccareestregistro, DbType.String, entity.Rccareestregistro);
            dbProvider.AddInParameter(command, helper.Rccareusucreacion, DbType.String, entity.Rccareusucreacion);
            dbProvider.AddInParameter(command, helper.Rccarefeccreacion, DbType.DateTime, entity.Rccarefeccreacion);
            dbProvider.AddInParameter(command, helper.Rccareusumodificacion, DbType.String, entity.Rccareusumodificacion);
            dbProvider.AddInParameter(command, helper.Rccarefecmodificacion, DbType.DateTime, entity.Rccarefecmodificacion);
            dbProvider.AddInParameter(command, helper.Rccareorigen, DbType.Int32, entity.Rccareorigen);
            dbProvider.AddInParameter(command, helper.Rccaretipocarga, DbType.Int32, entity.Rccaretipocarga);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RcaCargaEsencialDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            //dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi > 0 ? entity.Equicodi : (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Rccarecarga, DbType.Decimal, entity.Rccarecarga);
            dbProvider.AddInParameter(command, helper.Rccaredocumento, DbType.String, entity.Rccaredocumento);
            dbProvider.AddInParameter(command, helper.Rccarefecharecepcion, DbType.DateTime, entity.Rccarefecharecepcion);
            dbProvider.AddInParameter(command, helper.Rccareestado, DbType.String, entity.Rccareestado);
            dbProvider.AddInParameter(command, helper.Rccarenombarchivo, DbType.String, entity.Rccarenombarchivo);
            dbProvider.AddInParameter(command, helper.Rccareestregistro, DbType.String, entity.Rccareestregistro);
            dbProvider.AddInParameter(command, helper.Rccareusucreacion, DbType.String, entity.Rccareusucreacion);
            dbProvider.AddInParameter(command, helper.Rccarefeccreacion, DbType.DateTime, entity.Rccarefeccreacion);
            dbProvider.AddInParameter(command, helper.Rccareusumodificacion, DbType.String, entity.Rccareusumodificacion);
            dbProvider.AddInParameter(command, helper.Rccarefecmodificacion, DbType.DateTime, entity.Rccarefecmodificacion);
            dbProvider.AddInParameter(command, helper.Rccaretipocarga, DbType.Int32, entity.Rccaretipocarga);
            dbProvider.AddInParameter(command, helper.Rccarecodi, DbType.Int32, entity.Rccarecodi);
            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rccarecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rccarecodi, DbType.Int32, rccarecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RcaCargaEsencialDTO GetById(int rccarecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rccarecodi, DbType.Int32, rccarecodi);
            RcaCargaEsencialDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RcaCargaEsencialDTO> List()
        {
            List<RcaCargaEsencialDTO> entitys = new List<RcaCargaEsencialDTO>();
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

        public List<RcaCargaEsencialDTO> GetByCriteria()
        {
            List<RcaCargaEsencialDTO> entitys = new List<RcaCargaEsencialDTO>();
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

        public List<RcaCargaEsencialDTO> ListarCargaEsencialFiltro(string vigente, string empresa, string documento, string cargaIni, 
            string cargaFin, string fecIni, string fecFin, string estadoRegistro, int origen, int regIni, int regFin)
        {
            List<RcaCargaEsencialDTO> entitys = new List<RcaCargaEsencialDTO>();

            string condicion = "";
            condicion = condicion + " AND RCCAREORIGEN = " + origen;

            if (origen.Equals(1))//Intranet
            {
                if (!string.IsNullOrEmpty(empresa))
                {
                    condicion = condicion + " AND EMP.EMPRRAZSOCIAL LIKE '%" + empresa.ToUpper() + "%'";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(empresa))
                {
                    condicion = condicion + " AND EMP.EMPRCODI = " + empresa;
                }                
            }
            

            if (!string.IsNullOrEmpty(documento))
            {
                condicion = condicion + " AND UPPER(RCCAREDOCUMENTO) LIKE '%" + documento.ToUpper() + "%'";
            }

            if (!string.IsNullOrEmpty(vigente))
            {
                condicion = condicion + " AND RCCAREESTADO  = '" + vigente+"'";
            } 
            
            if (cargaIni != null && !cargaIni.Equals(""))
            {
                condicion = condicion + " AND RCCARECARGA >= " + cargaIni;
            }

            if (cargaFin != null && !cargaFin.Equals(""))
            {
                condicion = condicion + " AND RCCARECARGA <= " + cargaFin;
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(CAE.RCCAREFECHARECEPCION) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(CAE.RCCAREFECHARECEPCION) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }           

            string queryString = string.Format(helper.SqlListFiltro, condicion, regFin, regIni);
            
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Rccareestregistro, DbType.String, estadoRegistro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCargaEsencialDTO entity = new RcaCargaEsencialDTO();

                    int iRccarecodi = dr.GetOrdinal(helper.Rccarecodi);
                    if (!dr.IsDBNull(iRccarecodi)) entity.Rccarecodi = Convert.ToInt32(dr.GetValue(iRccarecodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRccarecarga = dr.GetOrdinal(helper.Rccarecarga);
                    if (!dr.IsDBNull(iRccarecarga)) entity.Rccarecarga = dr.GetDecimal(iRccarecarga);

                    int iRccaredocumento = dr.GetOrdinal(helper.Rccaredocumento);
                    if (!dr.IsDBNull(iRccaredocumento)) entity.Rccaredocumento = dr.GetString(iRccaredocumento);

                    int iRccarefecharecepcion = dr.GetOrdinal(helper.Rccarefecharecepcion);
                    if (!dr.IsDBNull(iRccarefecharecepcion)) entity.Rccarefecharecepcion = dr.GetDateTime(iRccarefecharecepcion);

                    int iRccareestado = dr.GetOrdinal(helper.Rccareestado);
                    if (!dr.IsDBNull(iRccareestado)) entity.Rccareestado = dr.GetString(iRccareestado);

                    int iRccarenombarchivo = dr.GetOrdinal(helper.Rccarenombarchivo);
                    if (!dr.IsDBNull(iRccarenombarchivo)) entity.Rccarenombarchivo = dr.GetString(iRccarenombarchivo);

                    int iRccareestregistro = dr.GetOrdinal(helper.Rccareestregistro);
                    if (!dr.IsDBNull(iRccareestregistro)) entity.Rccareestregistro = dr.GetString(iRccareestregistro);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iRccaretipocarga = dr.GetOrdinal(helper.Rccaretipocarga);
                    if (!dr.IsDBNull(iRccaretipocarga)) entity.Rccaretipocarga = dr.GetInt32(iRccaretipocarga);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<RcaCargaEsencialDTO> ListarCargaEsencialPorPuntoMedicion(string puntoMedicion, string empresa)
        {
            List<RcaCargaEsencialDTO> entitys = new List<RcaCargaEsencialDTO>();

            string condicion = "";

            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + " AND EMP.emprcodi IN (" + empresa.ToUpper() + ")";
            }

            if (!string.IsNullOrEmpty(puntoMedicion))
            {
                condicion = condicion + " AND CAE.equicodi IN (" + puntoMedicion + ")";
            }

            string queryString = string.Format(helper.SqlListFiltroPorPuntoMedicion, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCargaEsencialDTO entity = new RcaCargaEsencialDTO();

                    int iRccarecodi = dr.GetOrdinal(helper.Rccarecodi);
                    if (!dr.IsDBNull(iRccarecodi)) entity.Rccarecodi = Convert.ToInt32(dr.GetValue(iRccarecodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRccarecarga = dr.GetOrdinal(helper.Rccarecarga);
                    if (!dr.IsDBNull(iRccarecarga)) entity.Rccarecarga = dr.GetDecimal(iRccarecarga);

                    int iRccaredocumento = dr.GetOrdinal(helper.Rccaredocumento);
                    if (!dr.IsDBNull(iRccaredocumento)) entity.Rccaredocumento = dr.GetString(iRccaredocumento);

                    int iRccarefecharecepcion = dr.GetOrdinal(helper.Rccarefecharecepcion);
                    if (!dr.IsDBNull(iRccarefecharecepcion)) entity.Rccarefecharecepcion = dr.GetDateTime(iRccarefecharecepcion);

                    int iRccareestado = dr.GetOrdinal(helper.Rccareestado);
                    if (!dr.IsDBNull(iRccareestado)) entity.Rccareestado = dr.GetString(iRccareestado);

                    int iRccarenombarchivo = dr.GetOrdinal(helper.Rccarenombarchivo);
                    if (!dr.IsDBNull(iRccarenombarchivo)) entity.Rccarenombarchivo = dr.GetString(iRccarenombarchivo);

                    int iRccareestregistro = dr.GetOrdinal(helper.Rccareestregistro);
                    if (!dr.IsDBNull(iRccareestregistro)) entity.Rccareestregistro = dr.GetString(iRccareestregistro);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iRccaretipocarga = dr.GetOrdinal(helper.Rccaretipocarga);
                    if (!dr.IsDBNull(iRccaretipocarga)) entity.Rccaretipocarga = dr.GetInt32(iRccaretipocarga);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<RcaCargaEsencialDTO> ListarCargaEsencialHistorial(int emprcodi, int equicodi, string estadoRegistro){
            List<RcaCargaEsencialDTO> entitys = new List<RcaCargaEsencialDTO>();

            string condicion = "";

            condicion = condicion + " AND CAE.emprcodi  = " + emprcodi + "";

            if (equicodi > 0)
            {
                condicion = condicion + " AND CAE.equicodi  = " + equicodi + "";
            }
            

            string queryString = string.Format(helper.SqlListHistorial, condicion);
            Console.WriteLine(queryString);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCargaEsencialDTO entity = new RcaCargaEsencialDTO();

                    int iRccarecodi = dr.GetOrdinal(helper.Rccarecodi);
                    if (!dr.IsDBNull(iRccarecodi)) entity.Rccarecodi = Convert.ToInt32(dr.GetValue(iRccarecodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRccarecarga = dr.GetOrdinal(helper.Rccarecarga);
                    if (!dr.IsDBNull(iRccarecarga)) entity.Rccarecarga = dr.GetDecimal(iRccarecarga);

                    int iRccaredocumento = dr.GetOrdinal(helper.Rccaredocumento);
                    if (!dr.IsDBNull(iRccaredocumento)) entity.Rccaredocumento = dr.GetString(iRccaredocumento);

                    int iRccarefecharecepcion = dr.GetOrdinal(helper.Rccarefecharecepcion);
                    if (!dr.IsDBNull(iRccarefecharecepcion)) entity.Rccarefecharecepcion = dr.GetDateTime(iRccarefecharecepcion);

                    int iRccareestado = dr.GetOrdinal(helper.Rccareestado);
                    if (!dr.IsDBNull(iRccareestado)) entity.Rccareestado = dr.GetString(iRccareestado);

                    int iRccarenombarchivo = dr.GetOrdinal(helper.Rccarenombarchivo);
                    if (!dr.IsDBNull(iRccarenombarchivo)) entity.Rccarenombarchivo = dr.GetString(iRccarenombarchivo);

                    int iRccareestregistro = dr.GetOrdinal(helper.Rccareestregistro);
                    if (!dr.IsDBNull(iRccareestregistro)) entity.Rccareestregistro = dr.GetString(iRccareestregistro);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);
                                        

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public RcaCargaEsencialDTO ObtenerPorCodigo(int rccarecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorCodigo);

            dbProvider.AddInParameter(command, helper.Rccarecodi, DbType.Int32, rccarecodi);
            RcaCargaEsencialDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new RcaCargaEsencialDTO();

                    int iRccarecodi = dr.GetOrdinal(helper.Rccarecodi);
                    if (!dr.IsDBNull(iRccarecodi)) entity.Rccarecodi = Convert.ToInt32(dr.GetValue(iRccarecodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRccarecarga = dr.GetOrdinal(helper.Rccarecarga);
                    if (!dr.IsDBNull(iRccarecarga)) entity.Rccarecarga = dr.GetDecimal(iRccarecarga);

                    int iRccaredocumento = dr.GetOrdinal(helper.Rccaredocumento);
                    if (!dr.IsDBNull(iRccaredocumento)) entity.Rccaredocumento = dr.GetString(iRccaredocumento);

                    int iRccarefecharecepcion = dr.GetOrdinal(helper.Rccarefecharecepcion);
                    if (!dr.IsDBNull(iRccarefecharecepcion)) entity.Rccarefecharecepcion = dr.GetDateTime(iRccarefecharecepcion);

                    int iRccareestado = dr.GetOrdinal(helper.Rccareestado);
                    if (!dr.IsDBNull(iRccareestado)) entity.Rccareestado = dr.GetString(iRccareestado);

                    int iRccarenombarchivo = dr.GetOrdinal(helper.Rccarenombarchivo);
                    if (!dr.IsDBNull(iRccarenombarchivo)) entity.Rccarenombarchivo = dr.GetString(iRccarenombarchivo);

                    int iRccareestregistro = dr.GetOrdinal(helper.Rccareestregistro);
                    if (!dr.IsDBNull(iRccareestregistro)) entity.Rccareestregistro = dr.GetString(iRccareestregistro);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iRccaretipocarga = dr.GetOrdinal(helper.Rccaretipocarga);
                    if (!dr.IsDBNull(iRccaretipocarga)) entity.Rccaretipocarga = Convert.ToInt32(dr.GetValue(iRccaretipocarga));
                }
            }

            return entity;
        }

        public int ListarCargaEsencialCount(string vigente, string empresa, string documento, string cargaIni,
            string cargaFin, string fecIni, string fecFin, string estadoRegistro, int origen)
        {
            List<RcaCargaEsencialDTO> entitys = new List<RcaCargaEsencialDTO>();

            string condicion = "";
            condicion = condicion + " AND RCCAREORIGEN = " + origen;

            if (origen.Equals(1))//Intranet
            {
                if (!string.IsNullOrEmpty(empresa))
                {
                    condicion = condicion + " AND EMP.EMPRRAZSOCIAL LIKE '%" + empresa.ToUpper() + "%'";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(empresa))
                {
                    condicion = condicion + " AND EMP.EMPRCODI = " + empresa;
                }
            }


            if (!string.IsNullOrEmpty(documento))
            {
                condicion = condicion + " AND UPPER(RCCAREDOCUMENTO) LIKE '%" + documento.ToUpper() + "%'";
            }

            if (!string.IsNullOrEmpty(vigente))
            {
                condicion = condicion + " AND RCCAREESTADO  = '" + vigente + "'";
            }

            if (cargaIni != null && !cargaIni.Equals(""))
            {
                condicion = condicion + " AND RCCARECARGA >= " + cargaIni;
            }

            if (cargaFin != null && !cargaFin.Equals(""))
            {
                condicion = condicion + " AND RCCARECARGA <= " + cargaFin;
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(CAE.RCCAREFECHARECEPCION) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(CAE.RCCAREFECHARECEPCION) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            string queryString = string.Format(helper.SqlListFiltroCount, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Rccareestregistro, DbType.String, estadoRegistro);

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

        public List<RcaCargaEsencialDTO> ListarCargaEsencialExcel(string vigente, string empresa, string documento, string cargaIni,
           string cargaFin, string fecIni, string fecFin, string estadoRegistro, int origen)
        {
            List<RcaCargaEsencialDTO> entitys = new List<RcaCargaEsencialDTO>();

            string condicion = "";
            condicion = condicion + " AND RCCAREORIGEN = " + origen;

            if (origen.Equals(1))//Intranet
            {
                if (!string.IsNullOrEmpty(empresa))
                {
                    condicion = condicion + " AND EMP.EMPRRAZSOCIAL LIKE '%" + empresa.ToUpper() + "%'";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(empresa))
                {
                    condicion = condicion + " AND EMP.EMPRCODI = " + empresa;
                }
            }


            if (!string.IsNullOrEmpty(documento))
            {
                condicion = condicion + " AND UPPER(RCCAREDOCUMENTO) LIKE '%" + documento.ToUpper() + "%'";
            }

            if (!string.IsNullOrEmpty(vigente))
            {
                condicion = condicion + " AND RCCAREESTADO  = '" + vigente + "'";
            }

            if (cargaIni != null && !cargaIni.Equals(""))
            {
                condicion = condicion + " AND RCCARECARGA >= " + cargaIni;
            }

            if (cargaFin != null && !cargaFin.Equals(""))
            {
                condicion = condicion + " AND RCCARECARGA <= " + cargaFin;
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(CAE.RCCAREFECHARECEPCION) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(CAE.RCCAREFECHARECEPCION) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            string queryString = string.Format(helper.SqlListFiltroExcel, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Rccareestregistro, DbType.String, estadoRegistro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCargaEsencialDTO entity = new RcaCargaEsencialDTO();

                    int iRccarecodi = dr.GetOrdinal(helper.Rccarecodi);
                    if (!dr.IsDBNull(iRccarecodi)) entity.Rccarecodi = Convert.ToInt32(dr.GetValue(iRccarecodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRccarecarga = dr.GetOrdinal(helper.Rccarecarga);
                    if (!dr.IsDBNull(iRccarecarga)) entity.Rccarecarga = dr.GetDecimal(iRccarecarga);

                    int iRccaredocumento = dr.GetOrdinal(helper.Rccaredocumento);
                    if (!dr.IsDBNull(iRccaredocumento)) entity.Rccaredocumento = dr.GetString(iRccaredocumento);

                    int iRccarefecharecepcion = dr.GetOrdinal(helper.Rccarefecharecepcion);
                    if (!dr.IsDBNull(iRccarefecharecepcion)) entity.Rccarefecharecepcion = dr.GetDateTime(iRccarefecharecepcion);

                    int iRccareestado = dr.GetOrdinal(helper.Rccareestado);
                    if (!dr.IsDBNull(iRccareestado)) entity.Rccareestado = dr.GetString(iRccareestado);

                    int iRccarenombarchivo = dr.GetOrdinal(helper.Rccarenombarchivo);
                    if (!dr.IsDBNull(iRccarenombarchivo)) entity.Rccarenombarchivo = dr.GetString(iRccarenombarchivo);

                    int iRccareestregistro = dr.GetOrdinal(helper.Rccareestregistro);
                    if (!dr.IsDBNull(iRccareestregistro)) entity.Rccareestregistro = dr.GetString(iRccareestregistro);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iRccaretipocarga = dr.GetOrdinal(helper.Rccaretipocarga);
                    if (!dr.IsDBNull(iRccaretipocarga)) entity.Rccaretipocarga = dr.GetInt32(iRccaretipocarga);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }
    }
}
