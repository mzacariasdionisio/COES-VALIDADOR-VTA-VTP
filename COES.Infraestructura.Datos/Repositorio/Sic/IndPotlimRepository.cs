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
    /// Clase de acceso a datos de la tabla IND_POTLIM
    /// </summary>
    public class IndPotlimRepository : RepositoryBase, IIndPotlimRepository
    {
        public IndPotlimRepository(string strConn) : base(strConn)
        {
        }

        IndPotlimHelper helper = new IndPotlimHelper();

        public int Save(IndPotlimDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Potlimcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Potlimmw, DbType.Decimal, entity.Potlimmw);
            dbProvider.AddInParameter(command, helper.Potlimnombre, DbType.String, entity.Potlimnombre);
            dbProvider.AddInParameter(command, helper.Potlimusucreacion, DbType.String, entity.Potlimusucreacion);
            dbProvider.AddInParameter(command, helper.Potlimfeccreacion, DbType.DateTime, entity.Potlimfeccreacion);
            dbProvider.AddInParameter(command, helper.Potlimusumodificacion, DbType.String, entity.Potlimusumodificacion);
            dbProvider.AddInParameter(command, helper.Potlimfecmodificacion, DbType.DateTime, entity.Potlimfecmodificacion);
            dbProvider.AddInParameter(command, helper.Potlimfechaini, DbType.DateTime, entity.Potlimfechaini);
            dbProvider.AddInParameter(command, helper.Potlimfechafin, DbType.DateTime, entity.Potlimfechafin);
            dbProvider.AddInParameter(command, helper.Potlimestado, DbType.Int32, entity.Potlimestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IndPotlimDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Potlimcodi, DbType.Int32, entity.Potlimcodi);
            dbProvider.AddInParameter(command, helper.Potlimmw, DbType.Decimal, entity.Potlimmw);
            dbProvider.AddInParameter(command, helper.Potlimnombre, DbType.String, entity.Potlimnombre);
            dbProvider.AddInParameter(command, helper.Potlimusucreacion, DbType.String, entity.Potlimusucreacion);
            dbProvider.AddInParameter(command, helper.Potlimfeccreacion, DbType.DateTime, entity.Potlimfeccreacion);
            dbProvider.AddInParameter(command, helper.Potlimusumodificacion, DbType.String, entity.Potlimusumodificacion);
            dbProvider.AddInParameter(command, helper.Potlimfecmodificacion, DbType.DateTime, entity.Potlimfecmodificacion);
            dbProvider.AddInParameter(command, helper.Potlimfechaini, DbType.DateTime, entity.Potlimfechaini);
            dbProvider.AddInParameter(command, helper.Potlimfechafin, DbType.DateTime, entity.Potlimfechafin);
            dbProvider.AddInParameter(command, helper.Potlimestado, DbType.Int32, entity.Potlimestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdatePartial(IndPotlimDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdatePartial);

            dbProvider.AddInParameter(command, helper.Potlimmw, DbType.Decimal, entity.Potlimmw);
            dbProvider.AddInParameter(command, helper.Potlimusumodificacion, DbType.String, entity.Potlimusumodificacion);
            dbProvider.AddInParameter(command, helper.Potlimfecmodificacion, DbType.DateTime, entity.Potlimfecmodificacion);
            dbProvider.AddInParameter(command, helper.Potlimcodi, DbType.Int32, entity.Potlimcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int potlimcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Potlimcodi, DbType.Int32, potlimcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndPotlimDTO GetById(int potlimcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Potlimcodi, DbType.Int32, potlimcodi);
            IndPotlimDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndPotlimDTO> List()
        {
            List<IndPotlimDTO> entitys = new List<IndPotlimDTO>();
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

        public List<IndPotlimDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin)
        {
            List<IndPotlimDTO> entitys = new List<IndPotlimDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Potlimfechaini, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Potlimfechafin, DbType.DateTime, fechaFin);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateMap(dr));
                }
            }

            return entitys;
        }

        public int Save(IndPotlimDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimmw, DbType.Decimal, entity.Potlimmw));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimnombre, DbType.String, entity.Potlimnombre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimusucreacion, DbType.String, entity.Potlimusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimfeccreacion, DbType.DateTime, entity.Potlimfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimusumodificacion, DbType.String, entity.Potlimusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimfecmodificacion, DbType.DateTime, entity.Potlimfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimfechaini, DbType.DateTime, entity.Potlimfechaini));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimfechafin, DbType.DateTime, entity.Potlimfechafin));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Potlimestado, DbType.Int32, entity.Potlimestado));

                dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();
                return id;
            }
        }

        public void UpdateEstado(IndPotlimDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstado);

            dbProvider.AddInParameter(command, helper.Potlimestado, DbType.Decimal, entity.Potlimestado);
            dbProvider.AddInParameter(command, helper.Potlimcodi, DbType.Int32, entity.Potlimcodi);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
