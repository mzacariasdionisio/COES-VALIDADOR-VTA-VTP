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
    /// Clase de acceso a datos de la tabla SMA_INDISPONIBILIDAD_TEMP_CAB
    /// </summary>
    public class SmaIndisponibilidadTempCabRepository: RepositoryBase, ISmaIndisponibilidadTempCabRepository
    {
        public SmaIndisponibilidadTempCabRepository(string strConn): base(strConn)
        {
        }

        SmaIndisponibilidadTempCabHelper helper = new SmaIndisponibilidadTempCabHelper();

        public int Save(SmaIndisponibilidadTempCabDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Intcabcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Intcabfecha, DbType.DateTime, entity.Intcabfecha);
            dbProvider.AddInParameter(command, helper.Intcabusucreacion, DbType.String, entity.Intcabusucreacion);
            dbProvider.AddInParameter(command, helper.Intcabfeccreacion, DbType.DateTime, entity.Intcabfeccreacion);
            dbProvider.AddInParameter(command, helper.Intcabusumodificacion, DbType.String, entity.Intcabusumodificacion);
            dbProvider.AddInParameter(command, helper.Intcabfecmodificacion, DbType.DateTime, entity.Intcabfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int SaveTransaccional(SmaIndisponibilidadTempCabDTO entity, IDbConnection connection, DbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabfecha, DbType.DateTime, entity.Intcabfecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabusucreacion, DbType.String, entity.Intcabusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabfeccreacion, DbType.DateTime, entity.Intcabfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabusumodificacion, DbType.String, entity.Intcabusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabfecmodificacion, DbType.DateTime, entity.Intcabfecmodificacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }


        public void Update(SmaIndisponibilidadTempCabDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Intcabfecha, DbType.DateTime, entity.Intcabfecha);
            dbProvider.AddInParameter(command, helper.Intcabusucreacion, DbType.String, entity.Intcabusucreacion);
            dbProvider.AddInParameter(command, helper.Intcabfeccreacion, DbType.DateTime, entity.Intcabfeccreacion);
            dbProvider.AddInParameter(command, helper.Intcabusumodificacion, DbType.String, entity.Intcabusumodificacion);
            dbProvider.AddInParameter(command, helper.Intcabfecmodificacion, DbType.DateTime, entity.Intcabfecmodificacion);
            dbProvider.AddInParameter(command, helper.Intcabcodi, DbType.Int32, entity.Intcabcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateTransaccional(SmaIndisponibilidadTempCabDTO entity, IDbConnection connection, DbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {               
                dbCommand.CommandText = helper.SqlUpdate;

                
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabfecha, DbType.DateTime, entity.Intcabfecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabusucreacion, DbType.String, entity.Intcabusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabfeccreacion, DbType.DateTime, entity.Intcabfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabusumodificacion, DbType.String, entity.Intcabusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabfecmodificacion, DbType.DateTime, entity.Intcabfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intcabcodi, DbType.Int32, entity.Intcabcodi));
                dbCommand.ExecuteNonQuery();
            }
        }
        

        public void Delete(int intcabcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Intcabcodi, DbType.Int32, intcabcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaIndisponibilidadTempCabDTO GetById(int intcabcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Intcabcodi, DbType.Int32, intcabcodi);
            SmaIndisponibilidadTempCabDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaIndisponibilidadTempCabDTO> List()
        {
            List<SmaIndisponibilidadTempCabDTO> entitys = new List<SmaIndisponibilidadTempCabDTO>();
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

        public List<SmaIndisponibilidadTempCabDTO> GetByCriteria()
        {
            List<SmaIndisponibilidadTempCabDTO> entitys = new List<SmaIndisponibilidadTempCabDTO>();
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

        public SmaIndisponibilidadTempCabDTO ObtenerPorFecha(DateTime fecha)
        {
            SmaIndisponibilidadTempCabDTO entity = null;

            string query = string.Format(helper.SqlObtenerPorFecha, fecha.ToString(ConstantesBase.FormatoFechaPE));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
