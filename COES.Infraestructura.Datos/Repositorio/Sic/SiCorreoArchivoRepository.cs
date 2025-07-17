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
    /// Clase de acceso a datos de la tabla SI_CORREO_ARCHIVO
    /// </summary>
    public class SiCorreoArchivoRepository: RepositoryBase, ISiCorreoArchivoRepository
    {
        public SiCorreoArchivoRepository(string strConn): base(strConn)
        {
        }

        SiCorreoArchivoHelper helper = new SiCorreoArchivoHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiCorreoArchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Earchcodi, DbType.Int32, entity.Earchcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Corrcodi, DbType.Int32, entity.Corrcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Earchtipo, DbType.Int32, entity.Earchtipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Earchnombreoriginal, DbType.String, entity.Earchnombreoriginal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Earchnombrefisico, DbType.String, entity.Earchnombrefisico));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Earchorden, DbType.Int32, entity.Earchorden));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Earchestado, DbType.Int32, entity.Earchestado));

            command.ExecuteNonQuery();
            return entity.Earchcodi;
        }

        public int Save(SiCorreoArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Earchcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Corrcodi, DbType.Int32, entity.Corrcodi);
            dbProvider.AddInParameter(command, helper.Earchtipo, DbType.Int32, entity.Earchtipo);
            dbProvider.AddInParameter(command, helper.Earchnombreoriginal, DbType.String, entity.Earchnombreoriginal);
            dbProvider.AddInParameter(command, helper.Earchnombrefisico, DbType.String, entity.Earchnombrefisico);
            dbProvider.AddInParameter(command, helper.Earchorden, DbType.Int32, entity.Earchorden);
            dbProvider.AddInParameter(command, helper.Earchestado, DbType.Int32, entity.Earchestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiCorreoArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Earchcodi, DbType.Int32, entity.Earchcodi);
            dbProvider.AddInParameter(command, helper.Corrcodi, DbType.Int32, entity.Corrcodi);
            dbProvider.AddInParameter(command, helper.Earchtipo, DbType.Int32, entity.Earchtipo);
            dbProvider.AddInParameter(command, helper.Earchnombreoriginal, DbType.String, entity.Earchnombreoriginal);
            dbProvider.AddInParameter(command, helper.Earchnombrefisico, DbType.String, entity.Earchnombrefisico);
            dbProvider.AddInParameter(command, helper.Earchorden, DbType.Int32, entity.Earchorden);
            dbProvider.AddInParameter(command, helper.Earchestado, DbType.Int32, entity.Earchestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int earchcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Earchcodi, DbType.Int32, earchcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiCorreoArchivoDTO GetById(int earchcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Earchcodi, DbType.Int32, earchcodi);
            SiCorreoArchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiCorreoArchivoDTO> List()
        {
            List<SiCorreoArchivoDTO> entitys = new List<SiCorreoArchivoDTO>();
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

        public List<SiCorreoArchivoDTO> GetByCriteria()
        {
            List<SiCorreoArchivoDTO> entitys = new List<SiCorreoArchivoDTO>();
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

        public List<SiCorreoArchivoDTO> GetByCorreos(string corrcodis)
        {
            List<SiCorreoArchivoDTO> entitys = new List<SiCorreoArchivoDTO>();
            string sql = string.Format(helper.SqlGetByCorreos, corrcodis);
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
