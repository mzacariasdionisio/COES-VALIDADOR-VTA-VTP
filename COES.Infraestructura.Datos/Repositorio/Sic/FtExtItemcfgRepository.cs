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
    /// Clase de acceso a datos de la tabla FT_EXT_ITEMCFG
    /// </summary>
    public class FtExtItemcfgRepository : RepositoryBase, IFtExtItemcfgRepository
    {
        public FtExtItemcfgRepository(string strConn) : base(strConn)
        {
        }

        FtExtItemcfgHelper helper = new FtExtItemcfgHelper();

        public int Save(FtExtItemcfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fitcfgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, entity.Ftitcodi);
            dbProvider.AddInParameter(command, helper.Fitcfgflagcomentario, DbType.String, entity.Fitcfgflagcomentario);
            dbProvider.AddInParameter(command, helper.Fitcfgflagvalorconf, DbType.String, entity.Fitcfgflagvalorconf);
            dbProvider.AddInParameter(command, helper.Fitcfgflagbloqedicion, DbType.String, entity.Fitcfgflagbloqedicion);
            dbProvider.AddInParameter(command, helper.Fitcfgflagsustento, DbType.String, entity.Fitcfgflagsustento);
            dbProvider.AddInParameter(command, helper.Fitcfgflagsustentoconf, DbType.String, entity.Fitcfgflagsustentoconf);
            dbProvider.AddInParameter(command, helper.Fitcfgflaginstructivo, DbType.String, entity.Fitcfgflaginstructivo);
            dbProvider.AddInParameter(command, helper.Fitcfgflagvalorobligatorio, DbType.String, entity.Fitcfgflagvalorobligatorio);
            dbProvider.AddInParameter(command, helper.Fitcfgflagsustentoobligatorio, DbType.String, entity.Fitcfgflagsustentoobligatorio);
            dbProvider.AddInParameter(command, helper.Fitcfginstructivo, DbType.String, entity.Fitcfginstructivo);
            dbProvider.AddInParameter(command, helper.Fitcfgusucreacion, DbType.String, entity.Fitcfgusucreacion);
            dbProvider.AddInParameter(command, helper.Fitcfgfeccreacion, DbType.DateTime, entity.Fitcfgfeccreacion);
            dbProvider.AddInParameter(command, helper.Fitcfgusumodificacion, DbType.String, entity.Fitcfgusumodificacion);
            dbProvider.AddInParameter(command, helper.Fitcfgfecmodificacion, DbType.DateTime, entity.Fitcfgfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftfmtcodi, DbType.Int32, entity.Ftfmtcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtItemcfgDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftitcodi, DbType.Int32, entity.Ftitcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagcomentario, DbType.String, entity.Fitcfgflagcomentario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagvalorconf, DbType.String, entity.Fitcfgflagvalorconf));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagbloqedicion, DbType.String, entity.Fitcfgflagbloqedicion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagsustento, DbType.String, entity.Fitcfgflagsustento));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagsustentoconf, DbType.String, entity.Fitcfgflagsustentoconf));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflaginstructivo, DbType.String, entity.Fitcfgflaginstructivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagvalorobligatorio, DbType.String, entity.Fitcfgflagvalorobligatorio));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagsustentoobligatorio, DbType.String, entity.Fitcfgflagsustentoobligatorio));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfginstructivo, DbType.String, entity.Fitcfginstructivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgusucreacion, DbType.String, entity.Fitcfgusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgfeccreacion, DbType.DateTime, entity.Fitcfgfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgusumodificacion, DbType.String, entity.Fitcfgusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgfecmodificacion, DbType.DateTime, entity.Fitcfgfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftfmtcodi, DbType.Int32, entity.Ftfmtcodi));


                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(FtExtItemcfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, entity.Ftitcodi);
            dbProvider.AddInParameter(command, helper.Fitcfgflagcomentario, DbType.String, entity.Fitcfgflagcomentario);
            dbProvider.AddInParameter(command, helper.Fitcfgflagvalorconf, DbType.String, entity.Fitcfgflagvalorconf);
            dbProvider.AddInParameter(command, helper.Fitcfgflagbloqedicion, DbType.String, entity.Fitcfgflagbloqedicion);
            dbProvider.AddInParameter(command, helper.Fitcfgflagsustento, DbType.String, entity.Fitcfgflagsustento);
            dbProvider.AddInParameter(command, helper.Fitcfgflagsustentoconf, DbType.String, entity.Fitcfgflagsustentoconf);
            dbProvider.AddInParameter(command, helper.Fitcfgflaginstructivo, DbType.String, entity.Fitcfgflaginstructivo);
            dbProvider.AddInParameter(command, helper.Fitcfgflagvalorobligatorio, DbType.String, entity.Fitcfgflagvalorobligatorio);
            dbProvider.AddInParameter(command, helper.Fitcfgflagsustentoobligatorio, DbType.String, entity.Fitcfgflagsustentoobligatorio);
            dbProvider.AddInParameter(command, helper.Fitcfginstructivo, DbType.String, entity.Fitcfginstructivo);
            dbProvider.AddInParameter(command, helper.Fitcfgusucreacion, DbType.String, entity.Fitcfgusucreacion);
            dbProvider.AddInParameter(command, helper.Fitcfgfeccreacion, DbType.DateTime, entity.Fitcfgfeccreacion);
            dbProvider.AddInParameter(command, helper.Fitcfgusumodificacion, DbType.String, entity.Fitcfgusumodificacion);
            dbProvider.AddInParameter(command, helper.Fitcfgfecmodificacion, DbType.DateTime, entity.Fitcfgfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftfmtcodi, DbType.Int32, entity.Ftfmtcodi);
            dbProvider.AddInParameter(command, helper.Fitcfgcodi, DbType.Int32, entity.Fitcfgcodi);
            dbProvider.ExecuteNonQuery(command);
        }


        public void Update(FtExtItemcfgDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftitcodi, DbType.Int32, entity.Ftitcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagcomentario, DbType.String, entity.Fitcfgflagcomentario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagvalorconf, DbType.String, entity.Fitcfgflagvalorconf));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagbloqedicion, DbType.String, entity.Fitcfgflagbloqedicion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagsustento, DbType.String, entity.Fitcfgflagsustento));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagsustentoconf, DbType.String, entity.Fitcfgflagsustentoconf));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflaginstructivo, DbType.String, entity.Fitcfgflaginstructivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagvalorobligatorio, DbType.String, entity.Fitcfgflagvalorobligatorio));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgflagsustentoobligatorio, DbType.String, entity.Fitcfgflagsustentoobligatorio));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfginstructivo, DbType.String, entity.Fitcfginstructivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgusucreacion, DbType.String, entity.Fitcfgusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgfeccreacion, DbType.DateTime, entity.Fitcfgfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgusumodificacion, DbType.String, entity.Fitcfgusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgfecmodificacion, DbType.DateTime, entity.Fitcfgfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftfmtcodi, DbType.Int32, entity.Ftfmtcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgcodi, DbType.Int32, entity.Fitcfgcodi));


                dbCommand.ExecuteNonQuery();

            }
        }

        public void Delete(int fitcfgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fitcfgcodi, DbType.Int32, fitcfgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int fitcfgcodi, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlDelete;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fitcfgcodi, DbType.Int32, fitcfgcodi));

                dbCommand.ExecuteNonQuery();
            }
        }



        public FtExtItemcfgDTO GetById(int fitcfgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fitcfgcodi, DbType.Int32, fitcfgcodi);
            FtExtItemcfgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtItemcfgDTO> List()
        {
            List<FtExtItemcfgDTO> entitys = new List<FtExtItemcfgDTO>();
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

        public List<FtExtItemcfgDTO> GetByCriteria()
        {
            List<FtExtItemcfgDTO> entitys = new List<FtExtItemcfgDTO>();
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

        public List<FtExtItemcfgDTO> ListarPorFormato(int ftfmtcodi)
        {
            List<FtExtItemcfgDTO> entitys = new List<FtExtItemcfgDTO>();
            var sqlQuery = string.Format(helper.SqlListarPorFormato, ftfmtcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    FtExtItemcfgDTO entity = helper.Create(dr);

                    int iFtpropcodi = dr.GetOrdinal(this.helper.Ftpropcodi);
                    if (!dr.IsDBNull(iFtpropcodi)) entity.Ftpropcodi = Convert.ToInt32(dr.GetValue(iFtpropcodi));

                    int iConcepcodi = dr.GetOrdinal(this.helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iPropcodi = dr.GetOrdinal(this.helper.Propcodi);
                    if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeletePorFormato(int ftfmtcodi)
        {
            var sqlQuery = string.Format(helper.SqlEliminarPorFormato, ftfmtcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

            }
        }

        public List<FtExtItemcfgDTO> ListarPorIds(string strFitcfgcodis)
        {
            List<FtExtItemcfgDTO> entitys = new List<FtExtItemcfgDTO>();
            var sqlQuery = string.Format(helper.SqlListarPorIds, strFitcfgcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

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
