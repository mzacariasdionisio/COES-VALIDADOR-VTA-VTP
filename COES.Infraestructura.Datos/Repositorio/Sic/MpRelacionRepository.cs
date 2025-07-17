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
    /// Clase de acceso a datos de la tabla MP_RELACION
    /// </summary>
    public class MpRelacionRepository: RepositoryBase, IMpRelacionRepository
    {
        public MpRelacionRepository(string strConn): base(strConn)
        {
        }

        MpRelacionHelper helper = new MpRelacionHelper();

        public void Save(MpRelacionDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                
                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtrelcodi, DbType.Int32, entity.Mtrelcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi1, DbType.Int32, entity.Mrecurcodi1));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi2, DbType.Int32, entity.Mrecurcodi2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrelvalor, DbType.Decimal, entity.Mrelvalor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrelusumodificacion, DbType.String, entity.Mrelusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrelfecmodificacion, DbType.DateTime, entity.Mrelfecmodificacion));

                dbCommand.ExecuteNonQuery();
                
            }
        }

        public void Update(MpRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi);
            dbProvider.AddInParameter(command, helper.Mtrelcodi, DbType.Int32, entity.Mtrelcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi1, DbType.Int32, entity.Mrecurcodi1);
            dbProvider.AddInParameter(command, helper.Mrecurcodi2, DbType.Int32, entity.Mrecurcodi2);
            dbProvider.AddInParameter(command, helper.Mrelvalor, DbType.Decimal, entity.Mrelvalor);
            dbProvider.AddInParameter(command, helper.Mrelusumodificacion, DbType.String, entity.Mrelusumodificacion);
            dbProvider.AddInParameter(command, helper.Mrelfecmodificacion, DbType.DateTime, entity.Mrelfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi, int mtrelcodi, int mrecurcodi1, int mrecurcodi2)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mtrelcodi, DbType.Int32, mtrelcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi1, DbType.Int32, mrecurcodi1);
            dbProvider.AddInParameter(command, helper.Mrecurcodi2, DbType.Int32, mrecurcodi2);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi, int mtrelcodi, int mrecurcodi1, int mrecurcodi2, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                
                dbCommand.CommandText = helper.SqlDelete;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtrelcodi, DbType.Int32, mtrelcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi1, DbType.Int32, mrecurcodi1));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi2, DbType.Int32, mrecurcodi2));
                
                dbCommand.ExecuteNonQuery();
                
            }
        }

        public MpRelacionDTO GetById(int mtopcodi, int mtrelcodi, int mrecurcodi1, int mrecurcodi2)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mtrelcodi, DbType.Int32, mtrelcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi1, DbType.Int32, mrecurcodi1);
            dbProvider.AddInParameter(command, helper.Mrecurcodi2, DbType.Int32, mrecurcodi2);
            MpRelacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MpRelacionDTO> List()
        {
            List<MpRelacionDTO> entitys = new List<MpRelacionDTO>();
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

        public List<MpRelacionDTO> GetByCriteria()
        {
            List<MpRelacionDTO> entitys = new List<MpRelacionDTO>();
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

        public List<MpRelacionDTO> ListarPorTopologia(int mtopcodi)
        {
            List<MpRelacionDTO> entitys = new List<MpRelacionDTO>();
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

        public List<MpRelacionDTO> ListarPorTopologiaYRecurso(int mtopcodi, int recurcodi1)
        {
            List<MpRelacionDTO> entitys = new List<MpRelacionDTO>();
            string query = string.Format(helper.SqlListarByTopologiaYRecurso, mtopcodi, recurcodi1);
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
        
    }
}
