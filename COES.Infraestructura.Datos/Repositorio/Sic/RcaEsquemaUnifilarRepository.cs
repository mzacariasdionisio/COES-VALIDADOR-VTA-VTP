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
    /// Clase de acceso a datos de la tabla RCA_ESQUEMA_UNIFILAR
    /// </summary>
    public class RcaEsquemaUnifilarRepository: RepositoryBase, IRcaEsquemaUnifilarRepository
    {
        public RcaEsquemaUnifilarRepository(string strConn): base(strConn)
        {
        }

        RcaEsquemaUnifilarHelper helper = new RcaEsquemaUnifilarHelper();

        public int Save(RcaEsquemaUnifilarDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rcesqucodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rcesqudocumento, DbType.String, entity.Rcesqudocumento);
            dbProvider.AddInParameter(command, helper.Rcesqufecharecepcion, DbType.DateTime, entity.Rcesqufecharecepcion);
            dbProvider.AddInParameter(command, helper.Rcesquestado, DbType.String, entity.Rcesquestado);
            dbProvider.AddInParameter(command, helper.Rcesqunombarchivo, DbType.String, entity.Rcesqunombarchivo);
            dbProvider.AddInParameter(command, helper.Rcesquestregistro, DbType.String, entity.Rcesquestregistro);
            dbProvider.AddInParameter(command, helper.Rcesquusucreacion, DbType.String, entity.Rcesquusucreacion);
            dbProvider.AddInParameter(command, helper.Rcesqufeccreacion, DbType.DateTime, entity.Rcesqufeccreacion);
            dbProvider.AddInParameter(command, helper.Rcesquusumodificacion, DbType.String, entity.Rcesquusumodificacion);
            dbProvider.AddInParameter(command, helper.Rcesqufecmodificacion, DbType.DateTime, entity.Rcesqufecmodificacion);
            dbProvider.AddInParameter(command, helper.Rcesquorigen, DbType.Int32, entity.Rcesquorigen);
            dbProvider.AddInParameter(command, helper.Rcesqunombarchivodatos, DbType.String, entity.Rcesqunombarchivodatos);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RcaEsquemaUnifilarDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rcesqudocumento, DbType.String, entity.Rcesqudocumento);
            dbProvider.AddInParameter(command, helper.Rcesqufecharecepcion, DbType.DateTime, entity.Rcesqufecharecepcion);
            dbProvider.AddInParameter(command, helper.Rcesquestado, DbType.String, entity.Rcesquestado);
            dbProvider.AddInParameter(command, helper.Rcesqunombarchivo, DbType.String, entity.Rcesqunombarchivo);
            dbProvider.AddInParameter(command, helper.Rcesqunombarchivodatos, DbType.String, entity.Rcesqunombarchivodatos);
            dbProvider.AddInParameter(command, helper.Rcesquestregistro, DbType.String, entity.Rcesquestregistro);
            dbProvider.AddInParameter(command, helper.Rcesquusucreacion, DbType.String, entity.Rcesquusucreacion);
            dbProvider.AddInParameter(command, helper.Rcesqufeccreacion, DbType.DateTime, entity.Rcesqufeccreacion);
            dbProvider.AddInParameter(command, helper.Rcesquusumodificacion, DbType.String, entity.Rcesquusumodificacion);
            dbProvider.AddInParameter(command, helper.Rcesqufecmodificacion, DbType.DateTime, entity.Rcesqufecmodificacion);
            dbProvider.AddInParameter(command, helper.Rcesqucodi, DbType.Int32, entity.Rcesqucodi);
            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rcesqucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rcesqucodi, DbType.Int32, rcesqucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RcaEsquemaUnifilarDTO GetById(int rcesqucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rcesqucodi, DbType.Int32, rcesqucodi);
            RcaEsquemaUnifilarDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RcaEsquemaUnifilarDTO> List()
        {
            List<RcaEsquemaUnifilarDTO> entitys = new List<RcaEsquemaUnifilarDTO>();
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

        public List<RcaEsquemaUnifilarDTO> GetByCriteria()
        {
            List<RcaEsquemaUnifilarDTO> entitys = new List<RcaEsquemaUnifilarDTO>();
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

        public List<RcaEsquemaUnifilarDTO> ListarEsquemaUnifilarExcel(string empresa, string codigoSuministro, string fecIni, string fecFin, int origen)
        {
            List<RcaEsquemaUnifilarDTO> entitys = new List<RcaEsquemaUnifilarDTO>();

            string condicion = "";
            condicion = condicion + " AND RCESQUORIGEN = " + origen;

            if (origen.Equals(1))
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
            

            if (!string.IsNullOrEmpty(codigoSuministro))
            {
                condicion = condicion + " AND UPPER(EQU.OSINERGCODI) LIKE '%" + codigoSuministro.ToUpper() + "%'";
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(EUN.RCESQUFECHARECEPCION) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(EUN.RCESQUFECHARECEPCION) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            string queryString = string.Format(helper.SqlListFiltroExcel, condicion, origen);
            Console.WriteLine(queryString);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaEsquemaUnifilarDTO entity = new RcaEsquemaUnifilarDTO();

                    int iRcesqucodi = dr.GetOrdinal(helper.Rcesqucodi);
                    if (!dr.IsDBNull(iRcesqucodi)) entity.Rcesqucodi = Convert.ToInt32(dr.GetValue(iRcesqucodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcesqudocumento = dr.GetOrdinal(helper.Rcesqudocumento);
                    if (!dr.IsDBNull(iRcesqudocumento)) entity.Rcesqudocumento = dr.GetString(iRcesqudocumento);

                    int iRcesqufecharecepcion = dr.GetOrdinal(helper.Rcesqufecharecepcion);
                    if (!dr.IsDBNull(iRcesqufecharecepcion)) entity.Rcesqufecharecepcion = dr.GetDateTime(iRcesqufecharecepcion);

                    int iRcesquestado = dr.GetOrdinal(helper.Rcesquestado);
                    if (!dr.IsDBNull(iRcesquestado)) entity.Rcesquestado = dr.GetString(iRcesquestado);

                    int iRcesqunombarchivo = dr.GetOrdinal(helper.Rcesqunombarchivo);
                    if (!dr.IsDBNull(iRcesqunombarchivo)) entity.Rcesqunombarchivo = dr.GetString(iRcesqunombarchivo);

                    int iRcesquestregistro = dr.GetOrdinal(helper.Rcesquestregistro);
                    if (!dr.IsDBNull(iRcesquestregistro)) entity.Rcesquestregistro = dr.GetString(iRcesquestregistro);

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

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);


                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<RcaEsquemaUnifilarDTO> ListarEsquemaUnifilarHistorial(int emprcodi, int equicodi)
        {
            List<RcaEsquemaUnifilarDTO> entitys = new List<RcaEsquemaUnifilarDTO>();

            string condicion = "";

            condicion = condicion + " AND EUN.emprcodi  = " + emprcodi + "";

            condicion = condicion + " AND EUN.equicodi  = " + equicodi + "";

            string queryString = string.Format(helper.SqlListHistorial, condicion);
            Console.WriteLine(queryString);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaEsquemaUnifilarDTO entity = new RcaEsquemaUnifilarDTO();


                    int iRcesqucodi = dr.GetOrdinal(helper.Rcesqucodi);
                    if (!dr.IsDBNull(iRcesqucodi)) entity.Rcesqucodi = Convert.ToInt32(dr.GetValue(iRcesqucodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcesqudocumento = dr.GetOrdinal(helper.Rcesqudocumento);
                    if (!dr.IsDBNull(iRcesqudocumento)) entity.Rcesqudocumento = dr.GetString(iRcesqudocumento);

                    int iRcesqufecharecepcion = dr.GetOrdinal(helper.Rcesqufecharecepcion);
                    if (!dr.IsDBNull(iRcesqufecharecepcion)) entity.Rcesqufecharecepcion = dr.GetDateTime(iRcesqufecharecepcion);

                    int iRcesquestado = dr.GetOrdinal(helper.Rcesquestado);
                    if (!dr.IsDBNull(iRcesquestado)) entity.Rcesquestado = dr.GetString(iRcesquestado);

                    int iRcesqunombarchivo = dr.GetOrdinal(helper.Rcesqunombarchivo);
                    if (!dr.IsDBNull(iRcesqunombarchivo)) entity.Rcesqunombarchivo = dr.GetString(iRcesqunombarchivo);

                    int iRcesquestregistro = dr.GetOrdinal(helper.Rcesquestregistro);
                    if (!dr.IsDBNull(iRcesquestregistro)) entity.Rcesquestregistro = dr.GetString(iRcesquestregistro);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprrazsocial = dr.GetString(iEmprnomb);

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

        public RcaEsquemaUnifilarDTO ObtenerPorCodigo(int rccarecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorCodigo);

            dbProvider.AddInParameter(command, helper.Rcesqucodi, DbType.Int32, rccarecodi);
            RcaEsquemaUnifilarDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new RcaEsquemaUnifilarDTO();


                    int iRcesqucodi = dr.GetOrdinal(helper.Rcesqucodi);
                    if (!dr.IsDBNull(iRcesqucodi)) entity.Rcesqucodi = Convert.ToInt32(dr.GetValue(iRcesqucodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcesqudocumento = dr.GetOrdinal(helper.Rcesqudocumento);
                    if (!dr.IsDBNull(iRcesqudocumento)) entity.Rcesqudocumento = dr.GetString(iRcesqudocumento);

                    int iRcesqufecharecepcion = dr.GetOrdinal(helper.Rcesqufecharecepcion);
                    if (!dr.IsDBNull(iRcesqufecharecepcion)) entity.Rcesqufecharecepcion = dr.GetDateTime(iRcesqufecharecepcion);

                    int iRcesquestado = dr.GetOrdinal(helper.Rcesquestado);
                    if (!dr.IsDBNull(iRcesquestado)) entity.Rcesquestado = dr.GetString(iRcesquestado);

                    int iRcesqunombarchivo = dr.GetOrdinal(helper.Rcesqunombarchivo);
                    if (!dr.IsDBNull(iRcesqunombarchivo)) entity.Rcesqunombarchivo = dr.GetString(iRcesqunombarchivo);

                    int iRcesquestregistro = dr.GetOrdinal(helper.Rcesquestregistro);
                    if (!dr.IsDBNull(iRcesquestregistro)) entity.Rcesquestregistro = dr.GetString(iRcesquestregistro);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprrazsocial = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iRcesqunombarchivodatos = dr.GetOrdinal(helper.Rcesqunombarchivodatos);
                    if (!dr.IsDBNull(iRcesqunombarchivodatos)) entity.Rcesqunombarchivodatos = dr.GetString(iRcesqunombarchivodatos);
                }
            }

            return entity;
        }

        public List<RcaEsquemaUnifilarDTO> ListarEsquemaUnifilarFiltro(string empresa, string codigoSuministro, string fecIni, string fecFin, int origen, int regIni, int regFin)
        {
            List<RcaEsquemaUnifilarDTO> entitys = new List<RcaEsquemaUnifilarDTO>();

            string condicion = "";
            condicion = condicion + " AND RCESQUORIGEN = " + origen;

            if (origen.Equals(1))
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


            if (!string.IsNullOrEmpty(codigoSuministro))
            {
                condicion = condicion + " AND UPPER(EQU.OSINERGCODI) LIKE '%" + codigoSuministro.ToUpper() + "%'";
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(EUN.RCESQUFECHARECEPCION) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(EUN.RCESQUFECHARECEPCION) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            string queryString = string.Format(helper.SqlListFiltro, condicion, origen, regFin, regIni);
            Console.WriteLine(queryString);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaEsquemaUnifilarDTO entity = new RcaEsquemaUnifilarDTO();

                    int iRcesqucodi = dr.GetOrdinal(helper.Rcesqucodi);
                    if (!dr.IsDBNull(iRcesqucodi)) entity.Rcesqucodi = Convert.ToInt32(dr.GetValue(iRcesqucodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcesqudocumento = dr.GetOrdinal(helper.Rcesqudocumento);
                    if (!dr.IsDBNull(iRcesqudocumento)) entity.Rcesqudocumento = dr.GetString(iRcesqudocumento);

                    int iRcesqufecharecepcion = dr.GetOrdinal(helper.Rcesqufecharecepcion);
                    if (!dr.IsDBNull(iRcesqufecharecepcion)) entity.Rcesqufecharecepcion = dr.GetDateTime(iRcesqufecharecepcion);

                    int iRcesquestado = dr.GetOrdinal(helper.Rcesquestado);
                    if (!dr.IsDBNull(iRcesquestado)) entity.Rcesquestado = dr.GetString(iRcesquestado);

                    int iRcesqunombarchivo = dr.GetOrdinal(helper.Rcesqunombarchivo);
                    if (!dr.IsDBNull(iRcesqunombarchivo)) entity.Rcesqunombarchivo = dr.GetString(iRcesqunombarchivo);

                    int iRcesquestregistro = dr.GetOrdinal(helper.Rcesquestregistro);
                    if (!dr.IsDBNull(iRcesquestregistro)) entity.Rcesquestregistro = dr.GetString(iRcesquestregistro);

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

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);


                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public int ListarEsquemaUnifilarCount(string empresa, string codigoSuministro, string fecIni, string fecFin, int origen)
        {
            List<RcaEsquemaUnifilarDTO> entitys = new List<RcaEsquemaUnifilarDTO>();

            string condicion = "";
            condicion = condicion + " AND RCESQUORIGEN = " + origen;

            if (origen.Equals(1))
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


            if (!string.IsNullOrEmpty(codigoSuministro))
            {
                condicion = condicion + " AND UPPER(EQU.OSINERGCODI) LIKE '%" + codigoSuministro.ToUpper() + "%'";
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(EUN.RCESQUFECHARECEPCION) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(EUN.RCESQUFECHARECEPCION) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            string queryString = string.Format(helper.SqlListFiltroCount, condicion, origen);
            Console.WriteLine(queryString);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

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
