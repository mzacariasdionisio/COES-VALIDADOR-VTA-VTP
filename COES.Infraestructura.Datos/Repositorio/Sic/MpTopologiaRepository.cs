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
    /// Clase de acceso a datos de la tabla MP_TOPOLOGIA
    /// </summary>
    public class MpTopologiaRepository: RepositoryBase, IMpTopologiaRepository
    {
        public MpTopologiaRepository(string strConn): base(strConn)
        {
        }

        MpTopologiaHelper helper = new MpTopologiaHelper();

        public int Save(MpTopologiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mtopnomb, DbType.String, entity.Mtopnomb);
            dbProvider.AddInParameter(command, helper.Mtopversion, DbType.Int32, entity.Mtopversion);
            dbProvider.AddInParameter(command, helper.Mtopestado, DbType.Int32, entity.Mtopestado);
            dbProvider.AddInParameter(command, helper.Mtopfecha, DbType.DateTime, entity.Mtopfecha);
            dbProvider.AddInParameter(command, helper.Mtopfechafutura, DbType.DateTime, entity.Mtopfechafutura);
            dbProvider.AddInParameter(command, helper.Mtopresolucion, DbType.String, entity.Mtopresolucion);
            dbProvider.AddInParameter(command, helper.Mtopoficial, DbType.Int32, entity.Mtopoficial);
            dbProvider.AddInParameter(command, helper.Mtopusuregistro, DbType.String, entity.Mtopusuregistro);
            dbProvider.AddInParameter(command, helper.Mtopfeccreacion, DbType.DateTime, entity.Mtopfeccreacion);
            dbProvider.AddInParameter(command, helper.Mtopusumodificacion, DbType.String, entity.Mtopusumodificacion);
            dbProvider.AddInParameter(command, helper.Mtopfecmodificacion, DbType.DateTime, entity.Mtopfecmodificacion);
            dbProvider.AddInParameter(command, helper.Mtopcodipadre, DbType.Int32, entity.Mtopcodipadre);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MpTopologiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi);
            dbProvider.AddInParameter(command, helper.Mtopnomb, DbType.String, entity.Mtopnomb);
            dbProvider.AddInParameter(command, helper.Mtopversion, DbType.Int32, entity.Mtopversion);
            dbProvider.AddInParameter(command, helper.Mtopestado, DbType.Int32, entity.Mtopestado);
            dbProvider.AddInParameter(command, helper.Mtopfecha, DbType.DateTime, entity.Mtopfecha);
            dbProvider.AddInParameter(command, helper.Mtopfechafutura, DbType.DateTime, entity.Mtopfechafutura);
            dbProvider.AddInParameter(command, helper.Mtopresolucion, DbType.String, entity.Mtopresolucion);
            dbProvider.AddInParameter(command, helper.Mtopoficial, DbType.Int32, entity.Mtopoficial);
            dbProvider.AddInParameter(command, helper.Mtopusuregistro, DbType.String, entity.Mtopusuregistro);
            dbProvider.AddInParameter(command, helper.Mtopfeccreacion, DbType.DateTime, entity.Mtopfeccreacion);
            dbProvider.AddInParameter(command, helper.Mtopusumodificacion, DbType.String, entity.Mtopusumodificacion);
            dbProvider.AddInParameter(command, helper.Mtopfecmodificacion, DbType.DateTime, entity.Mtopfecmodificacion);
            dbProvider.AddInParameter(command, helper.Mtopcodipadre, DbType.Int32, entity.Mtopcodipadre);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MpTopologiaDTO GetById(int mtopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            MpTopologiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MpTopologiaDTO> List()
        {
            List<MpTopologiaDTO> entitys = new List<MpTopologiaDTO>();
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

        public List<MpTopologiaDTO> GetByCriteria()
        {
            List<MpTopologiaDTO> entitys = new List<MpTopologiaDTO>();
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

        public int Save(MpTopologiaDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopnomb, DbType.String, entity.Mtopnomb));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopversion, DbType.Int32, entity.Mtopversion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopestado, DbType.Int32, entity.Mtopestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopfecha, DbType.DateTime, entity.Mtopfecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopfechafutura, DbType.DateTime, entity.Mtopfechafutura));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopresolucion, DbType.String, entity.Mtopresolucion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopoficial, DbType.Int32, entity.Mtopoficial));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopusuregistro, DbType.String, entity.Mtopusuregistro));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopfeccreacion, DbType.DateTime, entity.Mtopfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopusumodificacion, DbType.String, entity.Mtopusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopfecmodificacion, DbType.DateTime, entity.Mtopfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodipadre, DbType.Int32, entity.Mtopcodipadre));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(MpTopologiaDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopnomb, DbType.String, entity.Mtopnomb));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopversion, DbType.Int32, entity.Mtopversion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopestado, DbType.Int32, entity.Mtopestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopfecha, DbType.DateTime, entity.Mtopfecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopfechafutura, DbType.DateTime, entity.Mtopfechafutura));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopresolucion, DbType.String, entity.Mtopresolucion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopoficial, DbType.Int32, entity.Mtopoficial));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopusuregistro, DbType.String, entity.Mtopusuregistro));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopfeccreacion, DbType.DateTime, entity.Mtopfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopusumodificacion, DbType.String, entity.Mtopusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopfecmodificacion, DbType.DateTime, entity.Mtopfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodipadre, DbType.Int32, entity.Mtopcodipadre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));

                dbCommand.ExecuteNonQuery();
            }
        }

        
        public List<MpTopologiaDTO> ListarEscenariosSddp(string fechaPeriodo, string resolucion, int identificador)
        {
            List<MpTopologiaDTO> entitys = new List<MpTopologiaDTO>();
            string strComando = string.Format(helper.SqlListarEscenariosSddp, fechaPeriodo, resolucion, identificador);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}
