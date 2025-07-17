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
    /// Clase de acceso a datos de la tabla IND_POTLIM_UNIDAD
    /// </summary>
    public class IndPotlimUnidadRepository : RepositoryBase, IIndPotlimUnidadRepository
    {
        public IndPotlimUnidadRepository(string strConn) : base(strConn)
        {
        }

        IndPotlimUnidadHelper helper = new IndPotlimUnidadHelper();

        public int Save(IndPotlimUnidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Equlimcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equlimpotefectiva, DbType.Decimal, entity.Equlimpotefectiva);
            dbProvider.AddInParameter(command, helper.Equlimusumodificacion, DbType.String, entity.Equlimusumodificacion);
            dbProvider.AddInParameter(command, helper.Equlimfecmodificacion, DbType.DateTime, entity.Equlimfecmodificacion);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Potlimcodi, DbType.Int32, entity.Potlimcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IndPotlimUnidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equlimcodi, DbType.Int32, entity.Equlimcodi);
            dbProvider.AddInParameter(command, helper.Equlimpotefectiva, DbType.Decimal, entity.Equlimpotefectiva);
            dbProvider.AddInParameter(command, helper.Equlimusumodificacion, DbType.String, entity.Equlimusumodificacion);
            dbProvider.AddInParameter(command, helper.Equlimfecmodificacion, DbType.DateTime, entity.Equlimfecmodificacion);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Potlimcodi, DbType.Int32, entity.Potlimcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int equlimcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Equlimcodi, DbType.Int32, equlimcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndPotlimUnidadDTO GetById(int equlimcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Equlimcodi, DbType.Int32, equlimcodi);
            IndPotlimUnidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndPotlimUnidadDTO> List()
        {
            List<IndPotlimUnidadDTO> entitys = new List<IndPotlimUnidadDTO>();
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

        public List<IndPotlimUnidadDTO> GetByCriteria()
        {
            List<IndPotlimUnidadDTO> entitys = new List<IndPotlimUnidadDTO>();
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

        public int Save(IndPotlimUnidadDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equlimcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equlimpotefectiva, DbType.Decimal, entity.Equlimpotefectiva));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equlimusumodificacion, DbType.String, entity.Equlimusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equlimfecmodificacion, DbType.DateTime, entity.Equlimfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equipadre, DbType.Int32, entity.Equipadre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimcodi, DbType.Int32, entity.Potlimcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));

                dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();
                return id;
            }
        }
    }
}
