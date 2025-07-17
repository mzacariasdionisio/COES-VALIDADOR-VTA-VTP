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
    /// Clase de acceso a datos de la tabla MP_PROPRECURSO
    /// </summary>
    public class MpProprecursoRepository: RepositoryBase, IMpProprecursoRepository
    {
        public MpProprecursoRepository(string strConn): base(strConn)
        {
        }

        MpProprecursoHelper helper = new MpProprecursoHelper();

        public void Save(MpProprecursoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mpropcodi, DbType.Int32, entity.Mpropcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mprvalfecvig, DbType.DateTime, entity.Mprvalfecvig));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mprvalvalor, DbType.String, entity.Mprvalvalor));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void Save(MpProprecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi);
            dbProvider.AddInParameter(command, helper.Mpropcodi, DbType.Int32, entity.Mpropcodi);
            dbProvider.AddInParameter(command, helper.Mprvalfecvig, DbType.DateTime, entity.Mprvalfecvig);
            dbProvider.AddInParameter(command, helper.Mprvalvalor, DbType.String, entity.Mprvalvalor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MpProprecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mprvalfecvig, DbType.DateTime, entity.Mprvalfecvig);
            dbProvider.AddInParameter(command, helper.Mprvalvalor, DbType.String, entity.Mprvalvalor);
            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi);
            dbProvider.AddInParameter(command, helper.Mpropcodi, DbType.Int32, entity.Mpropcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi, int mrecurcodi, int mpropcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, mrecurcodi);
            dbProvider.AddInParameter(command, helper.Mpropcodi, DbType.Int32, mpropcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi, int mrecurcodi, int mpropcodi, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlDelete;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mpropcodi, DbType.Int32, mpropcodi));

                dbCommand.ExecuteNonQuery();
            }
        }

        public MpProprecursoDTO GetById(int mtopcodi, int mrecurcodi, int mpropcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, mrecurcodi);
            dbProvider.AddInParameter(command, helper.Mpropcodi, DbType.Int32, mpropcodi);
            MpProprecursoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MpProprecursoDTO> List()
        {
            List<MpProprecursoDTO> entitys = new List<MpProprecursoDTO>();
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

        public List<MpProprecursoDTO> GetByCriteria()
        {
            List<MpProprecursoDTO> entitys = new List<MpProprecursoDTO>();
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

        public List<MpProprecursoDTO> ListarPorTopologia(int mtopcodi)
        {
            List<MpProprecursoDTO> entitys = new List<MpProprecursoDTO>();
            string query = string.Format(helper.SqlListarByTopologia, mtopcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MpProprecursoDTO> ListarPorTopologiaYRecurso(int mtopcodi, int recurcodi)
        {
            List<MpProprecursoDTO> entitys = new List<MpProprecursoDTO>();
            string query = string.Format(helper.SqlListarByTopologiaYRecurso, mtopcodi, recurcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        

        public void Update(MpProprecursoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mprvalfecvig, DbType.DateTime, entity.Mprvalfecvig));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mprvalvalor, DbType.String, entity.Mprvalvalor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mpropcodi, DbType.Int32, entity.Mpropcodi));

                dbCommand.ExecuteNonQuery();
            }
        }
    }
}
