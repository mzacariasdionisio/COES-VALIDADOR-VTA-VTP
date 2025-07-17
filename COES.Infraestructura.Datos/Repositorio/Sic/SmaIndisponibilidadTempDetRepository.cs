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
    /// Clase de acceso a datos de la tabla SMA_INDISPONIBILIDAD_TEMP_DET
    /// </summary>
    public class SmaIndisponibilidadTempDetRepository: RepositoryBase, ISmaIndisponibilidadTempDetRepository
    {
        public SmaIndisponibilidadTempDetRepository(string strConn): base(strConn)
        {
        }

        SmaIndisponibilidadTempDetHelper helper = new SmaIndisponibilidadTempDetHelper();

        public int Save(SmaIndisponibilidadTempDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Intdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Intcabcodi, DbType.Int32, entity.Intcabcodi);
            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, entity.Urscodi);
            dbProvider.AddInParameter(command, helper.Intdetindexiste, DbType.String, entity.Intdetindexiste);
            dbProvider.AddInParameter(command, helper.Intdettipo, DbType.String, entity.Intdettipo);
            dbProvider.AddInParameter(command, helper.Intdetbanda, DbType.Decimal, entity.Intdetbanda);
            dbProvider.AddInParameter(command, helper.Intdetmotivo, DbType.String, entity.Intdetmotivo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int SaveTransaccional(SmaIndisponibilidadTempDetDTO entity, IDbConnection connection, DbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intdetcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabcodi, DbType.Int32, entity.Intcabcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Urscodi, DbType.Int32, entity.Urscodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intdetindexiste, DbType.String, entity.Intdetindexiste));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intdettipo, DbType.String, entity.Intdettipo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intdetbanda, DbType.Decimal, entity.Intdetbanda));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intdetmotivo, DbType.String, entity.Intdetmotivo));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(SmaIndisponibilidadTempDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Intdetcodi, DbType.Int32, entity.Intdetcodi);
            dbProvider.AddInParameter(command, helper.Intcabcodi, DbType.Int32, entity.Intcabcodi);
            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, entity.Urscodi);
            dbProvider.AddInParameter(command, helper.Intdetindexiste, DbType.String, entity.Intdetindexiste);
            dbProvider.AddInParameter(command, helper.Intdettipo, DbType.String, entity.Intdettipo);
            dbProvider.AddInParameter(command, helper.Intdetbanda, DbType.Decimal, entity.Intdetbanda);
            dbProvider.AddInParameter(command, helper.Intdetmotivo, DbType.String, entity.Intdetmotivo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int intdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Intdetcodi, DbType.Int32, intdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeletePorCabTransaccional(string intcabcodis, IDbConnection connection, DbTransaction transaction)
        {
            string sql = string.Format(helper.SqlDeletePorIdsCab, intcabcodis);
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = sql;

                dbCommand.ExecuteNonQuery();
            }
        }


        public SmaIndisponibilidadTempDetDTO GetById(int intdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Intdetcodi, DbType.Int32, intdetcodi);
            SmaIndisponibilidadTempDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaIndisponibilidadTempDetDTO> List()
        {
            List<SmaIndisponibilidadTempDetDTO> entitys = new List<SmaIndisponibilidadTempDetDTO>();
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

        public List<SmaIndisponibilidadTempDetDTO> GetByCriteria()
        {
            List<SmaIndisponibilidadTempDetDTO> entitys = new List<SmaIndisponibilidadTempDetDTO>();
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

        public List<SmaIndisponibilidadTempDetDTO> ListarPorFecha(DateTime fecha)
        {
            List<SmaIndisponibilidadTempDetDTO> entitys = new List<SmaIndisponibilidadTempDetDTO>();

            string query = string.Format(helper.SqlListarPorFecha, fecha.ToString(ConstantesBase.FormatoFechaPE));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SmaIndisponibilidadTempDetDTO entity = helper.Create(dr);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
