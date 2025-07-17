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
    /// Clase de acceso a datos de la tabla CB_VERSION
    /// </summary>
    public class CbVersionRepository : RepositoryBase, ICbVersionRepository
    {
        public CbVersionRepository(string strConn) : base(strConn)
        {
        }

        readonly CbVersionHelper helper = new CbVersionHelper();

        public int Save(CbVersionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbvercodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvcodi, DbType.Int32, entity.Cbenvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbvernumversion, DbType.Int32, entity.Cbvernumversion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbverestado, DbType.String, entity.Cbverestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbverusucreacion, DbType.String, entity.Cbverusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbverfeccreacion, DbType.DateTime, entity.Cbverfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbveroperacion, DbType.Int32, entity.Cbveroperacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbverdescripcion, DbType.String, entity.Cbverdescripcion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbverconexion, DbType.Int32, entity.Cbverconexion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbvertipo, DbType.Int32, entity.Cbvertipo));

            command.ExecuteNonQuery();
            return id;
        }

        public void Update(CbVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbenvcodi, DbType.Int32, entity.Cbenvcodi);
            dbProvider.AddInParameter(command, helper.Cbvercodi, DbType.Int32, entity.Cbvercodi);
            dbProvider.AddInParameter(command, helper.Cbvernumversion, DbType.Int32, entity.Cbvernumversion);
            dbProvider.AddInParameter(command, helper.Cbverestado, DbType.String, entity.Cbverestado);
            dbProvider.AddInParameter(command, helper.Cbverusucreacion, DbType.String, entity.Cbverusucreacion);
            dbProvider.AddInParameter(command, helper.Cbverfeccreacion, DbType.DateTime, entity.Cbverfeccreacion);
            dbProvider.AddInParameter(command, helper.Cbveroperacion, DbType.Int32, entity.Cbveroperacion);
            dbProvider.AddInParameter(command, helper.Cbverdescripcion, DbType.String, entity.Cbverdescripcion);
            dbProvider.AddInParameter(command, helper.Cbverconexion, DbType.Int32, entity.Cbverconexion);
            dbProvider.AddInParameter(command, helper.Cbvertipo, DbType.Int32, entity.Cbvertipo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbvercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbvercodi, DbType.Int32, cbvercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbVersionDTO GetById(int cbvercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbvercodi, DbType.Int32, cbvercodi);
            CbVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbVersionDTO> List()
        {
            List<CbVersionDTO> entitys = new List<CbVersionDTO>();
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

        public List<CbVersionDTO> GetByCriteria(string cbenvcodi)
        {
            List<CbVersionDTO> entitys = new List<CbVersionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, cbenvcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void CambiarEstado(string versioncodis, string estado)
        {
            int resul = -1;
            string sql = string.Format(helper.SqlCambiarEstado, estado, versioncodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            resul = dbProvider.ExecuteNonQuery(command);
        }

        public List<CbVersionDTO> GetByPeriodoyEstado(string mesVigencia, string estados)
        {
            List<CbVersionDTO> entitys = new List<CbVersionDTO>();
            string sql = string.Format(helper.SqlGetByPeriodoyEstado, estados, mesVigencia);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
