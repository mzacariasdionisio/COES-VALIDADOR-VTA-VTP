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
    /// Clase de acceso a datos de la tabla SIO_CAMBIOPRIE
    /// </summary>
    public class SioCambioprieRepository : RepositoryBase, ISioCambioprieRepository
    {
        public SioCambioprieRepository(string strConn) : base(strConn)
        {
        }

        SioCambioprieHelper helper = new SioCambioprieHelper();

        public int Save(SioCambioprieDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Campricodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi2, DbType.Int32, entity.Emprcodi2);
            dbProvider.AddInParameter(command, helper.Camprifecmodificacion, DbType.DateTime, entity.Camprifecmodificacion);
            dbProvider.AddInParameter(command, helper.Campriusumodificacion, DbType.String, entity.Campriusumodificacion);
            dbProvider.AddInParameter(command, helper.Camprivalor, DbType.String, entity.Camprivalor);
            dbProvider.AddInParameter(command, helper.Cabpricodi, DbType.Int32, entity.Cabpricodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SioCambioprieDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi2, DbType.Int32, entity.Emprcodi2);
            dbProvider.AddInParameter(command, helper.Camprifecmodificacion, DbType.DateTime, entity.Camprifecmodificacion);
            dbProvider.AddInParameter(command, helper.Campriusumodificacion, DbType.String, entity.Campriusumodificacion);
            dbProvider.AddInParameter(command, helper.Camprivalor, DbType.String, entity.Camprivalor);
            dbProvider.AddInParameter(command, helper.Cabpricodi, DbType.Int32, entity.Cabpricodi);
            dbProvider.AddInParameter(command, helper.Campricodi, DbType.Int32, entity.Campricodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int campricodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Campricodi, DbType.Int32, campricodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SioCambioprieDTO GetById(int campricodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Campricodi, DbType.Int32, campricodi);
            SioCambioprieDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SioCambioprieDTO> List()
        {
            List<SioCambioprieDTO> entitys = new List<SioCambioprieDTO>();
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

        public List<SioCambioprieDTO> GetByCriteria()
        {
            List<SioCambioprieDTO> entitys = new List<SioCambioprieDTO>();
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

        public int Save(SioCambioprieDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Campricodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Barrcodi, DbType.Int32, entity.Barrcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi2, DbType.Int32, entity.Emprcodi2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Camprifecmodificacion, DbType.DateTime, entity.Camprifecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Campriusumodificacion, DbType.String, entity.Campriusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Camprivalor, DbType.String, entity.Camprivalor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cabpricodi, DbType.Int32, entity.Cabpricodi));

                dbCommand.ExecuteNonQuery();
                return id;

            }
        }
    }
}
