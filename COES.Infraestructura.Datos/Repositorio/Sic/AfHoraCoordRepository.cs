using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla AF_HORA_COORD
    /// </summary>
    public class AfHoraCoordRepository : RepositoryBase, IAfHoraCoordRepository
    {
        public AfHoraCoordRepository(string strConn) : base(strConn)
        {
        }

        AfHoraCoordHelper helper = new AfHoraCoordHelper();

        public int Save(AfHoraCoordDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Afhocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Afhofecmodificacion, DbType.DateTime, entity.Afhofecmodificacion);
            dbProvider.AddInParameter(command, helper.Afhousumodificacion, DbType.String, entity.Afhousumodificacion);
            dbProvider.AddInParameter(command, helper.Afhofeccreacion, DbType.DateTime, entity.Afhofeccreacion);
            dbProvider.AddInParameter(command, helper.Afhousucreacion, DbType.String, entity.Afhousucreacion);
            dbProvider.AddInParameter(command, helper.Afhofecha, DbType.DateTime, entity.Afhofecha);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, entity.Afecodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AfHoraCoordDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Afhofecmodificacion, DbType.DateTime, entity.Afhofecmodificacion);
            dbProvider.AddInParameter(command, helper.Afhousumodificacion, DbType.String, entity.Afhousumodificacion);
            dbProvider.AddInParameter(command, helper.Afhofeccreacion, DbType.DateTime, entity.Afhofeccreacion);
            dbProvider.AddInParameter(command, helper.Afhousucreacion, DbType.String, entity.Afhousucreacion);
            dbProvider.AddInParameter(command, helper.Afhofecha, DbType.DateTime, entity.Afhofecha);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Afhocodi, DbType.Int32, entity.Afhocodi);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, entity.Afecodi);
            dbProvider.AddInParameter(command, helper.Fdatcodi, DbType.Int32, entity.Fdatcodi);
            dbProvider.AddInParameter(command, helper.Afhmotivo, DbType.String, entity.Afhmotivo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int afhocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Afhocodi, DbType.Int32, afhocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AfHoraCoordDTO GetById(int afhocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Afhocodi, DbType.Int32, afhocodi);
            AfHoraCoordDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AfHoraCoordDTO> List()
        {
            List<AfHoraCoordDTO> entitys = new List<AfHoraCoordDTO>();
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

        public List<AfHoraCoordDTO> GetByCriteria()
        {
            List<AfHoraCoordDTO> entitys = new List<AfHoraCoordDTO>();
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

        #region Intranet CTAF

        public List<AfHoraCoordDTO> ListHoraCoord(int afecodi, int fdatcodi)
        {
            List<AfHoraCoordDTO> entitys = new List<AfHoraCoordDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListHoraCoord);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, afecodi);
            dbProvider.AddInParameter(command, helper.Fdatcodi, DbType.Int32, fdatcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AfHoraCoordDTO entity = helper.Create(dr);
                    int iIntsumcodi = dr.GetOrdinal("Intsumcodi");

                    if (!dr.IsDBNull(iIntsumcodi)) entity.Intsumcodi = dr.GetInt32(iIntsumcodi);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<AfHoraCoordDTO> ListHoraCoordCtaf(string afeanio, string afecorr, int fdatcodi)
        {
            List<AfHoraCoordDTO> entitys = new List<AfHoraCoordDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListHoraCoordCTAF);
            dbProvider.AddInParameter(command, helper.Afeanio, DbType.Int32, afeanio);
            dbProvider.AddInParameter(command, helper.Afecorr, DbType.Int32, afecorr);
            dbProvider.AddInParameter(command, helper.Fdatcodi, DbType.Int32, fdatcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AfHoraCoordDTO entity = helper.Create(dr);
                    int iIntsumcodi = dr.GetOrdinal("Intsumcodi");

                    if (!dr.IsDBNull(iIntsumcodi)) entity.Intsumcodi = dr.GetInt32(iIntsumcodi);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public void DeleteHoraCoord(int afecodi, int fdatcodi, int emprcodi, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                string query = string.Format(helper.SqlDeleteHoraCoord, afecodi, fdatcodi, emprcodi);
                dbCommand.CommandText = query;
                dbCommand.ExecuteNonQuery();
            }
        }

        public int Save(AfHoraCoordDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afhocodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afhofecmodificacion, DbType.DateTime, entity.Afhofecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afhousumodificacion, DbType.String, entity.Afhousumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afhofeccreacion, DbType.DateTime, entity.Afhofeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afhousucreacion, DbType.String, entity.Afhousucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afhofecha, DbType.DateTime, entity.Afhofecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afecodi, DbType.Int32, entity.Afecodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fdatcodi, DbType.Int32, entity.Fdatcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afhmotivo, DbType.String, entity.Afhmotivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumcodi, DbType.String, entity.Intsumcodi));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public List<AfHoraCoordDTO> ListHoraCoordSuministradora(int afecodi)
        {
            List<AfHoraCoordDTO> entitys = new List<AfHoraCoordDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListHoraCoordSuministradora);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, afecodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AfHoraCoordDTO entity = new AfHoraCoordDTO();
                    int iEmpresaSuministradora = dr.GetOrdinal("EmpresaSuministradora");
                    int iAfhofecha = dr.GetOrdinal("AFHOFECHA");
                    int iAfecodi = dr.GetOrdinal("AFECODI");

                    if (!dr.IsDBNull(iEmpresaSuministradora)) entity.EmpresaSuministradora = dr.GetString(iEmpresaSuministradora);
                    if (!dr.IsDBNull(iAfhofecha)) entity.Afhofecha = dr.GetDateTime(iAfhofecha);
                    if (!dr.IsDBNull(iAfecodi)) entity.Afecodi = dr.GetInt32(iAfecodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateHoraCoordSuministradora(AfHoraCoordDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateHoraCoordSuministradora);

            dbProvider.AddInParameter(command, helper.Afhofecha, DbType.DateTime, entity.Afhofecha);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, entity.Afecodi);
            dbProvider.AddInParameter(command, helper.Eracmfsuministrador, DbType.String, entity.EmpresaSuministradora);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<AfHoraCoordDTO> ListEmpClixSuministradora(string eracmfsuministrador)
        {
            List<AfHoraCoordDTO> entitys = new List<AfHoraCoordDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpClixSuministradora);
            dbProvider.AddInParameter(command, helper.Eracmfsuministrador, DbType.String, eracmfsuministrador);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AfHoraCoordDTO entity = new AfHoraCoordDTO();
                    int iEmprnombr = dr.GetOrdinal("EmpresaCliente");
                    int iEmpresaSuministradora = dr.GetOrdinal("EmpresaSuministradora");

                    if (!dr.IsDBNull(iEmprnombr)) entity.Emprnombr = dr.GetString(iEmprnombr);
                    if (!dr.IsDBNull(iEmpresaSuministradora)) entity.EmpresaSuministradora = dr.GetString(iEmpresaSuministradora);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
